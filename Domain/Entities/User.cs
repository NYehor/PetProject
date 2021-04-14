using System;

namespace Domain.Entities
{
    public class User
    {
        public virtual String Id { get; set; }
        public virtual string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual string Email { get; set; }
        public virtual bool EmailConfirmed { get; set; }
        public virtual String PasswordHash { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string AuthenticationType { get; set; }
        public bool IsAuthenticated { get; set; }
    }
}
