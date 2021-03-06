namespace SperanzaPizzaApi.Data.DTO.Payments
{
    public class SavePaymentRequest
    {
        public Order order { get; set;  }
    }

    public class Order
    {
        public string orderId { get; set; }
        public string extOrderId { get; set; }
        
        public string merchantPosId { get; set; }
        public string status { get; set; }
    }
    /* пример запроса от PayU с информацией о заказе
    "order":{
        "orderId":"LDLW5N7MF4140324GUEST000P01",
        "extOrderId":"Order id in your shop",
        "orderCreateDate":"2012-12-31T12:00:00",
        "notifyUrl":"http://tempuri.org/notify",
        "customerIp":"127.0.0.1",
        "merchantPosId":"{POS ID (pos_id)}",
        "description":"My order description",
        "currencyCode":"PLN",
        "totalAmount":"200",
        "buyer":{
            "email":"john.doe@example.org",
            "phone":"111111111",
            "firstName":"John",
            "lastName":"Doe",
            "language":"en"
        },
        "payMethod": {
            "type": "PBL" //or "CARD_TOKEN", "INSTALLMENTS"
        },
        "products":[
            {
                "name":"Product 1",
                "unitPrice":"200",
                "quantity":"1"
            }
        ],
        "status":"COMPLETED"
        },
    "localReceiptDateTime": "2016-03-02T12:58:14.828+01:00",
    "properties": [
            {
                "name": "PAYMENT_ID",
                "value": "151471228"
            }
        ]
    }       
*/
}