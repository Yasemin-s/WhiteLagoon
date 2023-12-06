using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhiteLagoon.Application.Common.Interfaces
{
    //burada tanimlananalar IUnitOfWork u kullanarak vt islemlerini gerceklestirebilecek.
    public interface IUnitOfWork
    {
        IVillaRepository Villa {  get; }
        IVillaNumberRepository VillaNumber {  get; }
        IAmenityRepository Amenity {  get; }
        IApplicationUserRepository User { get; }
        IBookingRepository Booking { get; }
        void Save();
    }
}
//bu sarmalayıcı icinde diger reposityoryler olacak.