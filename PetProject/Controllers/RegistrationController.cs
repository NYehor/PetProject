using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegistrationController : ApiController
    {
        //private IUnitOfWork _unitOfWork;

        //public RegistrationController(IUnitOfWork unitOfWork)
        //{
        //    _unitOfWork = unitOfWork;
        //}

        //[HttpPost]
        //public IActionResult Post()
        //{
        //    User user = new User();
        //    user.Email = Request.Form.FirstOrDefault(p => p.Key == "email").Value;
        //    user.UserName = Request.Form.FirstOrDefault(p => p.Key == "username").Value;
        //    user.Password = Request.Form.FirstOrDefault(p => p.Key == "password").Value;
        //    user.FirstName = Request.Form.FirstOrDefault(p => p.Key == "firstname").Value;
        //    user.LastName = Request.Form.FirstOrDefault(p=> p.Key == "lastname").Value;
        //    user.RegistrationDate = DateTime.Now;

        //    _unitOfWork.Users.Create(user);

        //    return Ok();
        //}
    }
}
