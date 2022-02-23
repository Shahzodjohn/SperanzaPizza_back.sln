using System.Collections.Generic;

namespace SperanzaPizzaApi.Data.DTO.Payments
{
    public class InitPaymentOrderRequest
    {
        public string notifyUrl { get; set; }
        public string customerIp { get; set; }
        public string merchantPosId { get; set; }
        public string description { get; set; }
        public string currencyCode { get; set; }
        public int totalAmount { get; set; }
        public string extOrderId { get; set; }
        public string continueUrl { get; set; }
        public BuyerModel buyer { get; set; }
        public List<ProductItemModel> products { get; set; }
    }

    public class BuyerModel {
        public string phone { get; set; }        
        public string firstName { get; set; }
        public string language { get; set; }
    }
    public class ProductItemModel {
        public string name { get; set; }
        public string unitPrice { get; set; }
        public string quantity { get; set; }
    }
}