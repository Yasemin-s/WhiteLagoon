//web projesinin tum yapýlaandýrýlmasý bu dosyada olur.
//yapilandirma icin kok dosya olur.

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;
using WhiteLagoon.Infrastructure.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//servis ekleme kismi
builder.Services.AddControllersWithViews();
//database icin eklenen servis
builder.Services.AddDbContext<ApplicationDbContext>(option =>   //identityuser yerine applicationuser yazildi, kimlik kullanýcýsý uygulama kullanicisi olarak degistirildi.
option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//kimlik dogrulama ve yetkilendirme icin gerekli servis
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>(); //kullanici ve kimlik rolu tanimlanmistir.

//repository icin eklenen service(ana repository ve kullanacak olan sinif). soyut olan IVillaRepository, somut olan VillaRepository  dir.
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

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
