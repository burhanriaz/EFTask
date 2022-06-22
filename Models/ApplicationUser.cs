using Microsoft.AspNetCore.Identity;

namespace EFTask.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Country { get; set; }
    }
}
