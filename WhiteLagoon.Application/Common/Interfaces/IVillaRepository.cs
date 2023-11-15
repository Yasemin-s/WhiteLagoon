using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Domain.Entities;  //domain referansi , villa sinifi icin

namespace WhiteLagoon.Application.Common.Interfaces
{
    public interface IVillaRepository
    {
        //villacontrollerdaki tum villayi almak icin gerekli kod satiridir.
        //villa sinifi domain altinda oldugu icin , referans almak gereklidir.(villa usttune gel ve (ctrl + . ))
        IEnumerable<Villa> GetAll(Expression<Func<Villa,bool>>? filter = null, string? includeProporties = null);
        Villa Get(Expression<Func<Villa,bool>> filter, string? includeProporties = null);
        void Add(Villa entity);
        void Update(Villa entity);
        void Remove(Villa entity);
        void Save();
    }
}
