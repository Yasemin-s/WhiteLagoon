using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Web.Models;
using WhiteLagoon.Web.ViewModels;

namespace WhiteLagoon.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            //ana sayyfada bu gorunuyor. HomeVM in index i
            HomeVM homeVM = new()
            {
                VillaList = _unitOfWork.Villa.GetAll(includeProporties: "VillaAmenity"), //villalarin listelenmesi icin
                Nights = 1,    //default olarak 1 gece olsun dedik.
                CheckInDate = DateOnly.FromDateTime(DateTime.Now),


            };
            return View(homeVM);
        }

        //metot, villa ogelerindeki id nin tek mi cift mi oldugunu kontrol ediyor.
        [HttpPost]
        public IActionResult Index(HomeVM homeVM)
        {
            homeVM.VillaList = _unitOfWork.Villa.GetAll(includeProporties: "VillaAmenity"); //villalarin listelenmesi icin
            return View(homeVM);
        }


        //tarih araligina gore villa listesini filtreleyip getiriyor.
        public IActionResult GetVillasByDate(int nights, DateOnly checkInDate)
        {
            var villaList = _unitOfWork.Villa.GetAll(includeProporties: "VillaAmenity").ToList();   //getall vt den tum villalari getirioyr.includeProporties ise, parametre ile iliskili olan villamaenity ozelligini dahhil ediyor.
            foreach (var villa in villaList)
            {
                if (villa.Id % 2 == 0)
                {
                    villa.IsAvailable = false;
                }
            }
            //model oluturularak , metodun " sonraki adýmda donecegi view e aktarilacak veileri temsil ediyor.
            HomeVM homeVM = new()
            {
                CheckInDate = checkInDate,
                VillaList = villaList,
                Nights = nights
            };
            return PartialView("_VillaList",homeVM); //sadece parcali gorunumun yenilenmesi saglanmistir.
        }

        public IActionResult Privacy()
        {
            return View();
        }


        public IActionResult Error()
        {
            return View();
        }
    }
}
