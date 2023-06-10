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
        Task<List<Invite>> GetAllInvitesAsync(TokenRequest token, string user_id);
        Task<HttpResponseMessage> SendInviteAsync(TokenRequest request, Lock exiLock, string email);
        Task<HttpResponseMessage> RespondToInviteAsync(TokenRequest request, string id, string answer);
    }
}
