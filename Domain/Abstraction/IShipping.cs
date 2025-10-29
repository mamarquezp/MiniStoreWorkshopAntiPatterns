using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniStore.Domain.Abstractions
{
    public interface IShippingStrategy
    {
        string ShippingMethodName { get; }

        // Contrato LSP: debe devolver un costo o lanzar excepción
        decimal CalculateCost(decimal totalWeightKg, decimal subtotal);

        // Contrato LSP: debe poder enviar o lanzar excepción si falla
        void Ship(string address, decimal totalWeightKg);

        // Para arreglar LSP, hacemos explícita la condición del peso
        bool CanShip(decimal totalWeightKg);
    }
}
