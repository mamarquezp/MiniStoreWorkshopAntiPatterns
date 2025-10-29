using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniStore.Application.DTOs;
using MiniStore.Application.UseCases;
using MiniStore.Domain.Abstractions;

namespace MiniStore.Presentation
{
    // SRP: Se encarga solo de la lógica del menú de consola
    public class MenuHandler
    {
        // Depende de Casos de Uso (Application)
        private readonly OrderService _orderService;
        private readonly IProductRepository _productRepo;
        private readonly ICustomerRepository _customerRepo;
        private readonly IOrderRepository _orderRepo;

        public MenuHandler(OrderService orderService, IProductRepository productRepo, ICustomerRepository customerRepo, IOrderRepository orderRepo)
        {
            _orderService = orderService;
            _productRepo = productRepo;
            _customerRepo = customerRepo;
            _orderRepo = orderRepo;
        }

        public void Run()
        {
            while (true)
            {
                Console.WriteLine("\n=== MENÚ ===");
                Console.WriteLine("1) Listar productos");
                Console.WriteLine("2) Registrar cliente");
                Console.WriteLine("3) Crear pedido y pagar");
                Console.WriteLine("4) Listar pedidos");
                Console.WriteLine("0) Salir");
                Console.Write("> ");
                var op = Console.ReadLine();
                if (op == "0") break;

                switch (op)
                {
                    case "1": ListProducts(); break;
                    case "2": RegisterCustomer(); break;
                    case "3": CreateOrder(); break;
                    case "4": ListOrders(); break;
                    default: Console.WriteLine("Opción inválida"); break;
                }
            }
        }

        private void ListProducts()
        {
            var products = _productRepo.GetAll();
            foreach (var p in products) Console.WriteLine($"{p.Sku} - {p.Name} - {p.Price:C} - {p.WeightKg}kg");
        }

        private void RegisterCustomer()
        {
            Console.Write("Nombre: ");
            var name = Console.ReadLine() ?? "";
            Console.Write("Email: ");
            var email = Console.ReadLine() ?? "";
            Console.Write("Teléfono: ");
            var phone = Console.ReadLine() ?? "";

            var c = new Domain.Entities.Customer(Guid.NewGuid().ToString("N"), name, email, phone);
            _customerRepo.Save(c);
            Console.WriteLine($"Cliente registrado: {c.Name} ({c.Id})");
        }

        private void CreateOrder()
        {
            Console.Write("Id Cliente: ");
            var cid = Console.ReadLine() ?? "";
            Console.Write("Sku Producto: ");
            var sku = Console.ReadLine() ?? "";
            Console.Write("Cantidad: ");
            var qtyStr = Console.ReadLine() ?? "1";
            int.TryParse(qtyStr, out var qty);
            Console.Write("Promo (standard/bf/vip/employee): ");
            var promo = Console.ReadLine();
            Console.Write("Pago (card/cash/crypto): ");
            var payment = Console.ReadLine();
            Console.Write("Envio (standard/express/drone): ");
            var ship = Console.ReadLine();

            var request = new CreateOrderRequest
            {
                CustomerId = cid,
                Sku = sku,
                Qty = qty,
                PromoCode = promo,
                PaymentMethod = payment,
                ShippingMethod = ship
            };

            var (order, error) = _orderService.CreateOrder(request);

            if (order != null)
            {
                Console.WriteLine($"Pedido {order.Id} creado y pagado. Total: {order.Total:C}");
            }
            else
            {
                Console.WriteLine($"[ERROR] {error}");
            }
        }

        private void ListOrders()
        {
            var orders = _orderRepo.GetAll();
            foreach (var o in orders)
                Console.WriteLine($"{o.Id} - C:{o.CustomerId} Sub:{o.Subtotal:C} Ship:{o.ShippingCost:C} Total:{o.Total:C} Paid:{o.Paid}");
        }
    }
}
