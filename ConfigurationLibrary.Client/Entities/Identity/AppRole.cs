using Microsoft.AspNetCore.Identity;

namespace ConfigurationLibrary.UI.Entities.Identity
{
    public class AppRole : IdentityRole<int>
    {
        public string? DisplayName { get; set; }
    }
}