using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//rezervasyon icin kullanilan modeldir.
namespace WhiteLagoon.Domain.Entities
{
    //rezervasyon modeli
    public class Booking
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }  //Rezervasyonu yapan kullanıcının kimlik numarasını tutar 

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }   //UserId ile User arasında bir ilişkiyi temsil eder

        [Required]
        public int VillaId { get; set; }    //Rezerve edilen villanın kimlik numarasını tutar 

        [ForeignKey("VillaId")]
        public Villa Villa { get; set; }    //VillaId ile Villa arasında bir ilişkiyi temsil eder

        [Required]
        public string Name { get; set; }    //Rezervasyon yapan kişinin adını tutar

        [Required]
        public string Email { get; set; }
        public string? Phone { get; set; }

        [Required]
        public double TotalCost { get; set; }   //Rezervasyonun toplam maliyetini temsil eder
        public int Nights { get; set; } // Rezervasyonun kaç gece süreceğini temsil eder
        public string? Status { get; set; } //Rezervasyonun durumunu temsil eder 

        [Required]
        public DateTime BookingDate { get; set; }   //Rezervasyonun yapıldığı tarihi temsil eder 

        [Required]
        public DateOnly CheckInDate { get; set; }   // Rezervasyonun başlama tarihini temsil eder 

        [Required]
        public DateOnly CheckOutDate { get; set; }  //Rezervasyonun bitiş tarihini temsil eder 

        public bool IsPaymentSuccessful { get; set; } = false;  // Ödemenin başarılı olup olmadığını belirten bir boolean değerdir
        public DateTime PaymentDate { get; set; }   //Ödeme tarihini temsil eder

        public string? StripeSessionId { get; set; }    //Stripe ödeme işlemi için kullanılan oturum ve işlem kimliklerini temsil eder
        public string? StripePaymentIntentId { get; set; }  //

        public DateTime ActualCheckInDate { get; set; } //Gerçekleşen giriş ve çıkış tarihlerini temsil eder
        public DateTime ActualCheckOutDate { get; set; }

        public int VillaNumber { get; set; }    // Bir villa numarasını temsil eder

        [NotMapped]
        public List<VillaNumber> VillaNumbers { get; set; } // rezervasyonun birden fazla villa numarasını tutan bir listeyi temsil eder.
    }
}
