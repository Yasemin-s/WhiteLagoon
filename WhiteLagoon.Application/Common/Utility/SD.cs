using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhiteLagoon.Application.Common.Utility
{
    //static details(sd) - rolleri burada tanimlamak istedik
    public static class SD
    {
        public const string Role_Customer = "Customer";
        public const string Role_Admin = "Admin";

        public const string StatusPending = "Pending";  //vt de kayit olusturup siparisi beklemeye almakicin
        public const string StatusApproved = "Approved";    //kullanici basarili odeme yaptiginda rezervasyon olusturuldu seklinde olacaktir
        public const string StatusCheckedIn = "CheckedIn";  //checin yaptiginda durum guncellenecektir.
        public const string StatusCompleted = "Completed";  //kontrol edildiginde tamamlandi oalcaktir.
        public const string StatusCancelled = "Cancelled";  //odeme yaparken vazgecip iptal durumu
        public const string StatusRefunded = "Refunded";    //odeme iade durumu

    }
}
