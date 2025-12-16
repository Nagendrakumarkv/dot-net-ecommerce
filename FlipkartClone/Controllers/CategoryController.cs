using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FlipkartClone.Data;
using FlipkartClone.Models;
using FlipkartClone.Constants;

namespace FlipkartClone.Controllers
{
    [Authorize(Roles = Roles.Admin)] // SECURE THIS CONTROLLER
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Category/Index (List)
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.OrderBy(c => c.DisplayOrder).ToListAsync());
        }

        // GET: Category/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Category/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();
            return View(category);
        }

        // POST: Category/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (id != category.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }
        
        // GET: Category/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
             if (id == null) return NotFound();
             var category = await _context.Categories.FindAsync(id);
             if (category == null) return NotFound();
             
             // Directly delete for speed (in real apps, show a confirmation page)
             _context.Categories.Remove(category);
             await _context.SaveChangesAsync();
             return RedirectToAction(nameof(Index));
        }
    }
}