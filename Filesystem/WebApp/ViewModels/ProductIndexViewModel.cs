using System.Collections.Generic;

namespace WebApp.ViewModels
{
    public class ProductIndexViewModel
    {
        public IEnumerable<ProductViewModel> Products { get; set; }

        public int Page;
    }
}