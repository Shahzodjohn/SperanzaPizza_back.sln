using System;
using System.Collections.Generic;

#nullable disable

namespace SperanzaPizzaApi.Models
{
    public partial class DmProductSize
    {
        public DmProductSize()
        {
            DmProductPrices = new HashSet<DmProductPrice>();
        }

        public int Id { get; set; }
        public string SizeName { get; set; }
        public string SizeValue { get; set; }
        public int ProductCategoryId { get; set; }

        public virtual DmProductCategory ProductCategory { get; set; }
        public virtual ICollection<DmProductPrice> DmProductPrices { get; set; }
    }
}
