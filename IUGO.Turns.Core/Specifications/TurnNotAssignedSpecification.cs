using IUGO.Domain.Specifications;
using IUGO.Turns.Core.TurnAggreate;

namespace IUGO.Turns.Core.Specifications
{
    public class TurnNotAssignedSpecification : CompositeSpecification<Turn>
    {
        public override bool IsSatisfiedBy(Turn o)
        {
            return !o.IsTurnAssigned;
        }
    }
}
