namespace SperanzaPizzaApi.Data.DTO.Orders
{
    public class CreateOrderProductsParams
    {
        public int productId { get; set; }
        public int sizeId { get; set; }
        public int count { get; set; }
    }
}