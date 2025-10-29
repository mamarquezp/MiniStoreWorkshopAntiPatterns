using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniStore.Domain.Entities
{
    public class Product
    {
        public string Sku { get; private set; }
        public string Name { get; private set; }
        public decimal Price { get; private set; }
        public decimal WeightKg { get; private set; }

        public Product(string sku, string name, decimal price, decimal weightKg)
        {
            Sku = sku;
            Name = name;
            Price = price;
            WeightKg = weightKg;
        }
    }
}
