using Microsoft.AspNetCore.Identity; // Add this
using Microsoft.EntityFrameworkCore;
using FlipkartClone.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. Database Connection (You already have this)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Add Identity Services (ADD THIS BLOCK)
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// ... existing code ...

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 3. Enable Authentication & Authorization (ADD THIS ORDER MATTERS!)
app.UseAuthentication(); // Must be before Authorization
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// 4. Map Razor Pages (Identity uses Razor Pages for Login/Register)
app.MapRazorPages(); 

app.Run();