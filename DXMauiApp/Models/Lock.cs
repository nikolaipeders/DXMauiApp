using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXMauiApp.Models
{
    public class Lock
    {
        public string _id { get; set; }
        public string serial { get; set; }
        public string name { get; set; }
        public string location { get; set; }
        public bool active { get; set; }
        public string owner { get; set; }
    }
}
