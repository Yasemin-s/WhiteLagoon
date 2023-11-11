using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        //veri aciklamada kullaniliyor. gorunnmez 
        [Display(Name="Image Url")]
        public string? ImageUrl { get; set; }
        public DateTime? Created_Date { get; set; }
        public DateTime? Updated_Date { get; set; }

    }
}
