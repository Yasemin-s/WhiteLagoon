using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Application.Common.Utility;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Infrastructure.Repository
{
    public class BookingRepository : Repository<Booking>, IBookingRepository
    {
        //veritabani baglami icin _db adinda nesne olusturulmus. bu nesne vt islemlerini gerceklestirecek.. veri tabanını direk biliyor çünkü .web projesine .infrasttructure ı referans verdik.
        private readonly ApplicationDbContext _db;

        //contructor icin kisa yol ctor tab tab
        public BookingRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Booking entity)
        {
            _db.Bookings.Update(entity);
        }

        //odeme icin gerekli arayuz , rezervasyon id ve durum bilgisi alindi.
        //public void UpdateStatus(int bookingId, string bookingStatus)
        //{
        //    //vt den rezervasyonun durumu alindi.
        //    var bookingFromDb = _db.Bookings.FirstOrDefault(m => m.Id == bookingId);
        //    if(bookingFromDb != null)
        //    {
        //        bookingFromDb.Status = bookingStatus;
        //        if(bookingStatus == SD.StatusCheckedIn)   //siparis durumu statik durumlara esitse, 
        //        {
        //            bookingFromDb.ActualCheckInDate = DateTime.Now;
        //        }
        //        if (bookingStatus == SD.StatusCompleted)  //chehckin - out tamamnlanmissa
        //        {
        //            bookingFromDb.ActualCheckInDate = DateTime.Now;
        //        }
        //    }
            
        //}

        //odeme icin gerekli arayuz
        //public void UpdateStripePaymentID(int bookingId, string sessionId, string paymentIntentId)
        //{
        //    var bookingFromDb = _db.Bookings.FirstOrDefault(m => m.Id == bookingId);    //vt deki eslesen rezervasyon id si aliniyor
        //    if (bookingFromDb != null)  //rez id bos degilse
        //    { 
        //        if(!string.IsNullOrEmpty(sessionId))    //oturum id si bos degilse
        //        {
        //            bookingFromDb.StripeSessionId = sessionId;  //oturum id sini Stripe e ata
        //        }
        //        if (!string.IsNullOrEmpty(paymentIntentId)) //odeme id si bos degilse
        //        {
        //            bookingFromDb.StripePaymentIntentId = paymentIntentId;  //odeme id sini Stripe ata
        //            bookingFromDb.PaymentDate = DateTime.Now;   //o an ki tarihi odeme tarihine ata
        //            bookingFromDb.IsPaymentSuccessful = true;   //odeme basarili kismini true yap
        //        }
        //    }
        //}
    }
}
