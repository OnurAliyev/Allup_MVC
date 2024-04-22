using System.ComponentModel.DataAnnotations;

namespace MVC_MiniProject_Allup.ViewModels;

public class ForgotPasswordViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}
