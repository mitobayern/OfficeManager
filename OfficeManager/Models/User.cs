namespace OfficeManager.Models
{
    using Microsoft.AspNetCore.Identity;

    public class User : IdentityUser<string>
    {
        public bool IsEnabled { get; set; }
    }
}
