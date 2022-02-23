using System;
using System.Collections.Generic;

#nullable disable

namespace SperanzaPizzaApi.Models
{
    public partial class DmStreet
    {
        public DmStreet()
        {
            DmAddresses = new HashSet<DmAddress>();
        }

        public int Id { get; set; }
        public string StreetName { get; set; }
        public int CityId { get; set; }

        public virtual DmCity City { get; set; }
        public virtual ICollection<DmAddress> DmAddresses { get; set; }
    }
}
