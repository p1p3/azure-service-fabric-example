using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IUGO.Turns.Core;
using IUGO.Turns.Core.Specifications.common;
using IUGO.Turns.Infrastructure.Data.ServiceFabricStorage.DTOs;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using Turn = IUGO.Turns.Core.TurnAggreate.Turn;

namespace IUGO.Turns.Infrastructure.Data.ServiceFabricStorage
{
    public class TurnRepositoryReliableStateManager : ITurnRepository
    {
        private readonly IReliableDictionary<Guid, DTOs.Turn> _turnStorage;
        private readonly ITransaction _transaction;

        public TurnRepositoryReliableStateManager(IReliableDictionary<Guid, DTOs.Turn> turnStorage,
            ITransaction transaction)
        {
            this._turnStorage = turnStorage;
            this._transaction = transaction;
        }

        public async Task<Turn> FindTurn(Guid id)
        {
            var turn = (await _turnStorage.TryGetValueAsync(_transaction, id)).Value;
            return turn.MapToCore();
        }

        public async Task UpdateTurn(Guid id, Turn turn)
        {
            var oldTurn = (await this._turnStorage.TryGetValueAsync(this._transaction, id)).Value;
            var succeed = await _turnStorage.TryUpdateAsync(this._transaction, turn.Id, turn.MapToRepository(), oldTurn);
            if (!succeed) throw new Exception($"Something went wrong when trying to update the turn {id}");
        }

        public async Task<Turn> AddTurn(Turn turn)
        {
            var itemToStore = turn.MapToRepository();
            var succeed = await _turnStorage.TryAddAsync(this._transaction, itemToStore.Id, itemToStore);

            if (!succeed) throw new Exception("Something went wrong when trying to add the turn");
            return turn;
        }

        public Task DeleteTurn(Turn turn)
        {
            var id = turn.Id;
            return _turnStorage.TryRemoveAsync(this._transaction, id);
        }

        public Task<IEnumerable<Turn>> ListAll()
        {
            var allSpecification = new AllSpecification<Turn>();
            return this.ListBySpecification(allSpecification);
        }

        public async Task<IEnumerable<Turn>> ListBySpecification(ISpecification<Turn> specification)
        {
            var result = new List<Turn>();

            var allTurns = await _turnStorage.CreateEnumerableAsync(_transaction, EnumerationMode.Unordered);

            using (var enumerator = allTurns.GetAsyncEnumerator())
            {
                while (await enumerator.MoveNextAsync(CancellationToken.None))
                {
                    var current = enumerator.Current;
                    var turn = current.Value.MapToCore();
                    if (specification.IsSatisfiedBy(turn))
                        result.Add(turn);
                }
            }

            return result;
        }

    }
}
