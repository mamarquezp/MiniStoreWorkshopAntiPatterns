//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace MiniStoreWorkshopAntiPatterns
//{
//    public class Product
//    {
//        public string Sku;
//        public string Name;
//        public decimal Price;
//        public decimal WeightKg;
//    }

//    public class Customer
//    {
//        public string Id;
//        public string Name;
//        public string Email;
//        public string Phone;
//    }

//    public class OrderLine
//    {
//        public string Sku;
//        public int Qty;
//        public decimal UnitPrice;
//        public decimal WeightKg; // duplicado
//    }

//    public class Order
//    {
//        public string Id;
//        public string CustomerId;
//        public List<OrderLine> Lines = new();
//        public decimal Subtotal;
//        public decimal Shipping;
//        public decimal Total;
//        public string PaymentType;
//        public string ShippingType;
//        public bool Paid;
//    }

//    // Interfaces/jerarquías dudosas (para provocar ISP/LSP)
//    public interface INotifierFat
//    {
//        void SendEmail(string to, string subject, string body);
//        void SendSms(string phone, string message);
//        void SendPush(string deviceId, string message);
//    }

//    public abstract class ShippingBase
//    {
//        // Contrato poco claro, hijas pueden lanzar por flujos esperables (LSP roto)
//        public virtual decimal CostFor(decimal totalWeightKg, decimal subtotal)
//        {
//            if (totalWeightKg < 0) throw new InvalidOperationException("Peso inválido");
//            return 0;
//        }
//        public virtual void Ship(string address, decimal totalWeightKg)
//        {
//            // algunas hijas no soportarán >2kg y lanzarán excepción
//        }
//    }
//}
