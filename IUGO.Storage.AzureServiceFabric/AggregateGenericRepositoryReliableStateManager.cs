using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IUGO.Domain;
using IUGO.Domain.Specifications;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;

namespace IUGO.Storage.AzureServiceFabric
{
    public class AggregateGenericRepositoryReliableStateManager<T,TI> : IGenericRepository<T> where T : IAggregate 
        where TI : IStorableItem
    {
        private readonly IReliableDictionary<Guid, TI> _aggreateStorage;
        private readonly ITransaction _transaction;
        private readonly IStorableEntityMapper<T, TI> _mapper;

        public AggregateGenericRepositoryReliableStateManager(IReliableDictionary<Guid, TI> aggreateStorage,
            ITransaction transaction, IStorableEntityMapper<T, TI> mapper)
        {
            this._aggreateStorage = aggreateStorage;
            this._transaction = transaction;
            this._mapper = mapper;
        }

        public async Task<T> Find(Guid id)
        {
            var entity = (await _aggreateStorage.TryGetValueAsync(_transaction, id)).Value;
            var coreEntity = _mapper.MapToCore(entity);
            return coreEntity;
        }

        public async Task Update(Guid id, T entity)
        {
            var oldEntityt = (await this._aggreateStorage.TryGetValueAsync(this._transaction, id)).Value;
            var storableEntity = _mapper.MapToStorable(entity);

            var succeed = await _aggreateStorage.TryUpdateAsync(this._transaction, entity.Id, storableEntity, oldEntityt);
            if (!succeed) throw new Exception($"Something went wrong when trying to update the entity {id}");
        }

        public async Task<T> Add(T entity)
        {
            var storableEntity = _mapper.MapToStorable(entity);
            var succeed = await _aggreateStorage.TryAddAsync(this._transaction, entity.Id, storableEntity);

            if (!succeed) throw new Exception("Something went wrong when trying to add the turn");
            return entity;
        }

        public Task Delete(T entity)
        {
            var id = entity.Id;
            return _aggreateStorage.TryRemoveAsync(this._transaction, id);
        }
        
        public Task<IEnumerable<T>> ListAll()
        {
            var allSpecification = new AllSpecification<T>();
            return this.ListBySpecification(allSpecification);
        }

        public async Task<IEnumerable<T>> ListBySpecification(ISpecification<T> specification)
        {
            var result = new List<T>();

            var allTurns = await _aggreateStorage.CreateEnumerableAsync(_transaction, EnumerationMode.Unordered);

            using (var enumerator = allTurns.GetAsyncEnumerator())
            {
                while (await enumerator.MoveNextAsync(CancellationToken.None))
                {
                    var current = enumerator.Current;
                    var entity = current.Value;
                    var coreEntity = _mapper.MapToCore(entity);
                    if (specification.IsSatisfiedBy(coreEntity))
                        result.Add(coreEntity);
                }
            }

            return result;
        }

    }
}
