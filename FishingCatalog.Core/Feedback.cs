using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishingCatalog.Core
{
    public class Feedback
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedAt {  get; set; }
        public string? Comment { get; set; }

        public Product Product { get; set; }
        public User User { get; set; }

        public Feedback(Guid id, Guid userId, Guid productId, DateTime createdAt, string? comment)
        {
            this.Id = id;
            this.ProductId = productId;
            this.UserId = userId;
            this.CreatedAt = createdAt;
            this.Comment = comment;
        }
    }
}
