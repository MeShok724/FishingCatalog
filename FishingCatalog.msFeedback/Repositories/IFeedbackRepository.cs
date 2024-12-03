using FishingCatalog.Core;

namespace FishingCatalog.msFeedback.Repositories
{
    public interface IFeedbackRepository
    {
        Task<Guid> Add(Feedback feedback);
        Task<Guid> Delete(Guid Id);
        Task<Guid> DeleteAllByProductId(Guid productId);
        Task<Guid> DeleteAllByUserId(Guid userId);
        Task<List<Feedback>> GetAll();
        Task<Feedback?> GetById(Guid id);
        Task<List<Feedback>> GetByProductId(Guid productId);
        Task<List<Feedback>> GetByUserId(Guid userId);
        Task<Guid> Update(Feedback feedback, Guid id);
    }
}