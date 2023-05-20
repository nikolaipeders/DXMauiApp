﻿using DXMauiApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXMauiApp.Services
{
    public interface IUserRestService
    {
        Task<HttpResponseMessage> SaveUserAsync(User user, bool isNewItem = false);
        Task<string> UserLoginAsync(User user);
        Task<HttpResponseMessage> UserSignOutAsync(TokenRequest request);


    }
}