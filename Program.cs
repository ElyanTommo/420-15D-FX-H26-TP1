using _420_15D_FX_H26_TP1.Data;
using _420_15D_FX_H26_TP1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


// Ajoute ASP.NET Identity (utilisateurs + r�les)
builder.Services.AddDefaultIdentity<Utilisateur>(options =>
{
    options.Password.RequireDigit = true;          // Exige au moins un chiffre
    options.Password.RequiredLength = 6;           // Longueur minimale du mot de passe
    options.Password.RequireUppercase = false;     // Majuscule non obligatoire
})
.AddEntityFrameworkStores<ApplicationDbContext>();

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();


builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
