using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace community.Core.Interfaces
{
    public interface IManager<T> where T : class
    {
        Task<List<T>> Index();
        Task<T> Details(Guid? id);
        Task<T> Create(T entity);
        Task Edit(T entity);
        Task Delete(Guid? id);
    }
}
