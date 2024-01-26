using System;
using magic_villa_api.Models;

namespace magic_villa_api.Respository.IRepository
{
    public interface IVillaNumberRespository : IRepository<VillaNumber>
    {
        Task<VillaNumber> UpdateAsync(VillaNumber entity);
    }
}

