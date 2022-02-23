using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SperanzaPizzaApi.Infrastructure.Helpers;

namespace SperanzaPizzaApi.Data.Common
{
    public static class PaymentToken
    {
        private static string _token = String.Empty;
        public static PayuSecretsHelperModel Config;
        
        public static string Token {
            get {
                return (PaymentToken.IsExpired())? GetNewToken() : _token;
            }
        }
        public static DateTime ExpirationDate { get; set; }
        private static string GetNewToken()
        {
            var url = $"https://secure.snd.payu.com/pl/standard/user/oauth/authorize?grant_type=client_credentials&client_id={Config.ClientId}&client_secret={Config.ClientSecret}";
            using (HttpClient client = new HttpClient())
            {
                var response = client.GetAsync(url).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    
                    
                    var responseData = response.Content.ReadAsStringAsync().Result;
                    var jsonData =  JsonConvert.DeserializeObject<Dictionary<string, string>>(responseData);
                    _token = jsonData["access_token"];
                    PaymentToken.ExpirationDate = DateTime.Now.AddSeconds(Int32.Parse(jsonData["expires_in"]));
                    return _token;
                }
                return null;
            }
        }
        
        public static bool IsExpired()
        {
            return _token == string.Empty || PaymentToken.ExpirationDate <= DateTime.Now;
        }
    }
}