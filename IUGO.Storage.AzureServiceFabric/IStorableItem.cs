
using System;

namespace IUGO.Storage.AzureServiceFabric
{
    public interface IStorableItem
    {
        Guid Id { get; }
    }
}
