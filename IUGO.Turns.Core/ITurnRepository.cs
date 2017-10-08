using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IUGO.Turns.Core.Specifications.common;
using IUGO.Turns.Core.TurnAggreate;

namespace IUGO.Turns.Core
{
    public interface ITurnRepository
    {
        Task<Turn> FindTurn(Guid id);
        Task UpdateTurn(Guid id, Turn turn);
        Task<Turn> AddTurn(Turn turn);
        Task DeleteTurn(Turn turn);
        Task<IEnumerable<Turn>> ListAll();
        Task<IEnumerable<Turn>> ListBySpecification(ISpecification<Turn> specification);
    }
}
