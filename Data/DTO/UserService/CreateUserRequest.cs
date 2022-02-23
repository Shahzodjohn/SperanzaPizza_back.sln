namespace SperanzaPizzaApi.Data.DTO.UserService
{
    public class CreateUserRequest
    {
        public string firstName {get; set;}
        public string secondName {get; set;}
        public string lastName {get; set;}
        public string login {get; set;}
        public string password {get; set;}
        public int roleId {get; set;}
        public string email {get; set;} 
    }
}