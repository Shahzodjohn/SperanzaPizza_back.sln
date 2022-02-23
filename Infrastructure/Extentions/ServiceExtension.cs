using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using SperanzaPizzaApi.Data;
using SperanzaPizzaApi.Infrastructure.Filters;
using SperanzaPizzaApi.Services.Messages;
using SperanzaPizzaApi.Services.Payments;
using Stripe;
using ProductService = SperanzaPizzaApi.Services.Products.ProductService;
using OrderService = SperanzaPizzaApi.Services.Orders.OrderService;

namespace SperanzaPizzaApi.Infrastructure.Extentions
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddDbContext<dbPizzaContext>();
            services.AddScoped<MessageService>();
            services.AddScoped<ProductService>();
            services.AddHttpClient<PaymentService>()
                .ConfigurePrimaryHttpMessageHandler(() =>
                {
                    return new HttpClientHandler
                    {
                        AllowAutoRedirect = false
                    };
                });
            services.AddScoped<OrderService>();
            
            return services;
        }

    }
}