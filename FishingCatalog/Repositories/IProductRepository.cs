using FishingCatalog.Core;

namespace FishingCatalog.msCatalog.Repositories
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAll();
        Task<List<Product>> GetByCategory(string category);
        Task<List<Product>> SortByName(bool ask);
        Task<List<Product>> SortByPrice(bool ask);
        Task<Product?> GetById(Guid id);
        Task<Guid> Add(Product product);
        Task<Guid> Update(Product product, Guid id);
        Task<Guid> Delete(Guid id);
    }
}
