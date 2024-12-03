using FishingCatalog.Core;

namespace FishingCatalog.msUser.Repositories
{
    public interface IUserRepository
    {
        Task<List<User>> GetAll();
        Task<List<User>> GetByRole(string name);
        Task<User?> GetByEmail(string email);
        Task<List<User>> GetActive();
        Task<List<User>> SortByName(bool ask);
        Task<List<User>> SortByRegistrationTime(bool ask);
        Task<List<User>> SortByLastLoginTime(bool ask);
        Task<User?> GetById(Guid id);
        Task<(Guid, string)> Add(User user);
        Task<Guid> Update(User user, Guid Id);
        Task<Guid> ChangeIsActive(Guid id, bool isActive);
        Task<Guid> ChangeLastLogin(Guid id, DateTime lastLogin);
        Task<Guid> Delete(Guid Id);
        Task<Guid> GetDafaultRoleId();
    }
}
