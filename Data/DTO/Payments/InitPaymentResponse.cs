namespace SperanzaPizzaApi.Data.DTO.Payments
{
    public class InitPaymentOrderResponse
    {
        public string orderId { get; set; }
        public string extOrderId { get; set; }
        public Status status { get; set;}
        public string redirectUri { get; set; }
    }

    public class Status
    {
        public string statusCode { get; set; }
    }
}