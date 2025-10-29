using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniStore.Domain.Abstractions
{
    public interface IPaymentGateway
    {
        string PaymentMethodName { get; }
        // Devuelve true si el pago fue exitoso
        bool Charge(decimal amount);
    }
}
