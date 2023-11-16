using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;  //veri tabani icin

//villa detaylarını goruntuleme , silme , guncelleme islemleri icin olusturulan controller dir.
namespace WhiteLagoon.Web.Controllers
{
    public class VillaController : Controller
    {
        //veritabani baglami icin _db adinda nesne olusturulmus. bu nesne vt islemlerini gerceklestirecek.
        private readonly IUnitOfWork _unitOfWork;
        //contructor icin kisa yol ctor tab tab
        public VillaController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var villas = _unitOfWork.Villa.GetAll();    //vt de Villas tablosunu cagirdik.
            return View(villas);
        }

        //villa olusturma kismi - get kismi
        public IActionResult Create()
        {
            return View();

        }

        //post kismi
        [HttpPost]
        public IActionResult Create(Villa obj)
        {
            //isim ve aciklama ayni olamaz kismini donduruyor - ozel dogrulama kismi
            if(obj.Name == obj.Description)
            {
                //hata iletisi belirli bir ozellik icin eklenirse , sadece o ozellikte gecerli olur.name gibi
                ModelState.AddModelError("name", "isim ve aciklama eslesemez.");
            }


            //modelin gecerli olup olmadigini kontrol ediyor. yani giris kisimlarini propler ile kotnrol edip sikinti yoksa if e girdiyor.
            if (ModelState.IsValid) {
                _unitOfWork.Villa.Add(obj);
                TempData["success"] = "The villa has been created successfully.";
                //villa controllerini icindeki index e git dedik
                return RedirectToAction(nameof(Index));
            }
             return View(obj); 
        }

        //id ye gore guncelleme yailacak
        public IActionResult Update(int villaId)
        {
            Villa? obj = _unitOfWork.Villa.Get(u => u.Id == villaId);   //tek 1 kayıt almak gerektiginde kullanilabilr
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
                _unitOfWork.Villa.Update(obj);
                TempData["success"] = "The villa has been updated successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }

        //silme kismi
        public IActionResult Delete(int villaId)
        {
            Villa? obj = _unitOfWork.Villa.Get(u => u.Id == villaId);   //Villa? diyerek, atanan değer ya Villa türündedir ya da null dur dedik.
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
            Villa? objFromDb = _unitOfWork.Villa.Get(u => u.Id == obj.Id);
            if (objFromDb is not null)
            {
                _unitOfWork.Villa.Remove(objFromDb);
                TempData["success"] = "The villa has been deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "The villa could not be deleted.";
            return View(obj);

        }

    }
}
