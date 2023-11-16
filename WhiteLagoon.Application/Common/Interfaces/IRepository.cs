using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Application.Common.Interfaces
{
    //T nin eski hali "Villa" di, bu sekilde "Villa" sinifi da "VillaNumber" sinifi da bu metotlari kullanabilcek durum getirilmiş oldu.
    public interface IRepository<T> where T : class
    {
        //villacontrollerdaki tum villayi almak icin gerekli kod satiridir.
        //villa sinifi domain altinda oldugu icin , referans almak gereklidir.(villa usttune gel ve (ctrl + . ))
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProporties = null);
        T Get(Expression<Func<T, bool>> filter, string? includeProporties = null);
        void Add(T entity);
        bool Any(Expression<Func<T, bool>> filter);
        void Remove(T entity);
        //update kullanmadik cunku her alani guncellemek istemeyebiliriz.
    }
}
