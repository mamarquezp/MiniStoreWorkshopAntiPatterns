using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniStore.Domain.Abstractions;

namespace MiniStore.Infrastructure.Payments
{
    public class CardPaymentGateway : IPaymentGateway
    {
        public string PaymentMethodName => "card";
        public bool Charge(decimal amount)
        {
            Console.WriteLine($"[CARD] Pagado {amount:C}");
            return true;
        }
    }

    public class CashPaymentGateway : IPaymentGateway
    {
        public string PaymentMethodName => "cash";
        public bool Charge(decimal amount)
        {
            Console.WriteLine($"[CASH] Registrado {amount:C}");
            return true;
        }
    }

    public class CryptoPaymentGateway : IPaymentGateway
    {
        public string PaymentMethodName => "crypto";

        // LSP: Esta clase cumple el contrato IPaymentGateway, se valida antes si es entero
        public bool Charge(decimal amount)
        {
            if (amount != Math.Truncate(amount))
            {
                Console.WriteLine("[CRYPTO] Pago rechazado: Solo montos enteros.");
                return false; 
            }
            Console.WriteLine($"[CRYPTO] Hash ok por {amount:C}");
            return true;
        }
    }
}
