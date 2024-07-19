using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SHOPTV.Data;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<SHOPTVContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SHOPTVContext") ?? throw new InvalidOperationException("Connection string 'SHOPTVContext' not found.")));

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
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

  
});
app.MapControllerRoute(
      name: "default",
      pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
