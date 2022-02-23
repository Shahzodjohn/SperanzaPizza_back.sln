namespace SperanzaPizzaApi.Data.DTO.Payments
{
    public class GetPaymentStatusResponse
    {
        public int orderId { get; set; }
        public string paymentStatus { get; set; }
    }
}