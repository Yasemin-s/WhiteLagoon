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
