using DXMauiApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXMauiApp.Services
{
    public interface IInviteRestService
    {
        Task<List<Invite>> GetAllInvitesAsync(TokenRequest token);
        Task<HttpResponseMessage> SendInviteAsync(TokenRequest request, Lock exiLock, string email);
        Task<HttpResponseMessage> RespondToinviteAsync(TokenRequest request, string response);
    }
}
