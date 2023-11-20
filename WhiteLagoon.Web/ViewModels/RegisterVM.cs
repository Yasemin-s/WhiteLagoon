using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WhiteLagoon.Web.ViewModels
{
    public class RegisterVM
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password))] //sifrelerin ayni olup olmadigini kontrol edecek
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Name { get; set; }


        [Display(Name="Phone Number")]
        public string? PhoneNumber { get; set; }

        public string? RedirectUrl { get; set; }    //sadece oturum açmış kullanıcıların sahip olduğu sayfaya yönlendirme yapmak için

        public string? Role { get; set; }   //RoleList icin baglayici olarak kullanildi - secili degeri tutmak icin tanimalndi.

        [ValidateNever]
        public IEnumerable<SelectListItem> RoleList { get; set; }   //ilgili kismi role ozelligine baglayip rolelist i cairip dropdown islemi yaptik.

    }
}
