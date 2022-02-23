using System;
using System.Collections.Generic;

namespace SperanzaPizzaApi.Data.DTO.Orders
{
    public class CreateNewOrderRequest
    {
        public List<CreateOrderProductsParams> products {get; set;}
        public string clientName {get; set;}
        public string clientPhone {get; set;}
        public string clientComment {get; set;}
        public decimal? productCost {get; set;}
        public decimal? deliveryCost {get; set;}
        public bool deliver {get; set;}
        public int? addressId {get; set;}
        public string postcode {get; set;}
        public string flatNumber {get; set;}
        public string gateCode {get; set;}
        public bool? asSoonasPossible {get; set;}
        public string cookingTime {get; set;}
        public bool hasInvoice {get; set;}
        public string clientCompanyName {get; set;}
        public string nip {get; set;}
        public bool cashPayment {get; set;}
        public string employeeComment {get; set;}
    }
}