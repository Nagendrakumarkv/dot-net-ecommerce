using System.ComponentModel.DataAnnotations;

namespace FlipkartClone.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string UserId { get; set; } // The customer
        public int AddressId { get; set; } // Where it's going
        public double TotalAmount { get; set; }
        public string OrderStatus { get; set; } = "Placed"; // Placed, Shipped, Delivered
        
        // Navigation Property
        public List<OrderItem> OrderItems { get; set; }
    }
}