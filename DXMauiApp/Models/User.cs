using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXMauiApp.Models
{
    public class User
    {
        public string Email { get; set; }
        public string RFID { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
