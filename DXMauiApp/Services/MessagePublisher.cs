using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace DXMauiApp.Services
{
    public class MessagePublisher : ValueChangedMessage<(string, string)>
    {
        public MessagePublisher(string value1, string value2) : base((value1, value2))
        {

        }
    }
}
