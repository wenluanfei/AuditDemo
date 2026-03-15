using AuditDemo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AuditDemo.Pages
{
    [IgnoreAntiforgeryToken]
    public class LoginActionModel : PageModel
    {
        private readonly SignInManager<AppUser> _signInManager;

        public LoginActionModel(SignInManager<AppUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<IActionResult> OnPostAsync(string email, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(email, password, false, false);
            if (result.Succeeded)
                return Redirect("/");
            return Redirect("/login?error=1");
        }
    }
}
