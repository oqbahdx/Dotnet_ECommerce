using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ECommerce.Domain.Enums;

namespace ECommerce.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        // User relation
        public Guid UserId { get; private set; }
        public User User { get; private set; } = null!;

        // Order details
        public string OrderNumber { get; private set; } = string.Empty;
        public decimal TotalAmount { get; private set; }
        public decimal ShippingCost { get; private set; }
        public decimal Discount { get; private set; }
        public OrderStatus Status { get; private set; } = OrderStatus.Pending;
        public PaymentMethod PaymentMethod { get; private set; }

        // Address (value object or entity)
        public string ShippingAddress { get; private set; } = string.Empty;

        // Timestamps
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime? PaidAt { get; private set; }
        public DateTime? ShippedAt { get; private set; }
        public DateTime? DeliveredAt { get; private set; }

        // Items
        public ICollection<OrderItem> Items { get; private set; } = new List<OrderItem>();

        private Order() { }

        public Order(Guid userId, string shippingAddress, PaymentMethod paymentMethod)
        {
            UserId = userId;
            ShippingAddress = shippingAddress;
            PaymentMethod = paymentMethod;
            OrderNumber = GenerateOrderNumber();
        }

        // Add item to order
        public void AddItem(Guid productId, string productName, decimal price, int quantity)
        {
            Items.Add(new OrderItem(productId, productName, price, quantity, Id));
            CalculateTotal();
        }

        // Calculate totals
        private void CalculateTotal()
        {
            TotalAmount = Items.Sum(i => i.TotalPrice) + ShippingCost - Discount;
        }

        // Mark order as paid
        public void MarkAsPaid()
        {
            Status = OrderStatus.Paid;
            PaidAt = DateTime.UtcNow;
        }

        // Mark as shipped
        public void MarkAsShipped()
        {
            Status = OrderStatus.Shipped;
            ShippedAt = DateTime.UtcNow;
        }

        // Mark as delivered
        public void MarkAsDelivered()
        {
            Status = OrderStatus.Delivered;
            DeliveredAt = DateTime.UtcNow;
        }

        // Cancel order
        public void Cancel()
        {
            Status = OrderStatus.Cancelled;
        }

        private string GenerateOrderNumber()
        {
            return $"ORD-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString()[..6]}";
        }
    }
}
