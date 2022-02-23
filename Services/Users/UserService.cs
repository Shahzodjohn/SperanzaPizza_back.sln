using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using SperanzaPizzaApi.Data.DTO.UserService;

namespace SperanzaPizzaApi.Services.Users
{
    public class UserService
    {
        public async Task<CreateUserResponse> RegisterUser(CreateUserRequest parameters)
        {
            using (SperanzaPizzaApi.Models.SPToCoreContext db = new SperanzaPizzaApi.Models.SPToCoreContext())
            {
                var queryResult = await db.EXPOSE_dm_Users_RegisterAsync(
                    parameters.firstName,
                    parameters.secondName,
                    parameters.lastName,
                    parameters.login,
                    parameters.password,
                    parameters.roleId,
                    parameters.email);
                if (queryResult == null)
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                        {ReasonPhrase = "Не удалось сохранить пользователя" });
                return new CreateUserResponse()
                {
                    isCreated = queryResult[0].isCreated,
                    message = queryResult[0].message,
                    userId = queryResult[0].userId
                };
            }
        }

        public async Task<LoginUserResponse> LoginUser(LoginUserRequest parameters)
        {
            using (SperanzaPizzaApi.Models.SPToCoreContext db = new SperanzaPizzaApi.Models.SPToCoreContext()) {
                var result = await db.EXPOSE_dm_Users_LoginAsync(parameters.login, parameters.password, parameters.roleId);
                if (result == null)
                    throw new  HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                        {ReasonPhrase = "Пользователь не найден" });
                if (result.FirstOrDefault().token == null)
                    throw new  HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                        {ReasonPhrase = "Неверный логин или пароль" });
                return new LoginUserResponse()
                {
                    message = result[0].message,
                    token = result[0].token
                };
            }
        }
        
    }
}