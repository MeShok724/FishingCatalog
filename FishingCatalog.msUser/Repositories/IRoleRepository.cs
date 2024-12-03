using FishingCatalog.Core;

namespace FishingCatalog.msUser.Repositories
{
    public interface IRoleRepository
    {
        Task<List<Role>> GetAll();
        Task<Guid> GetByName(string name);
        Task<Guid> Add(Role role);
        Task<Guid> Delete(Guid Id);
    }
}
