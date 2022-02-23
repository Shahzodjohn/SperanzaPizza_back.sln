namespace SperanzaPizzaApi.Data.DTO.Messages
{
    public class CreateNewMessageParams
    {
        public string clientname { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string message { get; set; }
    }
}