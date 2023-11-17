using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;
using WhiteLagoon.Infrastructure.Repository;
using WhiteLagoon.Web.ViewModels;  //veri tabani icin

//villa detaylarını goruntuleme , silme , guncelleme islemleri icin olusturulan controller dir.
namespace WhiteLagoon.Web.Controllers
{
    public class AmenityController : Controller
    {
        //veritabani baglami icin _db adinda nesne olusturulmus. bu nesne vt islemlerini gerceklestirecek.. veri tabanını direk biliyor çünkü .web projesine .infrasttructure ı referans verdik.
        private readonly IUnitOfWork _unitOfWork;
        //contructor icin kisa yol ctor tab tab
        public AmenityController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var amenities = _unitOfWork.Amenity.GetAll(includeProporties: "Villa");
            return View(amenities);  //gorunume git dersen, 3 tane seyin donduruldugunu gorursun.
        }

        //villa olusturma kismi - get kismi
        public IActionResult Create()
        {
            AmenityVM amenityVM = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                })
            };
            return View(amenityVM);

        }

        //post kismi
        [HttpPost]
        public IActionResult Create(AmenityVM obj)
        {
           if (ModelState.IsValid) 
            {
                _unitOfWork.Amenity.Add(obj.Amenity);  //artik _db.Add() seklinde kullanabiirin, aldigi nesneye gore referans alacak.
                _unitOfWork.Save();
                TempData["success"] = "The amenity has been created successfully.";
                return RedirectToAction(nameof(Index));
            }
            //dropdown menuyu tekrar doldurmayi sagliyor
            obj.VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
             return View(obj); 

        }

        //id ye gore guncelleme apmak icin View e gonderiyor.
        public IActionResult Update(int amenityId)
        {
            AmenityVM amenityVM = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Amenity = _unitOfWork.Amenity.Get(u => u.Id == amenityId)
            };
            if(amenityVM.Amenity == null)
            {
                return RedirectToAction("Error","Home");
            }
            return View(amenityVM);
        }


        //post kismi - data endpoints
        [HttpPost]
        public IActionResult Update(AmenityVM amenityVM)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Amenity.Update(amenityVM.Amenity);
                _unitOfWork.Save();
                 TempData["success"] = "The amenity has been updated successfully";
                return RedirectToAction(nameof(Index));
            }
            amenityVM.VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            return View(amenityVM);
        }


        //silme kismi
        public IActionResult Delete(int amenityId)
        {
            AmenityVM amenityVM = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Amenity = _unitOfWork.Amenity.Get(u => u.Id == amenityId)
            };
            if (amenityVM.Amenity == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(amenityVM);
        }


        [HttpPost]
        public IActionResult Delete(AmenityVM amenityVM)
        {
            Amenity? objFromDb = _unitOfWork.Amenity
                .Get(u => u.Id == amenityVM.Amenity.Id);
            if (objFromDb is not null)
            {
                _unitOfWork.Amenity.Remove(objFromDb);
                _unitOfWork.Save();
                TempData["success"] = "The amenity has been deleted successfully.";
                return RedirectToAction(nameof(Index));  //burada "Indexa" seklinde yazim yanlisi yapilirsa hata mesai vermez. bu sekilde kullanirsam hata mesai gosterir.
            }
            TempData["error"] = "The amenity could not be deleted.";
            return View();

        }

    }
}
