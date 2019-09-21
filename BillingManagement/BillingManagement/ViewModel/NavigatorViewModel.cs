using System;
using System.Collections.Generic;
using System.Windows.Input;
using BillingManagement.Utils;

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

        private string _childViewName = ViewModelViewManager.BillSearchView;

        public ICommand GoToBillDetail { get; private set; }

        public NavigatorViewModel()
        {
            GoToBillDetail = new RelayCommand((_) =>
            {
                ViewNameToDisplay = ViewModelViewManager.BillDetailView;
            });
        }

        
    }
}
