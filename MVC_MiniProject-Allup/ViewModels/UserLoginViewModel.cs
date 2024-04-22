using System.ComponentModel.DataAnnotations;

namespace MVC_MiniProject_Allup.ViewModels;

public class UserLoginViewModel
{
    [DataType(DataType.Text)]
    [StringLength(25)]
    public string UserName { get; set; }
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
