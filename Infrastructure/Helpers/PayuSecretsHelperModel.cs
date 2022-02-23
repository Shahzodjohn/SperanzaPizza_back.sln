using System.Configuration;

namespace SperanzaPizzaApi.Infrastructure.Helpers
{
    public sealed class PayuSecretsHelperModel
    {
        public string MerchantPosId { get; set; }
        public string SecondKey { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}