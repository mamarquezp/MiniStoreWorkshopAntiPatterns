using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniStore.Domain.Entities
{
    public class OrderLine
    {
        public string Sku { get; private set; }
        public int Qty { get; private set; }
        public decimal UnitPrice { get; private set; }
        public decimal WeightKg { get; private set; }

        public OrderLine(string sku, int qty, decimal unitPrice, decimal weightKg)
        {
            Sku = sku;
            Qty = qty;
            UnitPrice = unitPrice;
            WeightKg = weightKg;
        }
    }
}
