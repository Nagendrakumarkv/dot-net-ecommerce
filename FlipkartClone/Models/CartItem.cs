namespace FlipkartClone.Models
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ImageUrl { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }

        // Helper to calculate total for this row
        public double Total => Price * Quantity;
    }
}