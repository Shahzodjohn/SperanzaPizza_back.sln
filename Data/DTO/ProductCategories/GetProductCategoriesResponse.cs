using System.Collections.Generic;

namespace SperanzaPizzaApi.Data.DTO.ProductCategories
{
    public class GetProductCategoriesResponse
    {
        public int? categoryId { get; set; }
        public string categoryName { get; set; }
        public List<SizeResponse?> sizes { get; set; }
    }
}