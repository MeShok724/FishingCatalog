using FishingCatalog.Core;
using FishingCatalog.Postgres;
using Microsoft.EntityFrameworkCore;

namespace FishingCatalog.msFeedback.Repositories
{
    public class FeedbackRepository(FishingCatalogDbContext dbContext) : IFeedbackRepository
    {
        private readonly FishingCatalogDbContext _context = dbContext;

        public async Task<List<Feedback>> GetAll()
        {
            return await _context.Feedbacks
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<Feedback?> GetById(Guid id)
        {
            var dbResponse = await _context.Feedbacks
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);
            return dbResponse;
        }
        public async Task<List<Feedback>> GetByUserId(Guid userId)
        {
            var dbResponse = await _context.Feedbacks
                .AsNoTracking()
                .Where(c => c.UserId == userId)
                .ToListAsync();
            if (dbResponse == null)
            {
                return [];
            }
            return dbResponse;
        }
        public async Task<List<Feedback>> GetByProductId(Guid productId)
        {
            var dbResponse = await _context.Feedbacks
                .AsNoTracking()
                .Where(c => c.ProductId == productId)
                .ToListAsync();
            if (dbResponse == null)
            {
                return [];
            }
            return dbResponse;
        }
        public async Task<Guid> Add(Feedback feedback)
        {
            User? user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == feedback.UserId);
            Product? product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == feedback.ProductId);
            if (product == null || user == null)
            {
                return Guid.Empty;
            }

            await _context.Feedbacks.AddAsync(feedback);
            _context.SaveChanges();
            return feedback.Id;
        }
        public async Task<Guid> Update(Feedback feedback, Guid id)
        {
            await _context.Feedbacks
                .Where(c => c.Id == id)
                .ExecuteUpdateAsync(c => c
                .SetProperty(c => c.Comment, feedback.Comment)
                );
            return feedback.Id;
        }
        public async Task<Guid> Delete(Guid Id)
        {
            await _context.Feedbacks
                .Where(c => c.Id == Id)
                .ExecuteDeleteAsync();
            return Id;
        }
        public async Task<Guid> DeleteAllByUserId(Guid userId)
        {
            await _context.Feedbacks
                .Where(c => c.UserId == userId)
                .ExecuteDeleteAsync();
            return userId;
        }
        public async Task<Guid> DeleteAllByProductId(Guid productId)
        {
            await _context.Feedbacks
                .Where(c => c.ProductId == productId)
                .ExecuteDeleteAsync();
            return productId;
        }
    }
}
