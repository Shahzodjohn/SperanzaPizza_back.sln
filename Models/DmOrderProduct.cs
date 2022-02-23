using System;
using System.Collections.Generic;

#nullable disable

namespace SperanzaPizzaApi.Models
{
    public partial class DmOrderProduct
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductPriceId { get; set; }
        public int ProductCount { get; set; }

        public virtual DmOrder Order { get; set; }
        public virtual DmProductPrice ProductPrice { get; set; }
    }
}
