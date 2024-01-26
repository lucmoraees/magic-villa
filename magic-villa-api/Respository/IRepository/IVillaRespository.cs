using System;
using System.Linq.Expressions;
using magic_villa_api.Models;

namespace magic_villa_api.Respository.IRepository
{
	public interface IVillaRespository: IRepository<Villa>
    {
        Task<Villa> UpdateAsync(Villa entity);
    }
}

