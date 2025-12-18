using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace FlipkartClone.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        // GET: /Payment/Index?addressId=5
        // This is the step where we "hand over" the user to the gateway
        public IActionResult Index(int addressId)
        {
            ViewBag.AddressId = addressId;
            return View();
        }

        // POST: /Payment/Process
        // This simulates the "Bank" verifying your details
        [HttpPost]
        public async Task<IActionResult> Process(int addressId, string cardNumber, string otp)
        {
            // SIMULATION: Fake processing delay
            await Task.Delay(2000); // Wait 2 seconds

            // SIMULATION: Simple validation (accept any card ending in 1)
            if (!string.IsNullOrEmpty(cardNumber) && !string.IsNullOrEmpty(otp))
            {
                // SUCCESS: Redirect to the Order Placement logic (Day 12)
                // In a real app, we would send a "Transaction ID" along with this
                return RedirectToAction("PlaceOrder", "Order", new { addressId = addressId });
            }
            else
            {
                // FAILURE: Show error
                TempData["Error"] = "Payment Failed! Invalid Details.";
                return RedirectToAction("Index", new { addressId = addressId });
            }
        }
    }
}