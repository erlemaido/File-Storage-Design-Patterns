using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Data;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class ProductsGeneratorController : Controller
    {
        private readonly AppDbContext _context;
        private static readonly Random Random = new();
        private readonly IWebHostEnvironment _webHostEnvironment;
        private const int MinPicturesPerProduct = 0;
        private const int MaxPicturesPerProduct = 20;

        public ProductsGeneratorController(AppDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _webHostEnvironment = hostEnvironment;
        }

        // GET: ProductsGenerator/Create
        public IActionResult Create()
        {
            ViewData["ProductStateTypeCode"] =
                new SelectList(_context.ProductStatusTypes, "ProductStateTypeCode", "Title");
            return View();
        }

        // POST: ProductsGenerator/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("ProductId,ProductCode,Title,Price,ProductStateTypeCode,PictureFiles,NumberOfItems")]
            ProductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                return await GenerateProduct(productViewModel.NumberOfItems, productViewModel.PictureFiles);
            }

            ViewData["ProductStateTypeCode"] = new SelectList(_context.ProductStatusTypes, "ProductStateTypeCode",
                "Title", productViewModel.ProductStateTypeCode);
            return View(productViewModel);
        }

        public async Task<IActionResult> GenerateProduct(int size, List<IFormFile> allPossiblePics)
        {
            var startTime = DateTime.Now;
            Console.WriteLine("-------------------------------------------------------------START Time: " + startTime);
            for (var i = 0; i < size; i++)
            {
                Console.WriteLine("ADDING product nr: " + i);
                var product = CreateRandomProduct(Convert.ToInt32(i));


                if (allPossiblePics != null && allPossiblePics.Any())
                {
                    Console.WriteLine("ADDING pictures for product nr: " + i);
                    AddRandomProductPictures(allPossiblePics, product);
                    Console.WriteLine("DONE with adding pictures for product nr: " + i);
                }

                try
                {
                    await _context.AddRangeAsync(product);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }
            }

            await _context.SaveChangesAsync();
            var endTime = DateTime.Now;
            Console.WriteLine("-------------------------------------------------------------END Time: " + endTime);
            Console.WriteLine("-------------------------------------------------------------TOOK: " +
                              endTime.Subtract(startTime).Milliseconds + " ms");

            return RedirectToAction(nameof(Index), "Products");
        }

        private void AddRandomProductPictures(IReadOnlyList<IFormFile> allPossiblePics, Product product)
        {
            var productPictures = new List<ProductPicture>();
            var nrOfProductPictures = Random.Next(MinPicturesPerProduct,
                MaxPicturesPerProduct <= allPossiblePics.Count
                    ? MaxPicturesPerProduct + 1
                    : allPossiblePics.Count + 1);

            var unusedIndices = Enumerable.Range(0, allPossiblePics.Count).ToArray();

            for (var i = 0; i < nrOfProductPictures; i++)
            {
                var randomPictureIndex = GetRandomNumberFromArray(allPossiblePics.Count, unusedIndices);
                unusedIndices = unusedIndices.Where(x => !x.Equals(randomPictureIndex)).ToArray();
                var picture = allPossiblePics[randomPictureIndex];
                var pictureUrl = UploadFile(picture, product.ProductCode);
                productPictures.Add(new ProductPicture()
                {
                    PictureUrl = pictureUrl,
                    Product = product,
                    SeqNr = Convert.ToInt32(i) + 1
                });
            }

            product.ProductPictures = productPictures;
        }

        private static Product CreateRandomProduct(int i)
        {
            return new()
            {
                Title = "Kaup" + i + " " + GenerateRandomTitle(10),
                Price = GenerateRandomPrice(0.01, 9999.99),
                ProductCode = GenerateRandomProductCode(6),
                ProductStateTypeCode = Random.Next(1, 5)
            };
        }

        private static string GenerateRandomProductCode(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[Random.Next(s.Length)]).ToArray());
        }

        private static string GenerateRandomTitle(int length)
        {
            const string chars = "abcdefghijklmnoprstuv ";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[Random.Next(s.Length)]).ToArray());
        }

        private static decimal GenerateRandomPrice(double min, double max)
        {
            return Convert.ToDecimal(Random.NextDouble() * (max - min) + min);
        }

        private static int GetRandomNumberFromArray(int max, int[] arr)
        {
            var rand = Random.Next(0, max);
            while (!arr.Contains(rand))
            {
                rand = Random.Next(0, max);
            }

            return rand;
        }

        private string UploadFile(IFormFile file, string productCode)
        {
            if (file == null)
            {
                return null;
            }

            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/" + productCode);
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = Guid.NewGuid() + "_" + file.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using var fileStream = new FileStream(filePath, FileMode.Create);
            file.CopyTo(fileStream);
            return uniqueFileName;
        }
    }
}