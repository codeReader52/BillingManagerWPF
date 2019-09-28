using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using BillingManagement.Model;
using BillingManagement.Utils;
using System.Windows.Input;
using System;

namespace BillingManagement.ViewModel
{
    public class BillSearchViewModel : ViewModelWithNotifierBase
    {
        public static readonly DateTime BEGINNING_OF_TIME = new DateTime(0);
        public enum PaidStatusFilter
        {
            NoFilter,
            Unpaid,
            Paid,
        };

        public ObservableCollection<BillInfo> BillList { get; set; } = new ObservableCollection<BillInfo>();
        public BillInfo SelectedBill { get; set; }

        public ObservableCollection<PaidStatusFilter> AllPaidStatusFilters { get; } = new ObservableCollection<PaidStatusFilter>
        {
            PaidStatusFilter.NoFilter, PaidStatusFilter.Unpaid, PaidStatusFilter.Paid
        };
        public PaidStatusFilter FilterByPaidStatus { get; set; } = PaidStatusFilter.NoFilter;
        public DateTime FilterByDateFrom { get; set; } = BEGINNING_OF_TIME;
        public ICommand AddBill { get; private set; }
        public ICommand OnGridDataRowClicked { get; private set; }
        public ICommand RequestBills { get; private set; }


        public BillSearchViewModel(IBillReaderWriter billReaderWriter, NavigatorViewModel navigator)
        {
            _navigator = navigator;
            _billReaderWriter = billReaderWriter;

            AddBill = new RelayCommand((_) => { _navigator.ViewNameToDisplay = Constants.BillDetailView; });
            OnGridDataRowClicked = new RelayCommand((_) => { DoHandleGridDataRowClicked(); });
            RequestBills = new RelayCommand(_ => { DoRequestBills(); });
        }

        private Predicate<BillInfo> GetFilterByPaidStatus()
        {
            Predicate <BillInfo> filterByPaidStatus = bill =>
            {
                if (FilterByPaidStatus == PaidStatusFilter.NoFilter)
                    return true;
                else if (FilterByPaidStatus == PaidStatusFilter.Paid)
                    return bill.IsAlreadyPaid;
                else
                    return !bill.IsAlreadyPaid;
            };
            return filterByPaidStatus;
        }

        private Predicate<BillInfo> GetFilterByDateTime()
        {
            Predicate<BillInfo> filterByDateTime = bill => 
            {
                return bill.DueDate >= FilterByDateFrom;
            };
            return filterByDateTime;
        }

        private void DoRequestBills()
        {
            BillList.Clear();
            SelectedBill = new BillInfo();
            Predicate<BillInfo> filterByPaidStatus = GetFilterByPaidStatus();
            Predicate<BillInfo> filterByDateTime = GetFilterByDateTime();
            IList<BillInfo> bills = _billReaderWriter.GetBillByFilter(bill => filterByPaidStatus(bill) && filterByDateTime(bill));
            foreach (BillInfo bill in bills)
            {
                BillList.Add(bill);
            }
        }

        private void DoHandleGridDataRowClicked()
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
        }

        private IBillReaderWriter _billReaderWriter;
        private NavigatorViewModel _navigator;
    }
}
