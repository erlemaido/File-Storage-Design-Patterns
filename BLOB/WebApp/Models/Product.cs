using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models
{
    [Table("product", Schema = "blob")]
    public class Product
    {
        [Column("product_id", TypeName = "serial")]
        public int ProductId { get; set; }

        [MaxLength(14, ErrorMessage = "Tootekood võib olla maksimaalselt 14 tähemärki pikk!")]
        [Required(ErrorMessage = "Tootekood on kohustuslik!")]
        [DisplayName("Tootekood")]
        [Column("product_code", TypeName = "varchar(14)")]
        public string ProductCode { get; set; }

        [MaxLength(255, ErrorMessage = "Nimi võib olla maksimaalselt 255 tähemärki pikk!")]
        [Required(ErrorMessage = "Nimi on kohustuslik!")]
        [DisplayName("Nimi")]
        [Column("title", TypeName = "varchar(255)")]
        public string Title { get; set; }

        [RegularExpression(@"^\d+.?\d{0,4}$", ErrorMessage = "Mittesobiv hind! Lubatud maksimaalselt neli komakohta.")]
        [Range(0, 999999999999999999.9999,
            ErrorMessage = "Mittesobiv hind! Lubatud hinna vahemik 0-999999999999999999.9999.")]
        [Required(ErrorMessage = "Hind on kohustuslik!")]
        [DisplayName("Hind")]
        [Column("price", TypeName = "decimal(19,4)")]
        public decimal Price { get; set; }

        [ForeignKey("ProductStateType")]
        [Required(ErrorMessage = "Toote seisundi liik on kohustuslik!")]
        [DisplayName("Toote seisundi liik")]
        [Column("product_state_type_code", TypeName = "smallint")]
        [DefaultValue(1)]
        public int ProductStateTypeCode { get; set; } = 1;

        [DisplayName("Toote seisundi liik")] public ProductStateType ProductStateType { get; set; }

        public ICollection<ProductPicture> ProductPictures { get; set; }
    }
}