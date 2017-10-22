namespace IUGO.Shippings.Core.ShippingAggregate
{
    public class ShippingVehicle
    {
        protected ShippingVehicle(string designationId, string id)
        {
            DesignationId = designationId;
            Id = id;
        }

        public static ShippingVehicle  CreateShippingVehicle(string designationId, string id)
        {
            return new ShippingVehicle(designationId,id);
        }

        public string Id { get; }
        public string DesignationId { get; }

    }

    public class NullShippingVehicle : ShippingVehicle
    {
        public NullShippingVehicle() : base("", "")
        {
        }
    }
}
