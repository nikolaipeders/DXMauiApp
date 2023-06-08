using DXMauiApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXMauiApp.Services
{
    public interface ILockRestService
    {
        Task<List<Lock>> GetAllLocksAsync(TokenRequest token);
        Task<Lock> GetLockByIdAsync(TokenRequest token, string id);
        Task<HttpResponseMessage> SaveLockAsync(TokenRequest token, Lock newLock, bool isNewItem = false);
        Task<HttpResponseMessage> DeleteLockAsync(TokenRequest request, Lock exiLock);
        Task<HttpResponseMessage> RemoveAccessAsync(TokenRequest request, Lock exiLock, User user);
    }
}
