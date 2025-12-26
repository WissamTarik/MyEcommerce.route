using MyEcommerce.Route.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Domain.Entities.Orders
{
    //part of table
    public class OrderItem:BaseEntity<int>
    {
        public OrderItem()
        {
            
        }

        public OrderItem(ProductInOrderItem product, int quantity, decimal price)
        {
            Product = product;
            Quantity = quantity;
            Price = price;
        }

      public  ProductInOrderItem Product { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
