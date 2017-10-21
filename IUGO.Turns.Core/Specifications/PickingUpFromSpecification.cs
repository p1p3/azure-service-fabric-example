using System.Collections.Generic;
using System.Linq;
using IUGO.Domain.Specifications;
using IUGO.Turns.Core.TurnAggreate;

namespace IUGO.Turns.Core.Specifications
{
    public class PickingUpFromSpecification : CompositeSpecification<Turn>
    {
        private readonly IEnumerable<string> _originsIds;

        public PickingUpFromSpecification(IEnumerable<string> originsIds)
        {
            _originsIds = originsIds;
        }

        public override bool IsSatisfiedBy(Turn o)
        {
            var hasMatch = o.OriginIds
                .Intersect(_originsIds)
                .Any();

            return hasMatch;
        }
    }
}