using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXMauiApp.Models
{
    public class Invite
    {
        public string _id { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public string lock_id { get; set; }
        public string date { get; set; }
        public string lock_name { get; set; }
        public bool accepted { get; set; }
    }
}
