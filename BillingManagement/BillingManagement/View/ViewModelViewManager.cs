using System;
using System.Collections.Generic;
using BillingManagement.ViewModel;
using BillingManagement.Model;
using BillingManagement.Utils;

namespace BillingManagement.View
{
    public static class ViewModelViewManager
    {
        private static NavigatorViewModel _navigator = new NavigatorViewModel();

        public static IDictionary<string, Func<object>> MapViewNameToViewModelFactory = new Dictionary<string, Func<object>>
        {
            { Constants.BillDetailView, GetBillDetailViewModel },
            { Constants.BillSearchView, GetBillSearchViewModel },
            { Constants.MainWindow, GetNavigator },
        };

        private static NavigatorViewModel GetNavigator()
        {
            return _navigator;
        }

        private static BillSearchViewModel GetBillSearchViewModel()
        {
            return new BillSearchViewModel(new SqliteDbBillReaderWriter(Constants.CONN_STRING), _navigator);
        }

        private static BillDetailViewModel GetBillDetailViewModel()
        {
            return new BillDetailViewModel(
                new SqliteDbBillReaderWriter(Constants.CONN_STRING), 
                new FileToByteStreamUiService(), 
                _navigator);
        }

        public static bool IsChildView(string viewName)
        {
            if (viewName == Constants.MainWindow)
                return false;
            else
                return true;
        }
    }
}
