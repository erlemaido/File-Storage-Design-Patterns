using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AppDbContext _context;
        private const int PageValue = 1;
        private const int PageSize = 8;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index(int page = PageValue, int pageSize = PageSize)
        {
            var skip = (page - 1) * pageSize;
            var appDbContext = _context.Products
                .Skip(skip)
                .Take(pageSize)
                .OrderBy(x => x.Title)
                .Include(p => p.ProductStateType)
                .Include(p => p.ProductPictures);

            var list = await appDbContext.ToListAsync();
            var view = new ProductIndexViewModel {Page = page, Products = CreateProductViewModels(list)};

            return View(view);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.ProductStateType)
                .Include(p => p.ProductPictures)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            var viewModel = CreateProductViewModel(product);

            return View(viewModel);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["ProductStateTypeCode"] =
                new SelectList(_context.ProductStatusTypes, "ProductStateTypeCode", "Title");
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("ProductId,ProductCode,Title,Price,ProductStateTypeCode,PictureFiles")]
            ProductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                var product = CreateProduct(productViewModel);

                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new {id = product.ProductId});
            }

            ViewData["ProductStateTypeCode"] = new SelectList(_context.ProductStatusTypes, "ProductStateTypeCode",
                "Title", productViewModel.ProductStateTypeCode);
            return View(productViewModel);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.ProductStateType)
                .Include(p => p.ProductPictures)
                .FirstOrDefaultAsync(m => m.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            var productViewModel = CreateProductViewModel(product);

            ViewData["ProductStateTypeCode"] = new SelectList(_context.ProductStatusTypes, "ProductStateTypeCode",
                "Title", product.ProductStateTypeCode);
            return View(productViewModel);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("ProductId,ProductCode,Title,Price,ProductStateTypeCode,PictureFiles")]
            ProductViewModel productViewModel)
        {
            if (id != productViewModel.ProductId)
            {
                return NotFound();
            }

            var product = CreateProduct(productViewModel);
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Details), new {id = product.ProductId});
            }

            ViewData["ProductStateTypeCode"] = new SelectList(_context.ProductStatusTypes, "ProductStateTypeCode",
                "Title", product.ProductStateTypeCode);
            return View(productViewModel);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.ProductStateType)
                .Include(p => p.ProductPictures)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            var viewModel = CreateProductViewModel(product);

            return View(viewModel);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult DeleteAll()
        {
            _context.ProductPictures.RemoveRange(_context.ProductPictures);
            _context.Products.RemoveRange(_context.Products);
            _context.SaveChanges();
            ViewData["ProductStateTypeCode"] =
                new SelectList(_context.ProductStatusTypes, "ProductStateTypeCode", "Title");
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }

        private static async Task<byte[]> ConvertToBytes(IFormFile image)
        {
            await using var memoryStream = new MemoryStream();
            await image.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }

        private static ProductViewModel CreateProductViewModel(Product product)
        {
            var viewModel = new ProductViewModel()
            {
                Title = product.Title,
                Price = product.Price,
                ProductCode = product.ProductCode,
                ProductId = product.ProductId,
                ProductStateType = product.ProductStateType,
                ProductStateTypeCode = product.ProductStateTypeCode,
                PictureBytes = product.ProductPictures.Select(p => p.Picture).ToList()
            };
            return viewModel;
        }

        private Product CreateProduct(ProductViewModel productViewModel)
        {
            var product = new Product()
            {
                Title = productViewModel.Title,
                ProductId = productViewModel.ProductId,
                Price = productViewModel.Price,
                ProductCode = productViewModel.ProductCode,
                ProductStateType = productViewModel.ProductStateType,
                ProductStateTypeCode = productViewModel.ProductStateTypeCode,
                ProductPictures = productViewModel.PictureFiles != null && productViewModel.PictureFiles.Count != 0
                    ? productViewModel.PictureFiles.Select((p, i) =>
                    {
                        var productPicturesSize = _context.ProductPictures
                            .Where(m => m.ProductId == productViewModel.ProductId)
                            .ToList().Count;

                        var bytes = ConvertToBytes(p);
                        var productPicture = new ProductPicture
                        {
                            SeqNr = i + productPicturesSize,
                            ProductId = productViewModel.ProductId,
                            Picture = bytes.Result
                        };
                        return productPicture;
                    }).ToList()
                    : new List<ProductPicture>()
            };
            return product;
        }

        private static IEnumerable<ProductViewModel> CreateProductViewModels(IEnumerable<Product> list)
        {
            var productViewModels = list.Select(product => new ProductViewModel()
                {
                    ProductId = product.ProductId,
                    ProductCode = product.ProductCode,
                    Title = product.Title,
                    Price = product.Price,
                    ProductStateTypeCode = product.ProductStateTypeCode,
                    ProductStateType = product.ProductStateType,
                    PictureBytes = (product.ProductPictures.Count > 0
                        ? new List<byte[]> {product.ProductPictures?.FirstOrDefault()?.Picture}
                        : new List<byte[]>())
                })
                .ToList();
            return productViewModels;
        }
    }
}