using Microsoft.AspNetCore.Identity;

namespace AuditDemo.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; } = "";
    }
}
