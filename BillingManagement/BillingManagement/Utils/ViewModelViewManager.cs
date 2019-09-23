using System;
using System.Collections.Generic;
using BillingManagement.ViewModel;
using BillingManagement.Model;

namespace BillingManagement.Utils
{
    public static class ViewModelViewManager
    {
        public static string MainWindow = "MainWindow";
        public static string BillDetailView = "BillDetailView";
        public static string BillSearchView = "BillSearchView";
        private static NavigatorViewModel _navigator = null;

        public static IDictionary<string, Func<object>> MapViewNameToViewModelFactory = new Dictionary<string, Func<object>>
        {
            { BillDetailView, GetBillDetailViewModel },
            { BillSearchView, GetBillSearchViewModel },
            { MainWindow, GetNavigator },
        };

        private static NavigatorViewModel GetNavigator()
        {
            _navigator = new NavigatorViewModel();
            return _navigator;
        }

        private static BillSearchViewModel GetBillSearchViewModel()
        {
            IBillReaderWriter reader = new BillJsonReaderWriter(Constants.BILLING_FOLDER);
            return new BillSearchViewModel(reader, _navigator);
        }

        private static BillDetailViewModel GetBillDetailViewModel()
        {
            IBillReaderWriter writer = new BillJsonReaderWriter(Constants.BILLING_FOLDER);
            return new BillDetailViewModel(writer);
        }

        public static bool IsChildView(string viewName)
        {
            if (viewName == MainWindow)
                return false;
            else
                return true;
        }
    }
}
