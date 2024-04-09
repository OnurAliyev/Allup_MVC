using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MVC_MiniProject_Allup.Models;

public class Slider:BaseEntity
{
    [Required]
    [StringLength(20)]
    public string FirstTitle { get; set; }
    [Required]
    [StringLength(20)]
    public string SecondTitle { get; set; }
    [Required]
    [StringLength(150)]
    public string Description { get; set; }
    public string? RedirectUrl { get; set; }
    [Required]
    [StringLength(40)]
    public string RedirectUrlText { get; set; }
    [StringLength(100)]
    public string? ImageUrl { get; set; }
    [NotMapped]
    public IFormFile? ImageFile { get; set; }
}
