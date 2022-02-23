using System;
using System.Collections.Generic;

#nullable disable

namespace SperanzaPizzaApi.Models
{
    public partial class DmAddress
    {
        public DmAddress()
        {
            DmDeliveryRoutes = new HashSet<DmDeliveryRoute>();
        }

        public int Id { get; set; }
        public int StreetId { get; set; }
        public string HouseNumber { get; set; }
        public string PostCode { get; set; }
        public decimal Longitude { get; set; }
        public decimal Lattitude { get; set; }

        public virtual DmStreet Street { get; set; }
        public virtual ICollection<DmDeliveryRoute> DmDeliveryRoutes { get; set; }
    }
}
