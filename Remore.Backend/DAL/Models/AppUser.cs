using Microsoft.AspNetCore.Identity;

namespace Remore.Backend.DAL.Models
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }
    }
}
