using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishingCatalog.Core
{
    public class Product
    {
        private const int NAME_MAX_LENGTH = 100;
        private const int CATEGORY_MAX_LENGTH = 100;
        private const int DESCRIPTION_MAX_LENGTH = 250;
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public byte[] Image { get; set; }

        private Product(Guid id, string name, decimal price, string category, string description, byte[] image)
        {
            Id = id;
            Name = name;
            Price = price;
            Category = category;
            Description = description;
            Image = image;
        }       
        public static (Product, string) Create(Guid id, string name, decimal price, string category, string description, byte[] image)
        {
            string error = string.Empty;
            if (string.IsNullOrEmpty(name) || name.Length > NAME_MAX_LENGTH)
            {
                error = "Name can not be empty or longer then 100 symbols";
            } else if (string.IsNullOrEmpty(category) || category.Length > CATEGORY_MAX_LENGTH)
            {
                error = "Category can not be empty or longer then 100 symbols";
            } else if (string.IsNullOrEmpty(description) || description.Length > DESCRIPTION_MAX_LENGTH)
            {
                error = "Description can not be empty or longer then 250 symbols";
            }

            Product product = new Product(id, name, price, category, description, image);
            return (product, error);
        }
    }
}
