using DXMauiApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXMauiApp.Services
{
    public interface IServerRestService
    {
        Task<HttpResponseMessage> GetStatusAsync();
    }
}
