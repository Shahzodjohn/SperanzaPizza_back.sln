using System.Collections.Generic;
using SperanzaPizzaApi.Data.DTO.Products;

namespace SperanzaPizzaApi.Data.DTO.Orders
{
    public class GetOrderItemResponse
    {
        public int orderId { get; set; }
        public string orderIdentifier { get; set; }
        public decimal productCost { get; set; }
        public decimal deliveryCost { get; set; }
        public bool hasDelivery { get; set; }
        public bool cashPayment { get; set; }
        public string streetname { get; set; }
        public string houseNumber { get; set; }
        public decimal? longitude { get; set; }
        public decimal? lattitude { get; set; }
        public List<OrderProductItem> products { get; set; }
        
    }
    
    public class OrderProductItem
    {
        public int Id { get; set; }
        public string productName { get; set; }
        public string categoryName { get; set; }
        public string sizeName { get; set; }
        public decimal priceValue { get; set; }
        public int productCount { get; set; }
    }
}