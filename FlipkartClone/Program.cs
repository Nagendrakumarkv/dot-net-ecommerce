using Microsoft.AspNetCore.Identity; // Add this
using Microsoft.EntityFrameworkCore;
using FlipkartClone.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. Database Connection (You already have this)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Add Identity Services (ADD THIS BLOCK)
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>() // <--- THIS LINE IS CRITICAL
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

// Add Session Support
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Cart clears after 30 mins of inactivity
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

var app = builder.Build();

// ... existing code ...

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession(); // <--- Add this line

// 3. Enable Authentication & Authorization (ADD THIS ORDER MATTERS!)
app.UseAuthentication(); // Must be before Authorization
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// 4. Map Razor Pages (Identity uses Razor Pages for Login/Register)
app.MapRazorPages();

// --- ADD THIS BLOCK HERE ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        await FlipkartClone.Data.DbSeeder.SeedRolesAndAdminAsync(services);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"--> [Seeder] An error occurred: {ex.Message}");
    }
}
// ---------------------------

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

// Add this line OUTSIDE the if-block to handle 404s in Dev mode too
app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");

app.Run();