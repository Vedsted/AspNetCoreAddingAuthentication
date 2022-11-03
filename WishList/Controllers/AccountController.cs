using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WishList.Models;
using WishList.Models.AccountViewModels;

namespace WishList.Controllers
{
    [Authorize]
    public class AccountController  : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View("Register");
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Register", model);

            var user = new ApplicationUser() { UserName = model.Email, Email = model.Email};
            var identityResultTask = _userManager.CreateAsync(user, model.Password);
            
            identityResultTask.Wait();
            
            if (identityResultTask.Result.Succeeded)
                return RedirectToAction("Index", "Home");

            foreach (var error in identityResultTask.Result.Errors)
            {
                ModelState.AddModelError("Password", error.Description);
            }
                
            return View("Register", model);
        }
    }
}
