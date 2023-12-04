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


//kimlik dogrulama ve yetkilendirme icin gerekli servis- kullanici modeli ve roller temsil edilir.
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>(); //kullanici bilgileri vt de deolanacaktir.

//yetkisiz giriste amenity yonlendirme kismi - erisim reddedildi ve gecersiz oturum kismi servisi
builder.Services.ConfigureApplicationCookie(option =>   //Bu yapý, kimlik doðrulama çerezlerinin yapýlandýrýlmasýný saðlar.
{
    option.AccessDeniedPath = "/Account/AccessDenied";  //yetkilendirme reddedildiðinde kullanýcýyý yönlendireceði sayfanýn yolunu belirtir.
    option.LoginPath = "/Account/Login";    //oturum açma iþlemi gerektiðinde kullanýcýyý yönlendireceði sayfanýn yolunu belirtir
});
builder.Services.Configure<IdentityOptions>(option =>   //kimlik doðrulama seçeneklerinin yapýlandýrýlmasýný saðlar.
{
    option.Password.RequiredLength = 6; //þifrenin en az 6 karakter uzunluðunda olmasý gerekiyor.
});

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
