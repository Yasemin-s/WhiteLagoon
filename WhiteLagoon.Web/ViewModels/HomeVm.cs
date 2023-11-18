using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Web.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Villa>? VillaList{ get; set; }
        public DateOnly CheckInDate { get; set; }   //dateonly, sadece tarih bilgisini gunay yil olarak alir ve tutar. datetime bu tarilerin yanında saat dk sn de alir.
        public DateOnly CheckOutDate { get; set; }
        public int Nights { get; set; }
    }
}
