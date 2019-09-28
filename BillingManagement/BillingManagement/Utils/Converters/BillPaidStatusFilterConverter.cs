using System;
using System.Collections.Generic;
using System.Globalization;
using BillingManagement.ViewModel;
using System.Windows.Data;

namespace BillingManagement.Utils
{
    public class BillPaidStatusFilterConverter : IValueConverter
    {
        private const string NO_FILTER = "No filter";
        private const string PAID_FILTER = "Paid";
        private const string NOT_PAID_FILTER = "Unpaid";
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is BillSearchViewModel.PaidStatusFilter))
                return "";
            
            switch((BillSearchViewModel.PaidStatusFilter) value)
            {
                case BillSearchViewModel.PaidStatusFilter.Paid: return PAID_FILTER;
                case BillSearchViewModel.PaidStatusFilter.Unpaid: return NOT_PAID_FILTER;
                default: return NO_FILTER;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string stringValue = value.ToString();

            switch(stringValue)
            {
                case PAID_FILTER: return BillSearchViewModel.PaidStatusFilter.Paid;
                case NOT_PAID_FILTER: return BillSearchViewModel.PaidStatusFilter.Unpaid;
                default: return BillSearchViewModel.PaidStatusFilter.NoFilter;
            }
        }
    }
}
