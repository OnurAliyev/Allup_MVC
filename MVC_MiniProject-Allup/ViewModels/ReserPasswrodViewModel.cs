using System.ComponentModel.DataAnnotations;

namespace MVC_MiniProject_Allup.ViewModels;

public class ReserPasswrodViewModel
{
    public string Email { get; set; }
    public string Token { get; set; }
    [DataType(DataType.Password)]
    [Compare("ConfirmNewPassword")]
    public string NewPassword { get; set; }

    [DataType(DataType.Password)]
    public string ConfirmNewPassword { get; set; }
}
