namespace MiniStoreWorkshopAntiPatterns
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("MiniStore Workshop - Anti-Patrones (para refactor SOLID)");
            var app = new OrderProcessor();
            app.Seed();

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
                    case "1":
                        app.ListProducts();
                        break;
                    case "2":
                        Console.Write("Nombre: ");
                        var name = Console.ReadLine() ?? "";
                        Console.Write("Email: ");
                        var email = Console.ReadLine() ?? "";
                        Console.Write("Teléfono: ");
                        var phone = Console.ReadLine() ?? "";
                        app.RegisterCustomer(name, email, phone);
                        break;
                    case "3":
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
                        app.CreateOrderAndPay(cid, sku, qty, promo, payment, ship);
                        break;
                    case "4":
                        app.ListOrders();
                        break;
                    default:
                        Console.WriteLine("Opción inválida");
                        break;
                }
            }
            Console.WriteLine("Fin del taller.");

        }
    }
}
