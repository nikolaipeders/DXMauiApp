﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXMauiApp.Models
{
    public class User
    {
        public string _id { get; set; }
        public string email { get; set; }
        public List<String> user_access { get; set; }
        public string name { get; set; }
        public string image { get; set; }
        public string password { get; set; }

    }
}
