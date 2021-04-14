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
        public async Task<IActionResult> Registration()
        {
            var user = new ApplicationUser<string>
            {
                Id = Guid.NewGuid().ToString(),
                UserName = Request.Form.FirstOrDefault(p => p.Key == "username").Value,
                Email = Request.Form.FirstOrDefault(p => p.Key == "email").Value,
                FirstName = Request.Form.FirstOrDefault(p => p.Key == "firstname").Value,
                SecondName = Request.Form.FirstOrDefault(p => p.Key == "lastname").Value,
                RegistrationDate = DateTime.Now
            };

            var result = await _userManager.CreateAsync(user, Request.Form.FirstOrDefault(p => p.Key == "password").Value);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return Ok();
            }
            else
                return BadRequest(result.Errors.ToString());
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> CheckUsernameIsAvailability([FromBody] string name)
        {
            var cont = ControllerContext.HttpContext;
            var req = Request.Body.Length;

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
            var cont = ControllerContext.HttpContext;
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
        public async Task<IActionResult> Login()
        {
            var email = Request.Form.FirstOrDefault(p => p.Key == "email").Value;
            var password = Request.Form.FirstOrDefault(p => p.Key == "password").Value;
            var result = await _signInManager.PasswordSignInAsync(email, password, true, false);

            if (result.Succeeded)
            {             
                return Ok();
            }

            return Ok("WTF");
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
