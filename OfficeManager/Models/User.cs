namespace OfficeManager.Models
{
    using Microsoft.AspNetCore.Identity;

    public class User : IdentityUser
    {
        public bool? IsEnabled { get; set; }
    }
}