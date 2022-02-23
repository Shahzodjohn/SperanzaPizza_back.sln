using System;
using System.Collections.Generic;

#nullable disable

namespace SperanzaPizzaApi.Models
{
    public partial class DmProductPrice
    {
        public DmProductPrice()
        {
            DmOrderProducts = new HashSet<DmOrderProduct>();
        }

        public int Id { get; set; }
        public int ProductId { get; set; }
        public int? SizeId { get; set; }
        public decimal PriceValue { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsPromo { get; set; }
        public decimal? PromoValue { get; set; }
        public DateTime? PromoEndDate { get; set; }

        public virtual DmProduct Product { get; set; }
        public virtual DmProductSize Size { get; set; }
        public virtual ICollection<DmOrderProduct> DmOrderProducts { get; set; }
    }
}
