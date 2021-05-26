using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using WebApp.Models;

namespace WebApp.ViewModels
{
    public class ProductViewModel
    {
        public int ProductId { get; set; }

        [MaxLength(14, ErrorMessage = "Tootekood võib olla maksimaalselt 14 tähemärki pikk!")]
        [Required(ErrorMessage = "Tootekood on kohustuslik!")]
        [DisplayName("Tootekood")]
        public string ProductCode { get; set; }

        [MaxLength(255, ErrorMessage = "Nimi võib olla maksimaalselt 255 tähemärki pikk!")]
        [Required(ErrorMessage = "Nimi on kohustuslik!")]
        [DisplayName("Nimi")]
        public string Title { get; set; }

        [RegularExpression(@"^\d+.?\d{0,4}$", ErrorMessage = "Mittesobiv hind! Lubatud maksimaalselt neli komakohta.")]
        [Range(0, 999999999999999999.9999,
            ErrorMessage = "Mittesobiv hind! Lubatud hinna vahemik 0-999999999999999999.9999.")]
        [Required(ErrorMessage = "Hind on kohustuslik!")]
        [DisplayName("Hind")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Toote seisundi liik on kohustuslik!")]
        [DisplayName("Toote seisundi liik")]
        [DefaultValue(1)]
        public int ProductStateTypeCode { get; set; } = 1;

        [DisplayName("Toote seisundi liik")] public ProductStateType ProductStateType { get; set; }

        public ICollection<ProductPicture> ProductPictures { get; set; }

        [DisplayName("Pilt")] public List<IFormFile> PictureFiles { get; set; }

        [DisplayName("Testtoodete arv")]
        [Required(ErrorMessage = "Testtoodete arv on kohustuslik!")]
        [Range(0, 200000, ErrorMessage = "Lubatud vahemik 1-200000")]
        public int NumberOfItems { get; set; }

        public List<byte[]> PictureBytes { get; set; }
    }
}