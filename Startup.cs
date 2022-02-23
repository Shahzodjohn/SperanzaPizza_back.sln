using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Org.BouncyCastle.Math.EC;
using SperanzaPizzaApi.Data;
using SperanzaPizzaApi.Data.Common;
using SperanzaPizzaApi.Infrastructure.Extentions;
using SperanzaPizzaApi.Infrastructure.Filters;
using SperanzaPizzaApi.Infrastructure.Helpers;
using SperanzaPizzaApi.Services.Messages;

namespace SperanzaPizzaApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var messageConfig = Configuration.GetSection("PizzaMessage").Get<MessageSecretsHelperModel>();
            var testPaymentConfig = Configuration.GetSection("SandboxPayment").Get<PayuSecretsHelperModel>();
           // var paymentConfig = Configuration.GetSection("PayuPayment").Get<PayuSecretsHelperModel>();
            services.AddSingleton<MessageSecretsHelperModel>(messageConfig);
            services.AddSingleton<PayuSecretsHelperModel>(testPaymentConfig);
            services.AddServices();
            
            PaymentToken.Config = testPaymentConfig;
            services.AddControllers();
            services.AddTransient<PayuFilter>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SperanzaPizzaApi", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SperanzaPizzaApi v1"));
            //}

            //app.UseHttpsRedirection();
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseRouting();
            app.UseAuthorization();
            var wsOptions = new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(30),
            };
            // wsOptions.AllowedOrigins.Add("http://135.125.239.81:3000");            
            app.UseWebSockets(wsOptions);
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}