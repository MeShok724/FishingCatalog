
namespace FishingCatalog.Core
{
    public class Role
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<User> Users { get; set; }

        private Role(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
        public static (Role, string) Create(Guid Id, string Name)
        {
            string error = string.Empty;
            if (string.IsNullOrEmpty(Name) || Name.Length > 30)
                error = "Name can not be empty or longer then 30 symbols";
            Role role = new Role (Id, Name);
            return (role, error);
        }
    }
}
