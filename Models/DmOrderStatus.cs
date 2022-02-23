using System;
using System.Collections.Generic;

#nullable disable

namespace SperanzaPizzaApi.Models
{
    public partial class DmOrderStatus
    {
        public DmOrderStatus()
        {
            DmOrders = new HashSet<DmOrder>();
        }

        public int Id { get; set; }
        public string StatusName { get; set; }
        public string SystemName { get; set; }

        public virtual ICollection<DmOrder> DmOrders { get; set; }
    }
}
