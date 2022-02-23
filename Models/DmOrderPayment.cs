using System;
using System.Collections.Generic;

#nullable disable

namespace SperanzaPizzaApi.Models
{
    public partial class DmOrderPayment
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ExtOrderId { get; set; }
        public int? StatusId { get; set; }

        public virtual DmOrder Order { get; set; }
        public virtual DmPaymentStatus Status { get; set; }
    }
}
