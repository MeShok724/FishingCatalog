using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishingCatalog.Core
{
    public class User
    {
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
    }
}
