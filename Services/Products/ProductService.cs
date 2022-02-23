using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SperanzaPizzaApi.Data;
using SperanzaPizzaApi.Data.DTO.ProductCategories;
using SperanzaPizzaApi.Data.DTO.Products;
using SperanzaPizzaApi.Models;

namespace SperanzaPizzaApi.Services.Products
{
    public class ProductService
    {
        public async Task<List<GetProductsResponse>> GetProductsByCategory(GetByCategoryParams request)
        {
            using (SperanzaPizzaApi.Models.SPToCoreContext db = new SperanzaPizzaApi.Models.SPToCoreContext())
            {
                var queryResult = (await db.EXPOSE_dm_Products_GetByCategoryAsync(request.categoryId, request.query));
                var result = queryResult.Select(query => new GetProductsResponse
                    {
                        products = (query.products == null)? null: JsonConvert.DeserializeObject<List<ProductItem>>(query.products),
                        categoryId = query.categoryId,
                        categoryName = query.categoryName,
                        sizes = JsonConvert.DeserializeObject<List<SizeResponse>>(query.sizes)
                        
                    });
                return result.ToList();
            }
        }

        public async Task<List<GetProductsResponse>> GetAllProducts(GetAllParams request)
        {
            using (SperanzaPizzaApi.Models.SPToCoreContext db = new SperanzaPizzaApi.Models.SPToCoreContext()) {
                var result = (await db.EXPOSE_dm_Products_GetAllAsync(request.query))
                    .Select(query => new GetProductsResponse {
                        categoryId = query.categoryId,
                        categoryName = query.CategoryName,
                        sizes = JsonConvert.DeserializeObject<List<SizeResponse>>(query.sizes),
                        products = JsonConvert.DeserializeObject<List<ProductItem>>(query.products)
                    });
                return result.ToList();
            }
        }
    }
}