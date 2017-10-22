using System;
using System.Threading.Tasks;
using IUGO.Shippings.Core;

namespace IUGO.Shippings.Infrastructure.Data
{
    public interface IUnitOfWork : IDisposable
    {
        Task<IShippingRepository> ShippingsRepository { get; }
        Task Commit();
    }
}
