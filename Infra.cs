using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniStoreWorkshopAntiPatterns
{
    public class ConsoleNotifier : INotifierFat
    {
        public void SendEmail(string to, string subject, string body)
            => Console.WriteLine($"[EMAIL] To:{to} Subj:{subject} Body:{body}");
        public void SendSms(string phone, string message)
            => Console.WriteLine($"[SMS] To:{phone} Msg:{message}");
        public void SendPush(string deviceId, string message)
            => throw new NotSupportedException("Push no disponible, pero la interfaz lo exige (ISP roto).");
    }

    public class FileRepo
    {
        public void SaveOrder(Order o)
        {
            Directory.CreateDirectory("db");
            var line = $"{o.Id};{o.CustomerId};{o.Subtotal};{o.Shipping};{o.Total};{o.PaymentType};{o.ShippingType};{o.Paid}";
            File.AppendAllLines("db/orders.csv", new[] { line });
        }
    }

    // Pagos: diferentes comportamientos, algunos lanzan por flujo normal (LSP)

    public class CardPayment
    {
        public bool Charge(decimal amount)
        {
            Console.WriteLine($"[CARD] Pagado {amount:C}");
            return true;
        }
    }

    public class CashPayment
    {
        public bool Charge(decimal amount)
        {
            Console.WriteLine($"[CASH] Registrado {amount:C}");
            return true;
        }
    }

    public class CryptoPayment
    {
        public bool Charge(decimal amount)
        {
            // Precondición oculta: solo montos enteros (mal para LSP)
            if (amount != Math.Truncate(amount))
                throw new InvalidOperationException("Crypto solo admite montos enteros.");
            Console.WriteLine($"[CRYPTO] Hash ok por {amount:C}");
            return true;
        }
    }

    // Envíos con comportamientos inconsistentes (LSP)
    public class StandardShipping : ShippingBase
    {
        public override decimal CostFor(decimal totalWeightKg, decimal subtotal)
        {
            return totalWeightKg <= 5 ? 25 : 50;
        }
        public override void Ship(string address, decimal totalWeightKg)
        {
            Console.WriteLine($"[SHIP] Standard a {address} ({totalWeightKg}kg)");
        }
    }

    public class ExpressShipping : ShippingBase
    {
        public override decimal CostFor(decimal totalWeightKg, decimal subtotal)
        {
            return 60; // tarifa plana poco realista
        }
        public override void Ship(string address, decimal totalWeightKg)
        {
            Console.WriteLine($"[SHIP] Express a {address} ({totalWeightKg}kg)");
        }
    }

    public class DroneShipping : ShippingBase
    {
        public override decimal CostFor(decimal totalWeightKg, decimal subtotal)
        {
            return 15; // barato pero con letra pequeña no declarada
        }
        public override void Ship(string address, decimal totalWeightKg)
        {
            if (totalWeightKg > 2) throw new NotSupportedException("Drones no soportan > 2kg (sorpresa).");
            Console.WriteLine($"[SHIP] Drone a {address} ({totalWeightKg}kg)");
        }
    }

}
