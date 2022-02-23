using System;
using System.Collections.Generic;
using SperanzaPizzaApi.Data.DTO.Products;
using Stripe;

namespace SperanzaPizzaApi.Data.DTO.Orders
{
    public class GetOrderResponse
    {
        public int orderId { get; set; }
        public int orderStatusId { get; set; }
        public string orderIdentifier { get; set; }
        public DateTime createdDate { get; set; }
        public string clientComment { get; set; }
        public bool? clientCommentIsRead { get; set; }
        public bool hasDelivery { get; set; }
        public List<OrderProductItem> products { get; set; }
        
        
    }
    
    
}