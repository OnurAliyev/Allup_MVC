using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MVC_MiniProject_Allup.Models;

public class Category:BaseEntity
{
    [Required]
    [StringLength(50)]
    public string Name { get; set; }
    [StringLength(200)]
    public string? ImageUrl { get; set; }
    [NotMapped]
    public IFormFile? ImageFile { get; set; }
    public List<Product>? Products { get; set; }
}
