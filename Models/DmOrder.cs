using System;
using System.Collections.Generic;

#nullable disable

namespace SperanzaPizzaApi.Models
{
    public partial class DmOrder
    {
        public DmOrder()
        {
            DmDeliveryRoutes = new HashSet<DmDeliveryRoute>();
            DmOrderPayments = new HashSet<DmOrderPayment>();
            DmOrderProducts = new HashSet<DmOrderProduct>();
        }

        public int Id { get; set; }
        public int OrderStatusId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ClosedDate { get; set; }
        public string ClientName { get; set; }
        public string ClientPhone { get; set; }
        public string ClientComment { get; set; }
        public string OrderIdentifier { get; set; }
        public decimal ProductCost { get; set; }
        public bool HasDelivery { get; set; }
        public decimal DeliveryCost { get; set; }
        public bool AsSoonAsPossible { get; set; }
        public TimeSpan? СookingTime { get; set; }
        public bool? HasInvoice { get; set; }
        public string ClientCompanyName { get; set; }
        public string Nip { get; set; }
        public bool CashPayment { get; set; }
        public bool? ClientCommentIsRead { get; set; }
        public string EmployeeComment { get; set; }

        public virtual DmOrderStatus OrderStatus { get; set; }
        public virtual ICollection<DmDeliveryRoute> DmDeliveryRoutes { get; set; }
        public virtual ICollection<DmOrderPayment> DmOrderPayments { get; set; }
        public virtual ICollection<DmOrderProduct> DmOrderProducts { get; set; }
    }
}
