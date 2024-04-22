using Microsoft.AspNetCore.Identity;

namespace MVC_MiniProject_Allup.Models
{
    public class AppUser:IdentityUser
    {
        public string FullName { get; set; }
    }
}
