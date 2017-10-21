using IUGO.Domain;

namespace IUGO.Storage.AzureServiceFabric
{
    public interface IStorableEntityMapper<T, TI> where T : IAggregate
        where TI : IStorableItem
    {
        TI MapToStorable(T entity);
        T MapToCore(TI entity);
    }
}

