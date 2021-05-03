using Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.ModelDto
{
    public class UserDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }

        public ApplicationUser<string> GetApplicationUser()
        {
            return new ApplicationUser<string>()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = UserName,
                Email = Email,
                FirstName = FirstName,
                SecondName = LastName,
                RegistrationDate = DateTime.Now
            };
        }
    }
}
