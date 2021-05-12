using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Models
{
    [Table("product_picture", Schema = "bytea")]
    [Index(nameof(ProductId), nameof(SeqNr), IsUnique = true)]
    public class ProductPicture
    {
        [Column("product_picture_id", TypeName = "bigserial")]
        public long ProductPictureId { get; set; }

        [Required(ErrorMessage = "Järjekorranumber on kohustuslik!")]
        [DisplayName("Jrk nr")]
        [Range(1, 100, ErrorMessage = "Mittesobiv järjekorranumber! Lubatud vahemik 1-100.")]
        [Column("seq_nr", TypeName = "smallint")]
        public int SeqNr { get; set; }

        [DisplayName("Pilt")]
        [Column("picture", TypeName = "bytea")]
        public byte[] Picture { get; set; }

        [Required(ErrorMessage = "Toode on kohustuslik!")]
        [DisplayName("Toode")]
        [Column("product_id", TypeName = "integer")]
        public int ProductId { get; set; }

        [DisplayName("Toode")] public Product Product { get; set; }
    }
}