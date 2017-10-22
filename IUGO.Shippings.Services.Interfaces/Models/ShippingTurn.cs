namespace IUGO.Shippings.Services.Interfaces.Models
{
    public class ShippingTurn
    {

        public string TurnId { get; set; }
        public ShippingDriver Driver { get; set; }
        public ShippingVehicle Vehicle { get; set; }
    }
}