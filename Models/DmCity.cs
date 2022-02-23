using System;
using System.Collections.Generic;

#nullable disable

namespace SperanzaPizzaApi.Models
{
    public partial class DmCity
    {
        public DmCity()
        {
            DmStreets = new HashSet<DmStreet>();
        }

        public int Id { get; set; }
        public string CityName { get; set; }

        public virtual ICollection<DmStreet> DmStreets { get; set; }
    }
}
