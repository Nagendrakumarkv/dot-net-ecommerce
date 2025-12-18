using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using FlipkartClone.Data;
using FlipkartClone.Models;
using FlipkartClone.Extensions;

namespace FlipkartClone.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: /Order/PlaceOrder
        // [HttpPost]
        [HttpGet]
        public IActionResult PlaceOrder(int addressId)
        {
            // 1. Get User ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // 2. Get Cart
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart");
            if (cart == null || cart.Count == 0) return RedirectToAction("Index", "Home");

            // 3. Create Order Object
            var order = new Order
            {
                UserId = userId,
                AddressId = addressId,
                OrderDate = DateTime.UtcNow,
                TotalAmount = cart.Sum(x => x.Total),
                OrderStatus = "Placed",
                OrderItems = new List<OrderItem>()
            };

            // 4. Create Order Items & Reduce Stock (Simulated)
            foreach (var item in cart)
            {
                var orderItem = new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price
                };
                order.OrderItems.Add(orderItem);

                // Optional: Reduce stock logic here if you had a 'Stock' field
                // var product = _context.Products.Find(item.ProductId);
                // product.Stock -= item.Quantity;
            }

            // 5. Save to DB
            _context.Orders.Add(order);
            _context.SaveChanges();

            // 6. Clear Cart
            HttpContext.Session.Remove("Cart");

            // 7. Show Success Page
            return RedirectToAction("OrderConfirmation", new { id = order.Id });
        }

        // GET: /Order/OrderConfirmation/5
        public IActionResult OrderConfirmation(int id)
        {
            return View(id); // Just pass the Order ID to display
        }
    }
}