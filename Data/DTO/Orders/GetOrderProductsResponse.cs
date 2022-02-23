namespace SperanzaPizzaApi.Data.DTO.Orders
{
    public class GetOrderProductsResponse
    {
        public int Id { get; set; }
        public string categoryName { get; set; }
        public string productName { get; set; }
        public string sizeName { get; set; }
        public decimal priceValue { get; set; }
        public int productCount { get; set; }
    }
}