using FishingCatalog.Core;

namespace FishingCatalog.msCart.Repositories
{
    public interface ICartRepository
    {
        Task<List<Cart>> GetAll();
        Task<Cart?> GetById(Guid id);
        Task<List<Cart>> GetByUserId(Guid userId);
        Task<Guid> Add(Cart cart);
        Task<Guid> Update(Cart cart, Guid id);
        Task<Guid> Delete(Guid id);
        Task<Guid> DeleteAllByUserId(Guid userId);
    }
}
