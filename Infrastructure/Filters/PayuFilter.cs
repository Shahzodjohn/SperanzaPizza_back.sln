using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using System.Web.Http.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SperanzaPizzaApi.Infrastructure.Filters
{
    public class PayuFilter : IAsyncActionFilter
    {
        private string _secondKey = "5d342f251c85211ce70a99a55e70235e";

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //verify Payu signature: algorithm for checking signature: https://developers.payu.com/en/restapi.html#references_verification_of_notifications_signature
            List<string> headers = context.HttpContext.Request.Headers["OpenPayu-Signature"].ToString().Split(";")
                .ToList();
            var signatureValue = (headers.FirstOrDefault(x => x.Contains("signature")))?.Split("=")[1];

            var concat = context.ActionArguments.Values.ToList()[0].ToString() + _secondKey;
            MD5 md5 = new MD5CryptoServiceProvider();
            md5.ComputeHash(Encoding.ASCII.GetBytes(concat));
            var hash = md5.Hash;
            var result = new StringBuilder();
            if (hash != null)
                foreach (var t in hash)
                {
                    result.Append(t.ToString("x2"));
                }

            if (signatureValue != result.ToString())
            {
                context.Result = new UnauthorizedObjectResult("Verification failed");
                return;
            }

            await next();
        }
    }
}