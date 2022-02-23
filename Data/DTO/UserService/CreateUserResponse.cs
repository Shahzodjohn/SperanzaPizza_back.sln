namespace SperanzaPizzaApi.Data.DTO.UserService
{
    public class CreateUserResponse
    {
        public bool? isCreated { get; set; }
        public string message { get; set; }
        public int? userId { get; set; }
    }
}