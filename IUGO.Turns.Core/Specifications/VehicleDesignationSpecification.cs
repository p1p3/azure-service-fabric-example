using System.Collections.Generic;
using System.Linq;
using IUGO.Domain.Specifications;
using IUGO.Turns.Core.TurnAggreate;

namespace IUGO.Turns.Core.Specifications
{
    public class VehicleDesignationSpecification : CompositeSpecification<Turn>
    {
        private readonly IEnumerable<string> _vehicleDesignationIds;

        public VehicleDesignationSpecification(IEnumerable<string> vehicleDesignationIds)
        {
            _vehicleDesignationIds = vehicleDesignationIds;
        }

        public override bool IsSatisfiedBy(Turn o)
        {
            return _vehicleDesignationIds.Contains(o.VehicleDesignationId);
        }
    }
}