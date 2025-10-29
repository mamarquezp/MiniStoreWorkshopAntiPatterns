using MiniStore.Application.UseCases;
using MiniStore.Domain.Abstractions;
using MiniStore.Infraestructure.Promotions;
using MiniStore.Infrastructure;
using MiniStore.Infrastructure.Notifications;
using MiniStore.Infrastructure.Payments;
using MiniStore.Infrastructure.Shipping;
using MiniStore.Presentation;

namespace MiniStore
{
    public class Program
    {
        // El Main ahora es el Composition Root y maneja la inyección de dependencias
        static void Main(string[] args)
        {
            Console.WriteLine("MiniStore Workshop - SOLID Refactorizado");

            // --- 1. Configurar Servicios de Infraestructura (Implementaciones) ---

            // Repositorios
            var productRepo = new InMemoryProductRepository();
            var customerRepo = new InMemoryCustomerRepository();
            var orderRepo = new FileOrderRepository();

            productRepo.Seed(); // "Sembrar" datos

            // Notificaciones (ISP)
            var emailNotifier = new ConsoleEmailNotifier();

            // Estrategias (OCP)
            var promoStrategies = new Dictionary<string, IPromoStrategy>
            {
                ["standard"] = new StandardPromoStrategy(),
                ["bf"] = new PercentagePromoStrategy("bf", 0.7m),
                ["vip"] = new PercentagePromoStrategy("vip", 0.85m),
                ["employee"] = new PercentagePromoStrategy("employee", 0.5m)
            };

            var paymentGateways = new Dictionary<string, IPaymentGateway>
            {
                ["cash"] = new CashPaymentGateway(),
                ["card"] = new CardPaymentGateway(),
                ["crypto"] = new CryptoPaymentGateway()
            };

            var shippingStrategies = new Dictionary<string, IShippingStrategy>
            {
                ["standard"] = new StandardShipping(),
                ["express"] = new ExpressShipping(),
                ["drone"] = new DroneShipping()
            };

            // --- 2. Configurar Servicios de Aplicación (Casos de Uso) ---

            var orderService = new OrderService(
                customerRepo,
                productRepo,
                orderRepo,
                emailNotifier,
                promoStrategies,
                paymentGateways,
                shippingStrategies
            );

            // --- 3. Configurar Presentación ---

            var menu = new MenuHandler(
                orderService,
                productRepo,
                customerRepo,
                orderRepo
            );

            // --- 4. Ejecutar ---
            menu.Run();

            Console.WriteLine("Fin del taller.");
        }
    }
}
