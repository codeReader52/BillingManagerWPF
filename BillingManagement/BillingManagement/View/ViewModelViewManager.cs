using System;
using System.Collections.Generic;
using BillingManagement.ViewModel;
using BillingManagement.Model;
using BillingManagement.Utils;
using BillingManagement.Utils.UiUtils;

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
            BillSearchViewModel bsvm = new BillSearchViewModel(new SqliteDbBillReaderWriter(Constants.CONN_STRING), _navigator);
            bsvm.FilterByDateFrom = new DateTime(DateTime.Now.Year, 1, 1);
            bsvm.PopUpWinService = new MessageBoxService();
            return bsvm;
        }

        private static BillDetailViewModel GetBillDetailViewModel()
        {
            BillDetailViewModel bdvm = new BillDetailViewModel(
                new SqliteDbBillReaderWriter(Constants.CONN_STRING), 
                new FileToByteStreamUiService(), 
                _navigator);
            bdvm.PopUpService = new MessageBoxService();
            return bdvm;
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
