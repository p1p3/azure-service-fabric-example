using System;
using System.Threading.Tasks;
using IUGO.Shipping.Core;

namespace IUGO.Shipping.Infrastructure.Data
{
    public interface IUnitOfWork : IDisposable
    {
        Task<IShippingRepository> ShippingsRepository { get; }
        Task Commit();
    }
}
