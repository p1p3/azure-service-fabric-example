using System;
using IUGO.Domain.Specifications;
using IUGO.Turns.Core.TurnAggreate;

namespace IUGO.Turns.Core.Specifications
{
    public class PickUpDateSpecification : CompositeSpecification<Turn>
    {
        private readonly DateTime _pickUpdate;

        public PickUpDateSpecification(DateTime pickUpdate)
        {
            _pickUpdate = pickUpdate;
        }

        public override bool IsSatisfiedBy(Turn o)
        {
            return o.AvailableFrom <= _pickUpdate;
        }
    }
}