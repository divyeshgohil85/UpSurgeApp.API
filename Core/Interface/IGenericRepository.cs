using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<List<T>> ListAllAsync();
        Task<T> GetByIdAsync(Guid id);
    }
}
