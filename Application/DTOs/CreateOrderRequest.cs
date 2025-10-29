using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniStore.Application.DTOs
{
    // DTO (Data Transfer Object) para el caso de uso
    public class CreateOrderRequest
    {
        public string? CustomerId { get; set; }
        public string? Sku { get; set; }
        public int Qty { get; set; }
        public string? PromoCode { get; set; }
        public string? PaymentMethod { get; set; }
        public string? ShippingMethod { get; set; }
    }
}
