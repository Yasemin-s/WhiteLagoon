using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Infrastructure.Repository
{
    public class AmenityRepository : Repository<Amenity>, IAmenityRepository
    {
        //veritabani baglami icin _db adinda nesne olusturulmus. bu nesne vt islemlerini gerceklestirecek.. veri tabanını direk biliyor çünkü .web projesine .infrasttructure ı referans verdik.
        private readonly ApplicationDbContext _db;

        //contructor icin kisa yol ctor tab tab
        public AmenityRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Amenity entity)
        {
            _db.Amenities.Update(entity);
        }
    }
}
