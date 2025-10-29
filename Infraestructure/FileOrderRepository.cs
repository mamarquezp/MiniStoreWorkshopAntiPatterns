using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniStore.Domain.Abstractions;
using MiniStore.Domain.Entities;

namespace MiniStore.Infrastructure
{
    // SRP: Su única responsabilidad es guardar pedidos en CSV.
    public class FileOrderRepository : IOrderRepository
    {
        private static List<Order> _orders = new(); // (en memoria)

        public IEnumerable<Order> GetAll() => _orders;

        public void Save(Order o)
        {
            _orders.Add(o); // (Guarda en memoria)

            // Persiste en disco
            Directory.CreateDirectory("db");
            var line = $"{o.Id};{o.CustomerId};{o.Subtotal};{o.ShippingCost};{o.Total};{o.PaymentType};{o.ShippingType};{o.Paid}";
            File.AppendAllLines("db/orders.csv", new[] { line });
        }
    }
}