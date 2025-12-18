using FlipkartClone.Models;

namespace FlipkartClone.Models.ViewModels
{
    public class CheckoutViewModel
    {
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
        public List<Address> Addresses { get; set; } = new List<Address>();
        public double GrandTotal => CartItems.Sum(x => x.Total);
    }
}