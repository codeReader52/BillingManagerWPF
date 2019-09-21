using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using BillingManagement.Model;
using BillingManagement.Utils;
using System.Windows.Input;

namespace BillingManagement.ViewModel
{
    public class BillSearchViewModel : ViewModelWithNotifierBase
    {
        public ObservableCollection<BillInfo> BillList { get; set; } = new ObservableCollection<BillInfo>();

        public ICommand AddBill { get; private set; }

        public BillSearchViewModel(IBillReaderWriter billReader, NavigatorViewModel navigator)
        {
            IList<BillInfo> bills = billReader.GetAllBills();
            foreach (BillInfo bill in bills)
            {
                BillList.Add(bill);
            }

            _navigator = navigator;

            AddBill = new RelayCommand((_) => { _navigator.ViewNameToDisplay = ViewModelViewManager.BillDetailView; });
        }

        private NavigatorViewModel _navigator;
    }
}
