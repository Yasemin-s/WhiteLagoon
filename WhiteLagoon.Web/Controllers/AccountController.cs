using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Application.Common.Utility;
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
            //SD.role_Admin diyerek, sd sinifindaki admin i kontrol etmek istedigimizi soyledik.
            if (!_roleManager.RoleExistsAsync(SD.Role_Admin).GetAwaiter().GetResult())
            {
                //admn adinda asenkron islemin bitmesi beklenerek rol olusturulmustur.
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).Wait();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer)).Wait();
            }

            RegisterVM registerVM = new()
            {
                RoleList = _roleManager.Roles.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Name
                })
            };

            return View(registerVM);
        }
 

        //kullanıcıyı vt ye kaydetmek icin kullanici verilerini alip esitledik.
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            
            ApplicationUser user = new()
            {
                Name = registerVM.Name,
                Email = registerVM.Email,
                PhoneNumber = registerVM.PhoneNumber,
                NormalizedEmail = registerVM.Email.ToUpper(),
                EmailConfirmed = true,
                UserName = registerVM.Email,
                CreatedAt = DateTime.Now    //hesabin ne zaman olsuturuldugu bilgisini tasir.
            };

            //ef yyerine, yardimci metot kullandil.
            var result = await _userManager.CreateAsync(user, registerVM.Password);
            
            //kullanıcı olusturma basarili ise
            if(result.Succeeded)
            {
                //rol secimi yapilmissa, secilen rol atanacaktir.
                if(!string.IsNullOrEmpty(registerVM.Role))
                {
                    //kullanicinin secmis oldugu rolu atar
                    await _userManager.AddToRoleAsync(user, registerVM.Role);
                }
                else
                {
                    //kullanici rol secmemisse default customer atar
                    await _userManager.AddToRoleAsync(user, SD.Role_Customer);
                }
           

                //oturum acma kismi , presistent ise tarayiciyi kapattigi anda oturumu sonlandirir, yani giris icin tekrar bilgi ister.
                await _signInManager.SignInAsync(user, isPersistent:false);

                if(string.IsNullOrEmpty(registerVM.RedirectUrl)) 
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return LocalRedirect(registerVM.RedirectUrl);
                }

                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            //registerVM sinifinin RoleList ozelligine rol ekler
            registerVM.RoleList = _roleManager.Roles.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Name
                });

            return View(registerVM);
        }

        //Login
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (ModelState.IsValid)
            {
                //basarisiz giris sonunda hesap kitlenmez.
                var result = await _signInManager
                    .PasswordSignInAsync(loginVM.Email, loginVM.Password, loginVM.RememberMe, lockoutOnFailure: false);



                //kullanıcı olusturma basarili ise
                if (result.Succeeded)
                {
                    if (string.IsNullOrEmpty(loginVM.RedirectUrl))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return LocalRedirect(loginVM.RedirectUrl);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                }
            }

            return View(loginVM);
        }

    }

}
