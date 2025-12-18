using Microsoft.AspNetCore.Mvc;
using FlipkartClone.Data;
using FlipkartClone.Models;
using FlipkartClone.Extensions;
using Microsoft.AspNetCore.Authorization; // Needed for [Authorize]
using Microsoft.AspNetCore.Identity; // Needed to get User ID
using System.Security.Claims;
using FlipkartClone.Models.ViewModels; // Add this

namespace FlipkartClone.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const string CartSessionKey = "Cart";

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Cart
        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>(CartSessionKey) ?? new List<CartItem>();
            return View(cart);
        }

        // GET: /Cart/Add/5
        public IActionResult Add(int id)
        {
            // 1. Get current cart
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>(CartSessionKey) ?? new List<CartItem>();

            // 2. Check if item already exists
            var existingItem = cart.FirstOrDefault(c => c.ProductId == id);

            if (existingItem != null)
            {
                // If in cart, just increase quantity
                existingItem.Quantity++;
            }
            else
            {
                // If not, fetch from DB and add new
                var product = _context.Products.Find(id);
                if (product != null)
                {
                    cart.Add(new CartItem
                    {
                        ProductId = product.Id,
                        ProductName = product.Title,
                        Price = product.Price,
                        ImageUrl = product.ImageUrl,
                        Quantity = 1
                    });
                }
            }

            // 3. Save back to session
            HttpContext.Session.SetObjectAsJson(CartSessionKey, cart);

            return RedirectToAction("Index");
        }

        // GET: /Cart/Remove/5
        public IActionResult Remove(int id)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>(CartSessionKey);

            if (cart != null)
            {
                var item = cart.FirstOrDefault(c => c.ProductId == id);
                if (item != null)
                {
                    cart.Remove(item);
                    HttpContext.Session.SetObjectAsJson(CartSessionKey, cart);
                }
            }
            return RedirectToAction("Index");
        }

        // GET: /Cart/Decrease/5
        public IActionResult Decrease(int id)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart");
            var item = cart?.FirstOrDefault(c => c.ProductId == id);

            if (item != null)
            {
                if (item.Quantity > 1)
                {
                    item.Quantity--;
                }
                else
                {
                    cart.Remove(item); // Remove if quantity goes to 0
                }
                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }
            return RedirectToAction("Index");
        }

        // GET: /Cart/Increase/5
        public IActionResult Increase(int id)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart");
            var item = cart?.FirstOrDefault(c => c.ProductId == id);

            if (item != null)
            {
                item.Quantity++;
                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }
            return RedirectToAction("Index");
        }

        [Authorize] // <--- Forces User to Login before seeing this page
        [HttpGet]
        public IActionResult Checkout()
        {
            // 1. Get Cart
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart");
            if (cart == null || cart.Count == 0)
            {
                return RedirectToAction("Index"); // Kick back if empty
            }

            // 2. Get User's Addresses
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var addresses = _context.Addresses
                .Where(a => a.UserId == userId)
                .ToList();

            // 3. Combine into ViewModel
            var model = new CheckoutViewModel
            {
                CartItems = cart,
                Addresses = addresses
            };

            return View(model);
        }

        // GET: /Cart/Payment?selectedAddressId=5
        [Authorize]
        public IActionResult Payment(int selectedAddressId)
        {
            // Pass the AddressId to the view so we can send it to the final "Place Order" step later
            ViewBag.AddressId = selectedAddressId;

            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart");
            return View(cart);
        }
    }
}