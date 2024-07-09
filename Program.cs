using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NHOM04.Data;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<NHOM04Context>(options =>

    options.UseSqlServer(builder.Configuration.GetConnectionString("NHOM04Context") ?? throw new InvalidOperationException("Connection string 'NHOM04Context' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession(options =>
{
    options.Cookie.Name = "MySession";
    options.IdleTimeout = new TimeSpan(15, 0, 0, 0);
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
});


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
