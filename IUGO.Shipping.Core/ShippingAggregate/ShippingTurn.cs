namespace IUGO.Shippings.Core.ShippingAggregate
{
    public class ShippingTurn
    {
        public ShippingTurn(string turnId, ShippingDriver driver, ShippingVehicle vehicle)
        {
            TurnId = turnId;
            Driver = driver;
            Vehicle = vehicle;
        }

        public static ShippingTurn CreateShippingTurn(string turnId, ShippingDriver driver, ShippingVehicle vehicle)
        {
            return new ShippingTurn(turnId,driver,vehicle);
        }

        public string TurnId { get; }
        public ShippingDriver Driver { get; }
        public ShippingVehicle Vehicle { get; }
    }

    public class NullShippingTurn : ShippingTurn
    {
        public NullShippingTurn() : base("", new NullShippingDriver(), new NullShippingVehicle())
        {

        }
    }
}
