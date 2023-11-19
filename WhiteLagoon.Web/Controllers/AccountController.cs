using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Web.ViewModels;

namespace WhiteLagoon.Web.Controllers
{
    public class AccountController : Controller
    {

        //bagimlilik enjjeksiyonu ile enjjekte etmek gereklidir.
        private readonly IUnitOfWork _unitOfWork;   //vt baglantisi icin , is birimi icindeki sb setler ile yapilacaktir.
        private readonly UserManager<ApplicationUser> _userManager; //kullanci yöneticisi icin uygulama kullanicilari turu veildi.
        private readonly SignInManager<ApplicationUser> _signInManager;  //yonetici, oturum acacak kisilerinn islemlerine karar verir
        private readonly RoleManager<IdentityRole> _roleManager; //ilerleyen asamlarda web sitesine kural koymak istenirse.

        //enjekte etmek icin conc kullandik.
        public AccountController(IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public IActionResult Login(string returnUrl=null)
        {
            returnUrl ??= Url.Content("~/");    //donus url bos degilse icerigi doldur dedik.

            LoginVM loginVM = new()
            {
                RedirectUrl = returnUrl
            };

            return View(loginVM);
        }
        public IActionResult Register()
        {
            //kural yoksa olustuacak - rol kotnrolu yapiyor, yoksa ikisini de yarat dedik.
            if (!_roleManager.RoleExistsAsync("Admin").GetAwaiter().GetResult())
            {
                //admn adinda asenkron islemin bitmesi beklenerek rol olusturulmustur.
                _roleManager.CreateAsync(new IdentityRole("Admin")).Wait();
                _roleManager.CreateAsync(new IdentityRole("Customer")).Wait();
            }

            return View();
        }
    }
}
