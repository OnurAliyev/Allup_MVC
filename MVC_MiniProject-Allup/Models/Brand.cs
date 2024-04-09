using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MVC_MiniProject_Allup.Models
{
    public class Brand:BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(200)]
        public string? BrandLogoUrl { get; set; }
        [NotMapped]
        public IFormFile? LogoImageFile { get; set; }
        public List<Product>? Products { get; set; }
    }
}
