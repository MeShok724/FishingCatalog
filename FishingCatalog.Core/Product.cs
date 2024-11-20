using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishingCatalog.Core
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public byte[] Image { get; set; }

        public Product(Guid id, string name, decimal price, string category, string description, byte[] image)
        {
            Id = id;
            Name = name;
            Price = price;
            Category = category;
            Description = description;
            Image = image;
        }       
    }
}
