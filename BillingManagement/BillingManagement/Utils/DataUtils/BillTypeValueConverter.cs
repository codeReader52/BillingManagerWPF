using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using BillingManagement.Model;

namespace BillingManagement.Utils
{
    class BillTypeValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is BillType)
                return _billTypeToDescDict[(BillType)value];
            return "Unknown";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
                return DoBackConvertBillType((string)value);
            return BillType.Unknown;
        }

        private BillType DoBackConvertBillType(string value)
        {
            foreach(BillType type in _billTypeToDescDict.Keys)
            {
                if (string.Compare(value, _billTypeToDescDict[type], true) == 0)
                    return type;
            }

            return BillType.Unknown;
        }

        private IDictionary<BillType, string> _billTypeToDescDict = new Dictionary<BillType, string>
        {
            { BillType.Food, "Food" },
            { BillType.Utility, "Utility" },
            { BillType.MiscSpending, "Miscelaneous spending" },
            { BillType.Unknown, "Unknown" },
        };
    }
}
