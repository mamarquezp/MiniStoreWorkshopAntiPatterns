using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniStore.Domain.Abstractions;
using MiniStore.Domain.Entities;

namespace MiniStore.Infraestructure.Promotions
{
    public class StandardPromoStrategy : IPromoStrategy
    {
        public string PromoCode => "standard";
        public decimal CalculateDiscount(Order order) => 0;
    }

    public class PercentagePromoStrategy : IPromoStrategy
    {
        public string PromoCode { get; }
        private readonly decimal _percentage;

        public PercentagePromoStrategy(string promoCode, decimal percentage)
        {
            PromoCode = promoCode;
            _percentage = percentage;
        }

        public decimal CalculateDiscount(Order order)
        {
            return order.Subtotal * (1 - _percentage);
        }
    }
}
