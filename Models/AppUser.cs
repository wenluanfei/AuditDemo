using Microsoft.AspNetCore.Identity;

namespace AuditDemo.Models
{
    /// <summary>
    /// Extends ASP.NET Core Identity's built-in IdentityUser.
    /// IdentityUser already provides: Id, Email, UserName, PasswordHash etc.
    /// We add FullName as a custom field for display purposes.
    /// </summary>
    public class AppUser : IdentityUser
    {
        /// <summary>Auditor's full name for display in the UI</summary>
        public string FullName { get; set; } = "";
    }
}
