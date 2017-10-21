using System;

namespace IUGO.Domain
{
    public interface IAggregate
    {
        Guid Id { get; }
    }
}