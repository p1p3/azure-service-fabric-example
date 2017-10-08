using System;
using System.Threading.Tasks;
using IUGO.Turns.Core;
using IUGO.Turns.Core.TurnAggreate;

namespace IUGO.Turns.Infrastructure.Data
{
    public interface IUnitOfWork : IDisposable
    {
        Task<ITurnRepository> TurnsRepository { get; }
        Task Commit();
    }
}
