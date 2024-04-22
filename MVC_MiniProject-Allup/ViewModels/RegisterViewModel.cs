using System.ComponentModel.DataAnnotations;

namespace MVC_MiniProject_Allup.ViewModels;

public class RegisterViewModel
{
    [DataType(DataType.Text)]
    [StringLength(25)]
    public string FullName { get; set; }
    [DataType(DataType.Text)]
    [StringLength(25)]
    public string UserName { get; set; }
    [DataType(DataType.Password)]
    [Compare("ConfirmPassword")]
    public string Password { get; set; }
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; }
    [DataType(DataType.EmailAddress)]
    [StringLength(80)]
    public string Email { get; set; }
    [DataType(DataType.PhoneNumber)]
    [StringLength(20)]
    public string PhoneNumber { get; set; }
}
