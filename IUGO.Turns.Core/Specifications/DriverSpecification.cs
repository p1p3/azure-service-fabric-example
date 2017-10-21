using IUGO.Domain.Specifications;
using IUGO.Turns.Core.TurnAggreate;

namespace IUGO.Turns.Core.Specifications
{
    public class DriverSpecification : CompositeSpecification<Turn>
    {
        private readonly string _driverId;

        public DriverSpecification(string driverId)
        {
            _driverId = driverId;
        }

        public override bool IsSatisfiedBy(Turn o)
        {
            return o.DriverId == _driverId;
        }
    }
}
