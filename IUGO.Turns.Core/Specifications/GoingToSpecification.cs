using System.Collections.Generic;
using System.Linq;
using IUGO.Domain.Specifications;
using IUGO.Turns.Core.TurnAggreate;

namespace IUGO.Turns.Core.Specifications
{
    public class GoingToSpecification : CompositeSpecification<Turn>
    {
        private readonly IEnumerable<string> _destinationIds;

        public GoingToSpecification(IEnumerable<string> destinationIds)
        {
            _destinationIds = destinationIds;
        }

        public override bool IsSatisfiedBy(Turn o)
        {
            var hasMatch = o.DestiniationIds
                .Intersect(_destinationIds)
                .Any();

            return hasMatch;
        }
    }
}
