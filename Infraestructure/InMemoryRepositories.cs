using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniStore.Domain.Abstractions;
using MiniStore.Domain.Entities;

namespace MiniStore.Infrastructure
{
    // Mantiene los datos estáticos como en el original, pero detrás de una interfaz
    public class InMemoryProductRepository : IProductRepository
    {
        private static List<Product> _products = new();

        public Product? GetBySku(string sku) => _products.FirstOrDefault(x => x.Sku == sku);
        public IEnumerable<Product> GetAll() => _products;

        public void Seed()
        {
            _products = new List<Product> {
                new Product("P1", "Mouse", 100m, 0.2m),
                new Product("P2", "Teclado", 300m, 0.8m),
                new Product("P3", "Laptop", 6000m, 2.5m),
            };
        }
    }

    public class InMemoryCustomerRepository : ICustomerRepository
    {
        private static List<Customer> _customers = new();

        public Customer? GetById(string id) => _customers.FirstOrDefault(x => x.Id == id);
        public IEnumerable<Customer> GetAll() => _customers;

        public void Save(Customer customer)
        {
            _customers.Add(customer);
        }
    }
}
