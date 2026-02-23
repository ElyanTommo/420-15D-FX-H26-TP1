using _420_15D_FX_H26_TP1.Data;
using _420_15D_FX_H26_TP1.Identity;
using _420_15D_FX_H26_TP1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


// Ajoute ASP.NET Identity (utilisateurs + r�les)
builder.Services.AddDefaultIdentity<Utilisateur>(options =>
{
    //options.Password.RequireDigit = true;
    //options.Password.RequiredLength = 6;
    //options.Password.RequireUppercase = false;
    options.SignIn.RequireConfirmedAccount = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddErrorDescriber<CustomIdentityErrorDescriber>();


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin")); // Seuls les utilisateurs avec le rôle "Admin" peuvent accéder
});

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeAreaFolder("Admin", "/", "AdminOnly");
});

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();


builder.Services.AddRazorPages();

var app = builder.Build();
using (var scope = app.Services.CreateScope())

{

    var service = scope.ServiceProvider;

    var context = service.GetRequiredService<ApplicationDbContext>();

    var userManager = service.GetRequiredService<UserManager<Utilisateur>>();

    var roleManager = service.GetRequiredService<RoleManager<IdentityRole>>();

    await DbInitializer.Initialize(context, userManager, roleManager);

}



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    //app.UseMigrationsEndPoint();
    app.UseExceptionHandler("/Error/500");   
    app.UseStatusCodePagesWithReExecute("/Error/{0}");
}
//else
//{
//    app.UseExceptionHandler("/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
