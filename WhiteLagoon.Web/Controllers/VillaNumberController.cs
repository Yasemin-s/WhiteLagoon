using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;  //veri tabani icin

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
            var villaNumbers = _db.VillaNumbers.ToList();    //vt de VillaNumbers tablosunu cagirdik.
            return View(villaNumbers);
        }

        //villa olusturma kismi - get kismi
        public IActionResult Create()
        {
            return View();

        }

        //post kismi
        [HttpPost]
        public IActionResult Create(VillaNumber obj)
        {
            //villa yi mdoelden kaldirdik. yani bu model dogrulanirken Villa ozelligine bakilmadan dogrulanacak.
            //ModelState.Remove("Villa");
            //modelin gecerli olup olmadigini kontrol ediyor. yani giris kisimlarini propler ile kotnrol edip sikinti yoksa if e girdiyor.
            if (ModelState.IsValid) { 
                _db.VillaNumbers.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "The Villa Number has been created successfully.";
                //villa controllerini icindeki index e git dedik
                return RedirectToAction("Index");
            }
             return View(obj); 
        }

        //id ye gore guncelleme yailacak
        public IActionResult Update(int villaId)
        {
            Villa? obj = _db.Villas.FirstOrDefault(u => u.Id == villaId);   //tek 1 kayıt almak gerektiginde kullanilabilr
            //var Villalist = _db.Villas.ToList();    listeyle calisilmasi gerekiyorsa kullanlabilir.
            //var Villalist = _db.Villas.Where(u => u.Price > 50);    filtreleme yapilmak istenirse
            //var Villalist = _db.Villas.Where(u => u.Price > 50 && u.Occupancy > 0).FirstOrDefault();    filtreleme yapilmak istenirse
            //var Villalist = _db.Villas.Find(villaId);
            if (obj == null)
            {
                return RedirectToAction("Error","Home");
            }
            return View(obj);
        }


        //post kismi - data endpoints
        [HttpPost]
        public IActionResult Update(Villa obj)
        {
            //web sayfasindaki form verilerinin , modelle eslesip eslesmemesinin ontrol ediyor.
            if (ModelState.IsValid && obj.Id > 0)
            {
                _db.Villas.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "The villa has been updated successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        //silme kismi
        public IActionResult Delete(int villaId)
        {
            Villa? obj = _db.Villas.FirstOrDefault(u => u.Id == villaId);   
            //null ile calisirken is/is not kullanması tavsiye edilir == yeine
            if (obj is null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(obj);
        }


        [HttpPost]
        public IActionResult Delete(Villa obj)
        {
            Villa? objFromDb = _db.Villas.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb is not null)
            {
                _db.Villas.Remove(objFromDb);
                _db.SaveChanges();
                //gecici verinin anahtar kelimesi ile degerini gonderdik. bu ekranda bildirim gosterecek. anatar kelimeler ayni olamlidir.
                TempData["success"] = "The villa has been deleted successfully.";
                return RedirectToAction("Index");
            }
            TempData["error"] = "The villa could not be deleted.";
            return View(obj);

        }

    }
}
