using System;
using System.Collections.Generic;

#nullable disable

namespace SperanzaPizzaApi.Models
{
    public partial class DmDeliveryRoute
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int AddressId { get; set; }
        public string Flat { get; set; }
        public string GateCode { get; set; }

        public virtual DmAddress Address { get; set; }
        public virtual DmOrder Order { get; set; }
    }
}
