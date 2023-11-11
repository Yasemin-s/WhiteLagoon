//web projesinin tum yapýlaandýrýlmasý bu dosyada olur.
//yapilandirma icin kok dosya olur.

using Microsoft.EntityFrameworkCore;
using WhiteLagoon.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//servis ekleme kismi
builder.Services.AddControllersWithViews();
//database icin eklenen servis
builder.Services.AddDbContext<ApplicationDbContext>(option =>
option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
//istek ekleme kismi
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
