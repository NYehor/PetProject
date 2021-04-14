using System;
using System.Security.Principal;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class ApplicationUser<TKey> : IdentityUser<TKey>
        where TKey : IEquatable<TKey>
    {
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
