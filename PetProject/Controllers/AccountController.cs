using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aplication.Interfaces;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using WebUI.ModelDto;

namespace WebUI.Controllers
{
    //[Authorize]
    public class AccountController : Controller
    {
        private IApplicationDb _applicationDb;
        private readonly UserManager<ApplicationUser<string>> _userManager;
        private readonly SignInManager<ApplicationUser<string>> _signInManager;

        public AccountController(
            UserManager<ApplicationUser<string>> userManager,
            SignInManager<ApplicationUser<string>> signInManager,
            IApplicationDb applicationDb)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _applicationDb = applicationDb;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Registration([FromForm] UserDto user)
        {
            var appUser = user.GetApplicationUser();
            var result = await _userManager.CreateAsync(appUser, user.Password);

            if (!result.Succeeded) BadRequest(result.Errors.ToString());

            await _signInManager.SignInAsync(appUser, isPersistent: false);
            return Ok();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> CheckUsernameIsAvailability([FromBody] string name)
        {
            if (name == null)
                return Json(true);

            var result = await _userManager.FindByNameAsync(name);

            if (result == null)
                return Json(true);
            else
                return Json(false);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> CheckEmailIsAvailability([FromBody] string email)
        {
            if (email == null) 
                return Json(true);

            var result = await _userManager.FindByEmailAsync(email);

            if (result == null)
                return Json(true);
            else
                return Json(false);
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> Login([FromBody] CredentialDto data)
        {
            var user = await _userManager.FindByEmailAsync(data.Email);

            if (user == null) return Json(false);

            var result = await _signInManager.PasswordSignInAsync(user, data.Password, true, false);

            if (result.Succeeded)
                return Json(true);
            else
                return Json(false);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction();
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
                return BadRequest();

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return BadRequest("User not found.");

            var result = await _userManager.ConfirmEmailAsync(user, code);

            if(result.Succeeded)
                return Ok("ConfirmEmail");
            else
                return BadRequest("Error");
        }
    }
}
