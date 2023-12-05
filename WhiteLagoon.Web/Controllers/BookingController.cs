using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Web.Controllers
{
    public class BookingController : Controller
    {
        //unitofwork u inject ettik bu sekilde vt islemlerini gerceklestirecez.
        private readonly IUnitOfWork _unitOfWork;
        public BookingController(IUnitOfWork unitOfWork)    //IUnitOfWork bağımlılığını enjekte etmek için kullanılır.
        {
            _unitOfWork = unitOfWork;
        }


        //bu metotta, kullanicinin secmis oldugu bilgiler gelir , anngi tarii kac geceyyi secmis ve secilen odanin id bilgisi geliyor.
        public IActionResult FinalizeBooking(int villaId, DateOnly checkInDate, int nights)
        {
            Booking booking = new() //() , c#9 ile gelen bu kısaltma, sınıfın varsayılan constructor'ını çağırır.
            {
                VillaId = villaId,  //
                Villa = _unitOfWork.Villa.Get(u=>u.Id==villaId, includeProporties:"VillaAmenity"),
                CheckInDate = checkInDate,
                Nights = nights,
                CheckOutDate = checkInDate.AddDays(nights),
            };
            booking.TotalCost = booking.Villa.Price * nights;   //rezervasyondaki toplam maliyetin, rezervasyondaki villanin fiyyati * gecelik fiyatina esitlenmistir.
            return View(booking);
        }
    }
}
