using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Add this
using FlipkartClone.Models;
using FlipkartClone.Data; // Add this

namespace FlipkartClone.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context; // 1. Add DB Context

    // 2. Inject DB Context in Constructor
    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index(string searchString, string sortOrder, int? minPrice, int? maxPrice)
    {
        // 1. Start with ALL products
        var products = from p in _context.Products.Include(c => c.Category)
                       select p;

        // 2. Apply Search Filter
        if (!string.IsNullOrEmpty(searchString))
        {
            products = products.Where(s => s.Title.ToLower().Contains(searchString.ToLower())
                                        || s.Description.ToLower().Contains(searchString.ToLower()));
        }

        // 3. Apply Price Filter
        if (minPrice.HasValue)
        {
            products = products.Where(p => p.Price >= minPrice);
        }
        if (maxPrice.HasValue)
        {
            products = products.Where(p => p.Price <= maxPrice);
        }

        // 4. Apply Sorting
        // Pass the sort order back to View so we know which is active
        ViewData["CurrentSort"] = sortOrder;

        switch (sortOrder)
        {
            case "price_desc":
                products = products.OrderByDescending(p => p.Price);
                break;
            case "price_asc":
                products = products.OrderBy(p => p.Price);
                break;
            default:
                products = products.OrderByDescending(p => p.Id); // Default: Newest first
                break;
        }

        // 5. Execute the query
        return View(await products.ToListAsync());
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}