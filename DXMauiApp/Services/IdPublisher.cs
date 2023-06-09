using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace DXMauiApp.Services
{
    public class IdPublisher : ValueChangedMessage<string>
    {
        public IdPublisher(string value) : base(value)
        {

        }
    }
}
