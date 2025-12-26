using MyEcommerce.Route.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Domain.Entities.Orders
{
    public class Order:BaseEntity<Guid>
    {
        public Order()
        {
            
        }

        public Order(string userEmail, OrderAddress shippingAddress, DeliveryMethod deliveryMethod, IEnumerable<OrderItem> items, decimal subtotal,string? paymentIntentId)
        {
            this.userEmail = userEmail;
            ShippingAddress = shippingAddress;
            DeliveryMethod = deliveryMethod;
            Items = items;
            Subtotal = subtotal;
            PaymentIntentId = paymentIntentId;
        }

        public string userEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; }=DateTimeOffset.Now;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public OrderAddress ShippingAddress { get; set; }
        public DeliveryMethod    DeliveryMethod { get; set; }//NP
        public int DeliveryMethodId { get; set; }
        public IEnumerable<OrderItem> Items { get; set; }
        public decimal Subtotal { get; set; }

        public decimal GetTotal() => Subtotal + DeliveryMethod.Cost;
        public string? PaymentIntentId { get; set; }
    }
}
