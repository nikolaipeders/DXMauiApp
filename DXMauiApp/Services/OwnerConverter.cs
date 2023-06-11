using CommunityToolkit.Mvvm.Messaging;
using DXMauiApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXMauiApp.Services
{
    public class OwnerConverter : IValueConverter
    {
        string id;

        public OwnerConverter()
        {
            WeakReferenceMessenger.Default.Register<MessagePublisher>(this, (r, m) =>
            {
                id = m.Value.Item1;
            });
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string owner = value as string;

            if (owner != null && id != null && owner.Equals(id))
                return "Owner";
            else
                return "Visitor";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
