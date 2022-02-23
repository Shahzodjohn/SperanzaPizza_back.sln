namespace SperanzaPizzaApi.Data.DTO.UserService
{
    public class LoginUserRequest
    {
        public string login {get; set;}
        public string password {get; set;}
        public int roleId {get; set;}
    }
}