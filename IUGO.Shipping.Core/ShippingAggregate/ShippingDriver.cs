using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IUGO.Shippings.Core.ShippingAggregate
{
    public class ShippingDriver
    {
        protected ShippingDriver(string id, string contactNumber, string fullName)
        {
            Id = id;
            ContactNumber = contactNumber;
            FullName = fullName;
        }

        public static ShippingDriver CreateShippingDriver(string id, string contactNumber, string fullName)
        {
            return  new ShippingDriver(id,contactNumber,fullName);
        }

        public string Id { get; }
        public string ContactNumber { get; }
        public string FullName { get; }
    }

    public class NullShippingDriver : ShippingDriver
    {
        public NullShippingDriver() : base("", "", "")
        {
        }
    }


}
