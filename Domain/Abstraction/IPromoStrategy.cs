using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniStore.Domain.Entities;

namespace MiniStore.Domain.Abstractions
{
    public interface IPromoStrategy
    {
        string PromoCode { get; }
        // Devuelve el monto del descuento
        decimal CalculateDiscount(Order order);
    }
}
