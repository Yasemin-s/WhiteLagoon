﻿using Microsoft.EntityFrameworkCore;
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
    public class VillaRepository : IVillaRepository
    {
        //veritabani baglami icin _db adinda nesne olusturulmus. bu nesne vt islemlerini gerceklestirecek.. veri tabanını direk biliyor çünkü .web projesine .infrasttructure ı referans verdik.
        private readonly ApplicationDbContext _db;

        //contructor icin kisa yol ctor tab tab
        public VillaRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public void Add(Villa entity)
        {
            _db.Add(entity);
        }

        public Villa Get(Expression<Func<Villa, bool>> filter, string? includeProporties = null)
        {
            IQueryable<Villa> query = _db.Set<Villa>();
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (!string.IsNullOrEmpty(includeProporties))
            {
                //Villa, VillaNumber -- case sensitive
                foreach (var includeProp in includeProporties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.FirstOrDefault();
        }

        public IEnumerable<Villa> GetAll(Expression<Func<Villa, bool>>? filter = null, string? includeProporties = null)
        {
            IQueryable<Villa> query = _db.Set<Villa>();
            if(filter != null)
            {
                query = query.Where(filter);
            }
            if (!string.IsNullOrEmpty(includeProporties))
            {
                //Villa, VillaNumber -- case sensitive
                foreach (var includeProp in includeProporties
                    .Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.ToList();
        }

        public void Remove(Villa entity)
        {
          _db.Remove(entity);
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(Villa entity)
        {
            _db.Update(entity);
        }
    }
}
