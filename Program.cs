// Additional libraries
using Microsoft.EntityFrameworkCore;
using LoginAndRegistration.Models;
// Creates builder (also part of boilerplate code for web apps)
var builder = WebApplication.CreateBuilder(args);
//  Creates the db connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// Adds database connection - must be before app.Build();
builder.Services.AddDbContext<MyContext>(options => // change MyContext to match context file name
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});



// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession(); // add to use session

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseSession(); // add to use session

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
