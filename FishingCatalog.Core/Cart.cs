using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishingCatalog.Core
{
    public class Cart
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime AddedAt { get; set; }
        public DateTime ModifiedAt { get; set;}

        public User User { get; set; }
        public Product Product { get; set; }

        public Cart(Guid id, Guid userId, Guid productId, int quantity, DateTime addedAt, DateTime modifiedAt)
        {
            Id = id;
            UserId = userId;
            ProductId = productId;
            Quantity = quantity;
            AddedAt = addedAt;
            ModifiedAt = modifiedAt;
        }
    }
}
