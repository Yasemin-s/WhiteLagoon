using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.IO;
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
        private readonly IWebHostEnvironment _webHostEnvironment;   //secilen dosyayi izlemek icin, 

        public VillaController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
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

                //secilen dosyanin goruntusu kontrol edilir.
                if(obj.Image != null)
                {
                    string fileName = Guid.NewGuid().ToString()+ Path.GetExtension(obj.Image.FileName);    //. benzersiz tanımlayıcııdır(128 bitlik) tostring ile de dizeyye cevrilmistir guid . Rastgele oluşturulan bu numaralar genellikle tekil kimlikler oluşturmak, nesneleri takip etmek veya öğeleri benzersiz bir şekilde tanımlamak için kullanılır. 
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"images\VillaImage");    //webroot , root klasoru yolunu verir.

                    //goruntu eklemek icin dosya akisi olusturma
                    using var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create);
                    obj.Image.CopyTo(fileStream);

                    obj.ImageUrl = @"\images\VillaImage\" + fileName;
                }
                else
                {
                    obj.ImageUrl = "http://placehold.co/600x400";   //default kendimiz verdik.
                }

                _unitOfWork.Villa.Add(obj);
                _unitOfWork.Save();
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

                //resim silme kismi - secilen dosyanin goruntusu kontrol edilir.
                if (obj.Image != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(obj.Image.FileName);    //. benzersiz tanımlayıcııdır(128 bitlik) tostring ile de dizeyye cevrilmistir guid . Rastgele oluşturulan bu numaralar genellikle tekil kimlikler oluşturmak, nesneleri takip etmek veya öğeleri benzersiz bir şekilde tanımlamak için kullanılır. 
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"images\VillaImage");    //webroot , root klasoru yolunu verir.

                    //guncelleme icin dosya secilmizse buraya girer.
                    if (!string.IsNullOrEmpty(obj.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\'));

                        //eski resim var ise bunu sil dedik
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    //goruntu eklemek icin dosya akisi olusturma
                    using var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create);
                    obj.Image.CopyTo(fileStream);

                    obj.ImageUrl = @"\images\VillaImage\" + fileName;
                }

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
                //guncelleme icin dosya secilmizse buraya girer.
                if (!string.IsNullOrEmpty(objFromDb.ImageUrl))
                {
                    var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, objFromDb.ImageUrl.TrimStart('\\'));

                    //resim var ise bunu sil dedik
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                _unitOfWork.Villa.Remove(objFromDb);
                _unitOfWork.Save();
                TempData["success"] = "The villa has been deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "The villa could not be deleted.";
            return View(obj);

        }

    }
}
