using System;
using System.Collections.Generic;
using System.Windows.Input;
using BillingManagement.View;
using BillingManagement.Utils;
using BillingManagement.Model;

namespace BillingManagement.ViewModel
{
    public class NavigatorViewModel : ViewModelWithNotifierBase
    {
        public string ViewNameToDisplay
        {
            get { return _childViewName; }
            set
            {
                ICollection<string> listViewNames = ViewModelViewManager.MapViewNameToViewModelFactory.Keys;
                
                if (listViewNames.Contains(value) && ViewModelViewManager.IsChildView(value))
                {
                    _childViewName = value;
                    NotifyPropChanged(nameof(ViewNameToDisplay));
                }
            }
        }

        private string _childViewName = Constants.BillSearchView;

        public ICommand GoToBillDetail { get; private set; }
        public int BillIdSelected { get; set; } = 0;

        public NavigatorViewModel()
        {
            GoToBillDetail = new RelayCommand((_) =>
            {
                ViewNameToDisplay = Constants.BillDetailView;
            });
        }
    }
}
