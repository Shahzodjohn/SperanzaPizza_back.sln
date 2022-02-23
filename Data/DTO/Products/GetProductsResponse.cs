#nullable enable
using System.Collections.Generic;
using System.Drawing;
using SperanzaPizzaApi.Data.DTO.ProductCategories;

namespace SperanzaPizzaApi.Data.DTO.Products
{
    public class GetProductsResponse : GetProductCategoriesResponse
    {
        public List<ProductItem?>? products { get; set; }
    }

    public class ProductItem
    {
        public int productId { get; set; }
        public string productName { get; set; }
        public string formulation { get; set; }
        public List<PriceResponse?> prices { get; set; }        
    }
    
}