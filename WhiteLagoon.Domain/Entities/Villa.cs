using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhiteLagoon.Domain.Entities
{
    public class Villa
    {
        public int Id { get; set; }
        [MaxLength(50)] //bu veri aciklamalari otomatik uygulanir ve datta annotion olarak valide islemlerinde gecerlidir.
        public required string Name { get; set; }
        public string? Description { get; set; }
        [Display(Name = "Price per nigt")]
        [Range(10,10000)]
        public double Price { get; set; }
        public int Sqft { get; set; }
        [Range(1,10)]
        public int Occupancy { get; set; }

        //resim ,pdf dosyalari icin -  Bu özellik sayesinde kullanıcılar web uygulamasına dosya yükleyebilir ve bu dosyaları sunucuya iletebilir.
        [NotMapped] //vt ye eklemeyeyi onluyor, bu özellik veritabanında bir alan olarak depolanmaz veya oluşturulmaz.
        public IFormFile? Image { get; set; }   //dosya yuklemede kullanilacak
        //veri aciklamada kullaniliyor. gorunnmez 
        [Display(Name="Image Url")]
        public string? ImageUrl { get; set; }
        public DateTime? Created_Date { get; set; }
        public DateTime? Updated_Date { get; set; }

        //vt de bu sutunu olusturmasin dedik.
        [ValidateNever]
        public IEnumerable<Amenity> VillaAmenity { get; set; } //amenity de villalari listeletmek icin ihtiyac vardi

        //NotMapped niteliği, bir sınıfın veya özelliklerin veritabanı tablosunda bir alan oluşturulmasını engeller
        [NotMapped]
        public bool IsAvailable { get; set; } = true;

    }
}
