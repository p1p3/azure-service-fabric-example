using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IUGO.Shippings.Core.ShippingAggregate
{
    public enum ShippingStates
    {
        Created = 0,
        Assigned = 1,
        PickedUp = 2,
        Delivired = 3,
    }
}
