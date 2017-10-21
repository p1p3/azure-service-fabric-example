using IUGO.Domain.Specifications;
using IUGO.Turns.Core.TurnAggreate;

namespace IUGO.Turns.Core.Specifications
{
    public class VehicleSpecification : CompositeSpecification<Turn>
    {
        private readonly string _vehicleId;

        public VehicleSpecification(string vehicleId)
        {
            _vehicleId = vehicleId;
        }

        public override bool IsSatisfiedBy(Turn o)
        {
            return o.VehicleId == _vehicleId;
        }
    }
}
