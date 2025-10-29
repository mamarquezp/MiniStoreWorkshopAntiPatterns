using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniStore.Domain.Entities
{
    public class Order
    {
        public string Id { get; private set; }
        public string CustomerId { get; private set; }
        private readonly List<OrderLine> _lines = new();
        public IReadOnlyCollection<OrderLine> Lines => _lines.AsReadOnly();

        public decimal Subtotal { get; private set; }
        public decimal ShippingCost { get; private set; }
        public decimal Total { get; private set; }
        public bool Paid { get; private set; }

        public string PaymentType { get; private set; }
        public string ShippingType { get; private set; }

        public Order(string id, string customerId, string paymentType, string shippingType)
        {
            Id = id;
            CustomerId = customerId;
            PaymentType = paymentType;
            ShippingType = shippingType;
            Paid = false;
        }

        public void AddLine(Product product, int qty)
        {
            var line = new OrderLine(product.Sku, qty, product.Price, product.WeightKg);
            _lines.Add(line);
            RecalculateTotals();
        }

        public decimal GetTotalWeight()
        {
            return _lines.Sum(l => l.WeightKg * l.Qty);
        }

        public void ApplyDiscount(decimal discountAmount)
        {
            Subtotal -= discountAmount;
            RecalculateTotals();
        }

        public void SetShippingCost(decimal cost)
        {
            ShippingCost = cost;
            RecalculateTotals();
        }

        public void MarkAsPaid()
        {
            Paid = true;
        }

        private void RecalculateTotals()
        {
            Subtotal = _lines.Sum(l => l.UnitPrice * l.Qty);
            Total = Subtotal + ShippingCost;
        }
    }
}
