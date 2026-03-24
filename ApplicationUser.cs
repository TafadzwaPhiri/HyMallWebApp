using Microsoft.AspNetCore.Identity;

namespace HyMall_App.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; } = string.Empty;

        // fields for Admin Settings
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? ZipCode { get; set; }
        public string Bio { get; set; } = "Normal text";
    }
}