using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IUGO.Domain.Specifications;

namespace IUGO.Domain
{
    public interface IGenericRepository<T> where T : IAggregate
    {
        Task<T> Find(Guid id);
        Task Update(Guid id, T turn);
        Task<T> Add(T turn);
        Task Delete(T turn);
        Task<IEnumerable<T>> ListAll();
        Task<IEnumerable<T>> ListBySpecification(ISpecification<T> specification);
    }
}
