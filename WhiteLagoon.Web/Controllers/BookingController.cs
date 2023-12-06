using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Application.Common.Utility;
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


        //kesin rezerve edecek kisinin giris yamasi saglanmisitr.
        [Authorize]
        //bu metotta, kullanicinin secmis oldugu bilgiler gelir , anngi tarii kac geceyyi secmis ve secilen odanin id bilgisi geliyor.
        public IActionResult FinalizeBooking(int villaId, DateOnly checkInDate, int nights)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ApplicationUser user = _unitOfWork.User.Get(u => u.Id == userId);   //kullanıcı kimliği (userId) kullanılarak kullanıcı bilgilerini almak için bir yapı kullanılıyor. User repository'si üzerinden kullanıcı bilgilerine erişiliyor.

            Booking booking = new() //() , c#9 ile gelen bu kısaltma, sınıfın varsayılan constructor'ını çağırır.
            {
                VillaId = villaId,  //
                Villa = _unitOfWork.Villa.Get(u => u.Id == villaId, includeProporties: "VillaAmenity"),
                CheckInDate = checkInDate,
                Nights = nights,
                CheckOutDate = checkInDate.AddDays(nights),
                UserId = userId,
                Phone = user.PhoneNumber,
                Email = user.Email,
                Name = user.Name,
            };
            booking.TotalCost = booking.Villa.Price * nights;   //rezervasyondaki toplam maliyetin, rezervasyondaki villanin fiyyati * gecelik fiyatina esitlenmistir.
            return View(booking);
        }

        [Authorize]
        [HttpPost]
        //rezervasyon sonlandirma metotu
        public IActionResult FinalizeBooking(Booking booking)
        {
            var villa = _unitOfWork.Villa.Get(u=>u.Id == booking.VillaId);
            booking.TotalCost = villa.Price * booking.Nights;

            booking.Status = SD.StatusPending;  //bekleme duurmuna alindi
            booking.BookingDate = DateTime.Now; //rezervasyon tarihi alik olarak guncellendi

            _unitOfWork.Booking.Add(booking);   //vt ye rezervasyo eklendi.
            _unitOfWork.Save();
            return RedirectToAction(nameof(BookingConfirmation), new {bookingId = booking.Id});
        }


        //rezervasyon onay islemi
        [Authorize]
        public IActionResult BookingConfirmation(int bookingId) //rezervasyon kimligi - id si
        {
            return View(bookingId);
        }

    }
}
