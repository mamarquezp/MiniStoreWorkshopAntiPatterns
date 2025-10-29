using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniStore.Domain.Entities;

namespace MiniStore.Domain.Abstractions
{
    public interface IProductRepository
    {
        Product? GetBySku(string sku);
        IEnumerable<Product> GetAll();
        void Seed();
    }

    public interface ICustomerRepository
    {
        Customer? GetById(string id);
        IEnumerable<Customer> GetAll();
        void Save(Customer customer);
    }

    public interface IOrderRepository
    {
        IEnumerable<Order> GetAll();
        void Save(Order order);
    }
}
