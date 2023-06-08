﻿using DXMauiApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXMauiApp.Services
{
    public class OwnerConverter : IValueConverter
    {
        string id;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Subscribe to the "AccountRegistered" message
            MessagingCenter.Subscribe<LocksViewModel, string>(this, "TransferId", OnTransferRegistered);

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

        private void OnTransferRegistered(LocksViewModel sender, string transferInfo)
        {
            // Update email and password fields
            id = transferInfo;
        }
    }
}
