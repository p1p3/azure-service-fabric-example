namespace IUGO.Shippings.Infrastructure.Data.ServiceFabricStorage.DTOs
{
    public class ShippingTurn
    {

        public string TurnId { get; set; }
        public ShippingDriver Driver { get; set; }
        public ShippingVehicle Vehicle { get; set; }
    }
}