using System;
using System.Collections.Generic;

#nullable disable

namespace SperanzaPizzaApi.Models
{
    public partial class DmProduct
    {
        public DmProduct()
        {
            DmProductPrices = new HashSet<DmProductPrice>();
        }

        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Formulation { get; set; }
        public int CategoryId { get; set; }
        public bool? IsActive { get; set; }
        public string ImagePath { get; set; }

        public virtual DmProductCategory Category { get; set; }
        public virtual ICollection<DmProductPrice> DmProductPrices { get; set; }
    }
}
