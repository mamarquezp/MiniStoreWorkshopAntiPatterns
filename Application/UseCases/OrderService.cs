using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiniStore.Domain.Entities;
using MiniStore.Domain.Abstractions;
using MiniStore.Application.DTOs;

namespace MiniStore.Application.UseCases
{
    // Orquesta el dominio y la infraestructura y Solo depende de abstracciones (DIP).
    public class OrderService
    {
        // Dependencias (inyectadas)
        private readonly ICustomerRepository _customerRepo;
        private readonly IProductRepository _productRepo;
        private readonly IOrderRepository _orderRepo;
        private readonly IEmailNotifier _emailNotifier;

        // Estrategias (inyectadas)
        private readonly IDictionary<string, IPromoStrategy> _promoStrategies;
        private readonly IDictionary<string, IPaymentGateway> _paymentGateways;
        private readonly IDictionary<string, IShippingStrategy> _shippingStrategies;

        public OrderService(
            ICustomerRepository customerRepo,
            IProductRepository productRepo,
            IOrderRepository orderRepo,
            IEmailNotifier emailNotifier,
            IDictionary<string, IPromoStrategy> promoStrategies,
            IDictionary<string, IPaymentGateway> paymentGateways,
            IDictionary<string, IShippingStrategy> shippingStrategies)
        {
            _customerRepo = customerRepo;
            _productRepo = productRepo;
            _orderRepo = orderRepo;
            _emailNotifier = emailNotifier;
            _promoStrategies = promoStrategies;
            _paymentGateways = paymentGateways;
            _shippingStrategies = shippingStrategies;
        }

        // Este método reemplaza al CreateOrderAndPay Sigue el patrón SRP
        public (Order? order, string error) CreateOrder(CreateOrderRequest request)
        {
            // 1. Validación (SRP: Validación de entrada)
            if (string.IsNullOrWhiteSpace(request.CustomerId) ||
                string.IsNullOrWhiteSpace(request.Sku) || request.Qty <= 0)
            {
                return (null, "Datos inválidos.");
            }

            // 2. Obtener Entidades
            var customer = _customerRepo.GetById(request.CustomerId);
            var product = _productRepo.GetBySku(request.Sku);
            if (customer == null || product == null)
            {
                return (null, "Cliente/Producto no encontrado.");
            }

            // 3. Resolver Estrategias (OCP)
            var promoStrategy = GetStrategy(_promoStrategies, request.PromoCode, "standard");
            var paymentGateway = GetStrategy(_paymentGateways, request.PaymentMethod, "cash");
            var shippingStrategy = GetStrategy(_shippingStrategies, request.ShippingMethod, "standard");

            // 4. Crear Entidad de Dominio
            var order = new Order(
                Guid.NewGuid().ToString("N"),
                customer.Id,
                paymentGateway.PaymentMethodName,
                shippingStrategy.ShippingMethodName
            );
            order.AddLine(product, request.Qty);

            var totalWeight = order.GetTotalWeight();

            // 5. Arreglo LSP: Validar antes de usar
            if (!shippingStrategy.CanShip(totalWeight))
            {
                return (null, $"Envío {shippingStrategy.ShippingMethodName} no soporta {totalWeight}kg.");
            }

            // 6. Calcular Costos
            var shippingCost = shippingStrategy.CalculateCost(totalWeight, order.Subtotal);
            order.SetShippingCost(shippingCost);

            var discount = promoStrategy.CalculateDiscount(order);
            order.ApplyDiscount(discount); // El total se recalcula dentro

            // 7. Procesar Pago, Arreglo LSP: El contrato de IPaymentGateway debe ser consistente.
            bool paid = paymentGateway.Charge(order.Total);
            if (!paid)
            {
                return (null, "Pago rechazado.");
            }
            order.MarkAsPaid();

            // 8. Ejecutar Envío
            try
            {
                shippingStrategy.Ship("Zona 10, Ciudad", totalWeight);
            }
            catch (Exception ex)
            {
                // Si algo falla se maneja desde aquí
                return (null, $"Error de envío: {ex.Message}");
            }

            // 9. Persistencia
            _orderRepo.Save(order);

            // 10. Notificación (ISP)
            _emailNotifier.SendEmail(customer.Email, "Pedido confirmado", $"Total: {order.Total:C}");

            return (order, "Pedido creado y pagado.");
        }

        // Helper para OCP
        private T GetStrategy<T>(IDictionary<string, T> strategies, string? key, string defaultKey)
        {
            key = (key ?? defaultKey).ToLower();
            if (strategies.TryGetValue(key, out var strategy))
            {
                return strategy;
            }
            Console.WriteLine($"[WARN] Estrategia '{key}' desconocida, usando '{defaultKey}'.");
            return strategies[defaultKey];
        }
    }
}
