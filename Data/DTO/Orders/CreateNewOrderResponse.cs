namespace SperanzaPizzaApi.Data.DTO.Orders
{
    public class CreateNewOrderResponse
    {
        public  int orderId { get; set; }
        public string orderIdentifier { get; set; }
        public bool isCashPayment { get; set; }
        public bool isDelivery { get; set; }
        public string paymentRedirectUrl { get; set; }
    }
}