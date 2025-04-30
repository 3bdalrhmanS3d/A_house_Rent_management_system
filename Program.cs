using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Tasken2.DBContext;
using Tasken2.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ApartmentConnectionString")));
//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<AppDbContext>();
builder.Services.AddScoped<IPasswordHasher<Tasken2.Models.Person>, PasswordHasher<Tasken2.Models.Person>>();

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    var hasher = services.GetRequiredService<IPasswordHasher<Person>>();

    context.Database.Migrate();

    bool anyAdmin = context.Persons.Any(p => p.AccountType == "admin");
    if (!anyAdmin)
    {
        var defaultAdmin = new Person
        {
            FullName = "Default Admin",
            nationalID = "00000000000000",
            phoneNumber = "01234567890",
            email = "admin@taskeen.com",
            AccountType = "admin",
            CreatedAt = DateTime.UtcNow
        };
        
        string rawPassword = "Admin@123";
        defaultAdmin.PasswordHash = hasher.HashPassword(defaultAdmin, rawPassword);

        context.Persons.Add(defaultAdmin);
        context.SaveChanges();

        Console.WriteLine("Seeded default admin → email: admin@taskeen.com, password: Admin@123");
    }
}
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();
app.UseSession(); // Use session middleware
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
