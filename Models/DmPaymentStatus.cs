using System;
using System.Collections.Generic;

#nullable disable

namespace SperanzaPizzaApi.Models
{
    public partial class DmPaymentStatus
    {
        public DmPaymentStatus()
        {
            DmOrderPayments = new HashSet<DmOrderPayment>();
        }

        public int Id { get; set; }
        public string StatusName { get; set; }
        public string PolandName { get; set; }

        public virtual ICollection<DmOrderPayment> DmOrderPayments { get; set; }
    }
}
