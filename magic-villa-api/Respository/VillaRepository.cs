using System;
using System.Linq.Expressions;
using magic_villa_api.Models;
using magic_villa_api.Respository.IRepository;
using magic_villa_api.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace magic_villa_api.Respository
{
    public class VillaRepository: Repository<Villa>, IVillaRespository
    {
        private readonly ApplicationDbContext _db;

        public VillaRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }

        public async Task<Villa> UpdateAsync(Villa entity)
        {
            entity.UpdatedDate = DateTime.Now;
            _db.Villas.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}

