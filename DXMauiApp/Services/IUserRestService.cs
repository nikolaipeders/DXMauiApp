using DXMauiApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXMauiApp.Services
{
    public interface IUserRestService
    {
        Task<HttpResponseMessage> CreateUserAsync(User user);
        Task<HttpResponseMessage> UpdateUserAsync(TokenRequest token, User user);
        Task<User> GetUserByIdAsync(TokenRequest token, string id);
        Task<LoginResponse> UserLoginAsync(User user);
        Task<HttpResponseMessage> UserConfirmAccessAsync(TokenRequest request);
        Task<HttpResponseMessage> VerifyUserByFaceAsync(TokenRequest request, Lock exiLock, string image);
    }
}
