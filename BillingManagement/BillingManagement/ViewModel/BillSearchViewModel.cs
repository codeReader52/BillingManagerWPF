
using System.Collections.Generic;
using System.Collections.ObjectModel;
using BillingManagement.View;
using System.Threading.Tasks;
using BillingManagement.Model;
using BillingManagement.Utils;
using System.Windows.Input;

namespace BillingManagement.ViewModel
{
    public class BillSearchViewModel : ViewModelWithNotifierBase
    {
        public ObservableCollection<BillInfo> BillList { get; set; } = new ObservableCollection<BillInfo>();
        public BillInfo SelectedBill { get; set; }

        public ICommand AddBill { get; private set; }
        public ICommand OnGridDataRowClicked { get; private set; }

        public BillSearchViewModel(IBillReaderWriter billReader, NavigatorViewModel navigator)
        {
            IList<BillInfo> bills = billReader.GetAllBills();
            foreach (BillInfo bill in bills)
            {
                BillList.Add(bill);
            }

            _navigator = navigator;

            AddBill = new RelayCommand((_) => { _navigator.ViewNameToDisplay = Constants.BillDetailView; });
            OnGridDataRowClicked = new RelayCommand((_) => 
            {
                if (SelectedBill.Id == 0)
                {
                    return;
                }
                else
                {
                    _navigator.BillIdSelected = SelectedBill.Id;
                    _navigator.ViewNameToDisplay = Constants.BillDetailView;
                    SelectedBill = new BillInfo();
                }
            });
        }

        private NavigatorViewModel _navigator;
    }
}
