using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrinkAndGo.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DrinkAndGo.Controllers
{
    public class AccountController : Controller
    {
        // Usermanager class has all the api to manage users in database ie. create user, dlt user
        private readonly UserManager<IdentityUser> _userManager;
        // SignManager class has all the api to manage signin logout of user
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signinManager)
        {
            _userManager = userManager;
            _signInManager = signinManager;
        }

        //  Login method redirect the user to login page/registration
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel()
            {
                ReturnUrl = returnUrl
            });
        }


        // login method to handle the signup | login
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
                return View(loginViewModel);

            var user = await _userManager.FindByNameAsync(loginViewModel.UserName);

            if(user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
                if(result.Succeeded)
                {
                    if (string.IsNullOrEmpty(loginViewModel.ReturnUrl))
                        return RedirectToAction("Index", "Home");
                    return Redirect(loginViewModel.ReturnUrl);
                }
            }
            ModelState.AddModelError("", "Username/password not found");
            return View(loginViewModel);
        }
    }
}
