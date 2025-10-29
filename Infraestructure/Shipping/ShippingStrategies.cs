using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniStore.Domain.Abstractions;

namespace MiniStore.Infrastructure.Shipping
{
    public class StandardShipping : IShippingStrategy
    {
        public string ShippingMethodName => "standard";
        public bool CanShip(decimal totalWeightKg) => true; // Siempre se puede usar

        public decimal CalculateCost(decimal totalWeightKg, decimal subtotal)
        {
            return totalWeightKg <= 5 ? 25 : 50;
        }

        public void Ship(string address, decimal totalWeightKg)
        {
            Console.WriteLine($"[SHIP] Standard a {address} ({totalWeightKg}kg)");
        }
    }

    public class ExpressShipping : IShippingStrategy
    {
        public string ShippingMethodName => "express";
        public bool CanShip(decimal totalWeightKg) => true; // Siempre se puede usar

        public decimal CalculateCost(decimal totalWeightKg, decimal subtotal)
        {
            return 60; // Tarifa plana
        }

        public void Ship(string address, decimal totalWeightKg)
        {
            Console.WriteLine($"[SHIP] Express a {address} ({totalWeightKg}kg)");
        }
    }

    public class DroneShipping : IShippingStrategy
    {
        public string ShippingMethodName => "drone";

        //LSP: Hacemos la precondición explícita
        public bool CanShip(decimal totalWeightKg)
        {
            return totalWeightKg <= 2;
        }

        public decimal CalculateCost(decimal totalWeightKg, decimal subtotal)
        {
            return 15;
        }

        public void Ship(string address, decimal totalWeightKg)
        {
            // Ya no se necesita lanzar excepción aquí por la prevalidación
            if (totalWeightKg > 2)
            {
                throw new InvalidOperationException("Drone no puede > 2kg (Error de validación previa)"); // Ya no debería llegar acá
            }
            Console.WriteLine($"[SHIP] Drone a {address} ({totalWeightKg}kg)");
        }
    }
}
