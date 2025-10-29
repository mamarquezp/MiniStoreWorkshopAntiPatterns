//namespace MiniStoreWorkshopAntiPatterns
//{

//    public class OrderProcessor
//    {
//        // Estado global estático (mala idea)
//        public static List<Product> Products = new();
//        public static List<Customer> Customers = new();
//        public static List<Order> Orders = new();

//        // Dependencias concretas (DIP roto)
//        private ConsoleNotifier _notifier = new();
//        private FileRepo _repo = new();

//        public void Seed()
//        {
//            Products = new List<Product> {
//            new Product{Sku="P1",Name="Mouse",Price=100m,WeightKg=0.2m},
//            new Product{Sku="P2",Name="Teclado",Price=300m,WeightKg=0.8m},
//            new Product{Sku="P3",Name="Laptop",Price=6000m,WeightKg=2.5m},
//        };
//        }

//        public void ListProducts()
//        {
//            foreach (var p in Products) Console.WriteLine($"{p.Sku} - {p.Name} - {p.Price:C} - {p.WeightKg}kg");
//        }

//        public void RegisterCustomer(string name, string email, string phone)
//        {
//            var c = new Customer { Id = Guid.NewGuid().ToString("N"), Name = name, Email = email, Phone = phone };
//            Customers.Add(c);
//            Console.WriteLine($"Cliente registrado: {c.Name} ({c.Id})");
//        }

//        // Método monolítico con lógica mezclada (SRP roto).
//        public void CreateOrderAndPay(string? customerId, string? sku, int qty, string? promo, string? payment, string? shipping)
//        {
//            try
//            {
//                if (string.IsNullOrWhiteSpace(customerId) || string.IsNullOrWhiteSpace(sku) || qty <= 0)
//                {
//                    Console.WriteLine("Datos inválidos."); return;
//                }

//                var c = Customers.FirstOrDefault(x => x.Id == customerId);
//                var p = Products.FirstOrDefault(x => x.Sku == sku);
//                if (c == null || p == null) { Console.WriteLine("Cliente/Producto no encontrado."); return; }

//                var order = new Order { Id = Guid.NewGuid().ToString("N"), CustomerId = c.Id, PaymentType = payment ?? "cash", ShippingType = shipping ?? "standard" };
//                order.Lines.Add(new OrderLine { Sku = p.Sku, Qty = qty, UnitPrice = p.Price, WeightKg = p.WeightKg });

//                // Calcular subtotal
//                order.Subtotal = order.Lines.Sum(l => l.UnitPrice * l.Qty);

//                // Aplicar promo (OCP roto: if/else por variantes)
//                if (promo == "bf") order.Subtotal *= 0.7m;
//                else if (promo == "vip") order.Subtotal *= 0.85m;
//                else if (promo == "employee") order.Subtotal *= 0.5m;
//                else if (!string.IsNullOrWhiteSpace(promo) && promo != "standard")
//                    Console.WriteLine("Promo desconocida, ignorada.");

//                // Elegir envío (OCP/DIP rotos, LSP en riesgo)
//                ShippingBase ship;
//                var totalWeight = order.Lines.Sum(l => l.WeightKg * l.Qty);
//                if (shipping == "standard") ship = new StandardShipping();
//                else if (shipping == "express") ship = new ExpressShipping();
//                else if (shipping == "drone") ship = new DroneShipping();
//                else ship = new StandardShipping();

//                order.Shipping = ship.CostFor(totalWeight, order.Subtotal);

//                // Elegir pago (OCP/DIP rotos, LSP: Crypto puede lanzar)
//                bool paid = false;
//                if (payment == "card") paid = new CardPayment().Charge(order.Subtotal + order.Shipping);
//                else if (payment == "cash") paid = new CashPayment().Charge(order.Subtotal + order.Shipping);
//                else if (payment == "crypto") paid = new CryptoPayment().Charge(order.Subtotal + order.Shipping);
//                else paid = new CashPayment().Charge(order.Subtotal + order.Shipping);

//                if (!paid) { Console.WriteLine("Pago rechazado."); return; }

//                // Completar envío (puede lanzar en drone >2kg)
//                ship.Ship("Zona 10, Ciudad", totalWeight);

//                order.Total = order.Subtotal + order.Shipping;
//                order.Paid = true;

//                Orders.Add(order);
//                _repo.SaveOrder(order);

//                // Notificar (ISP roto: interfaz exige Push; DIP roto: uso directo de concreción)
//                _notifier.SendEmail(c.Email, "Pedido confirmado", $"Total: {order.Total:C}");
//                try { _notifier.SendPush("device-xyz", "Push innecesario"); } catch { }

//                Console.WriteLine($"Pedido {order.Id} creado y pagado. Total: {order.Total:C}");
//            }
//            catch (Exception ex)
//            {
//                // Swallow exceptions (mala práctica): oculta problemas reales
//                Console.WriteLine("[WARN] Algo falló pero ignoramos: " + ex.Message);
//            }
//        }

//        public void ListOrders()
//        {
//            foreach (var o in Orders)
//                Console.WriteLine($"{o.Id} - C:{o.CustomerId} Sub:{o.Subtotal:C} Ship:{o.Shipping:C} Total:{o.Total:C} Paid:{o.Paid}");
//        }
//    }

//}
