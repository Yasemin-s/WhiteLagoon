using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;
using WhiteLagoon.Web.ViewModels;  //veri tabani icin

//villa detaylarını goruntuleme , silme , guncelleme islemleri icin olusturulan controller dir.
namespace WhiteLagoon.Web.Controllers
{
    public class VillaNumberController : Controller
    {
        //veritabani baglami icin _db adinda nesne olusturulmus. bu nesne vt islemlerini gerceklestirecek.. veri tabanını direk biliyor çünkü .web projesine .infrasttructure ı referans verdik.
        private readonly ApplicationDbContext _db;

        //contructor icin kisa yol ctor tab tab
        public VillaNumberController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var villaNumbers = _db.VillaNumbers.Include(u=>u.Villa).ToList();   
            return View(villaNumbers);  //gorunume git dersen, 3 tane seyin donduruldugunu gorursun.
        }

        //villa olusturma kismi - get kismi
        public IActionResult Create()
        {

            ////villa numaralarini listeletmek icin
            //IEnumerable<SelectListItem> list = _db.Villas.ToList().Select(u => new SelectListItem{ 
            //    Text = u.Name,
            //    Value = u.Id.ToString()
            //});
            ////dinamik yapi oldugu icin view bag kullanildi
            //ViewBag.VillaList = list;
            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _db.Villas.ToList().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                })
            };
            return View(villaNumberVM);

        }

        //post kismi
        [HttpPost]
        public IActionResult Create(VillaNumberVM obj)
        {
            //oda numarasi kontrolu yapir.
            bool roomNumberExists = _db.VillaNumbers.Any(u => u.Villa_Number == obj.VillaNumber.Villa_Number);

           if (ModelState.IsValid && !roomNumberExists) 
            { 
                _db.VillaNumbers.Add(obj.VillaNumber);  //artik _db.Add() seklinde kullanabiirin, aldigi nesneye gore referans alacak.
                _db.SaveChanges();
                TempData["success"] = "The Villa Number has been created successfully.";
                return RedirectToAction(nameof(Index));
            }
            if (roomNumberExists)
            {
                TempData["error"] = "The villa Number already exists.";
            }
            //dropdown menuyu tekrar doldurmayi sagliyor
            obj.VillaList = _db.Villas.ToList().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
             return View(obj); 

        }

        //id ye gore guncelleme apmak icin View e gonderiyor.
        public IActionResult Update(int villaNumberId)
        {
            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _db.Villas.ToList().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                VillaNumber = _db.VillaNumbers.FirstOrDefault(u => u.Villa_Number == villaNumberId)
            };
            if(villaNumberVM.VillaNumber == null)
            {
                return RedirectToAction("Error","Home");
            }
            return View(villaNumberVM);
        }


        //post kismi - data endpoints
        [HttpPost]
        public IActionResult Update(VillaNumberVM villaNumberVM)
        {
            if (ModelState.IsValid)
            {
                _db.VillaNumbers.Update(villaNumberVM.VillaNumber);
                _db.SaveChanges();
                TempData["success"] = "The villa has been updated successfully";
                return RedirectToAction(nameof(Index));
            }
            villaNumberVM.VillaList = _db.Villas.ToList().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            return View(villaNumberVM);
        }


        //silme kismi
        public IActionResult Delete(int villaNumberId)
        {
            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _db.Villas.ToList().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                VillaNumber = _db.VillaNumbers.FirstOrDefault(u => u.Villa_Number == villaNumberId)
            };
            if (villaNumberVM.VillaNumber == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(villaNumberVM);
        }


        [HttpPost]
        public IActionResult Delete(VillaNumberVM villaNumberVM)
        {
            VillaNumber? objFromDb = _db.VillaNumbers
                .FirstOrDefault(u => u.Villa_Number == villaNumberVM.VillaNumber.Villa_Number);
            if (objFromDb is not null)
            {
                _db.VillaNumbers.Remove(objFromDb);
                _db.SaveChanges();
                TempData["success"] = "The villa number has been deleted successfully.";
                return RedirectToAction(nameof(Index));  //burada "Indexa" seklinde yazim yanlisi yapilirsa hata mesai vermez. bu sekilde kullanirsam hata mesai gosterir.
            }
            TempData["error"] = "The villa number could not be deleted.";
            return View();

        }

    }
}
