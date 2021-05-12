using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Models
{
    [Table("product_state_type", Schema = "filesystem")]
    [Index(nameof(Title), IsUnique = true)]
    public class ProductStateType
    {
        [Key]
        [Column("product_state_type_code", TypeName = "smallint")]
        public int ProductStateTypeCode { get; set; }

        [Column("title", TypeName = "varchar(50)")]
        [Required(ErrorMessage = "Nimetus on kohustuslik!")]
        [DisplayName("Nimetus")]
        [MaxLength(50, ErrorMessage = "Nimetus võib olla maksimaalselt 50 tähemärki pikk!")]

        public string Title { get; set; }
    }
}