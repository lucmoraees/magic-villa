using System;
using System.Linq.Expressions;
using magic_villa_api.Models;
using magic_villa_api.Respository.IRepository;
using magic_villa_api.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace magic_villa_api.Respository
{
    public class VillaNumberRepository : Repository<VillaNumber>, IVillaNumberRespository
    {
        private readonly ApplicationDbContext _db;

        public VillaNumberRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<VillaNumber> UpdateAsync(VillaNumber entity)
        {
            entity.UpdatedDate = DateTime.Now;
            _db.VillaNumbers.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}

