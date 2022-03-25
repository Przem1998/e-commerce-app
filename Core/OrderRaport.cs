namespace Core
{
    public class OrderRaport
    {
        public int AmountOfOrders { get; set; }
        public string OrdersValue { get; set; }
        public int AmountOfCompletedOrders { get; set; }
        public int AmountOfPendingOrders { get; set; }
        public int AmountOfCanceledOrders { get; set; }
    }
}