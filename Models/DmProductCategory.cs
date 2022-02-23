using System;
using System.Collections.Generic;

#nullable disable

namespace SperanzaPizzaApi.Models
{
    public partial class DmProductCategory
    {
        public DmProductCategory()
        {
            DmProductSizes = new HashSet<DmProductSize>();
            DmProducts = new HashSet<DmProduct>();
        }

        public int Id { get; set; }
        public string CategoryName { get; set; }

        public virtual ICollection<DmProductSize> DmProductSizes { get; set; }
        public virtual ICollection<DmProduct> DmProducts { get; set; }
    }
}
