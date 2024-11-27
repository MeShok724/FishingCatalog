using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishingCatalog.Core
{
    public class User
    {
        private const int NAME_MAX_LENGTH = 100;
        private const int EMAIL_MAX_LENGTH = 50;
        private const int PASSWORD_MAX_LENGTH = 250;
        public Guid Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public string PasswordHash { get; set; } = String.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.MinValue;
        public DateTime LastLogin { get; set;} = DateTime.MinValue;
        public bool IsActive { get; set; } = false;
        public Guid RoleId { get; set; }

        public List<Cart> Carts { get; set; } = [];
        public Role Role { get; set; }

        private User(Guid Id, string Name, string Email, string PasswordHash, 
            DateTime CreatedAt, DateTime LastLogin, bool IsActive, Guid RoleId)
        {
            this.Id = Id;
            this.Name = Name;
            this.Email = Email;
            this.PasswordHash = PasswordHash;
            this.CreatedAt = CreatedAt;
            this.LastLogin = LastLogin;
            this.IsActive = IsActive;
            this.RoleId = RoleId;
        }
        public static (User, string) Create(Guid Id, string Name, string Email, string PasswordHash,
            DateTime CreatedAt, DateTime LastLogin, bool IsActive, Guid RoleId)
        {
            string error = string.Empty;
            if (string.IsNullOrEmpty(Name) || Name.Length > NAME_MAX_LENGTH) {
                error = "Name can not be empty or longer then 100 symbols";
            } else if (string.IsNullOrEmpty(Email) || Email.Length > EMAIL_MAX_LENGTH)
            {
                error = "Email can not be empty or longer then 50 symbols";
            } else if (string.IsNullOrEmpty(PasswordHash) || PasswordHash.Length > PASSWORD_MAX_LENGTH)
            {
                error = "Password hash can not be empty or longer then 250 symbols";
            }

            User user = new User(Id, Name, Email, PasswordHash, CreatedAt, LastLogin, IsActive, RoleId);
            return (user, error);
        }
    }
}
