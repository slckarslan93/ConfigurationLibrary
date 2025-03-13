using Microsoft.AspNetCore.Identity;

namespace ConfigurationLibrary.UI.Entities.Identity
{
    public class AppUser : IdentityUser<int>
    {
        public string FullName { get; set; }
        public string? UserImage { get; set; }
        public DateTimeOffset RegisterDate { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
