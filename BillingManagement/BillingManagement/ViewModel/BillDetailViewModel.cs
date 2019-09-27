using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;
using BillingManagement.Model;
using BillingManagement.Utils;
using BillingManagement.View;
using System.Collections.Generic;

namespace BillingManagement.ViewModel
{
    public class BillDetailViewModel: ViewModelWithNotifierBase
    {
        public ICommand OnRecordButtonClick { get; private set; }
        public ICommand OnCancel { get; private set; }

        public ObservableCollection<BillType> AllBillTypes
        {
            get
            {
                var allTypes = Enum.GetValues(typeof(BillType)).Cast<BillType>();
                ObservableCollection<BillType> retCollection = new ObservableCollection<BillType>(allTypes);
                return retCollection;
            }
        }

        public string ErrorString
        {
            get { return _errString; }
            set
            {
                _errString = value;
                NotifyPropChanged(nameof(ErrorString));
            }
        }

        public string BillName
        {
            get { return _bill.BillName; }
            set
            {
                _bill.BillName = value;
                OnRecordButtonClick.CanExecute(null);
            }
        }

        public BillType BillType
        {
            get { return _bill.Type; }
            set
            {
                _bill.Type = value;
                NotifyPropChanged(nameof(BillType));
            }
        }

        public DateTime DueDate
        {
            get { return _bill.DueDate; }
            set
            {
                _bill.DueDate = value;
                NotifyPropChanged(nameof(DueDate));
            }
        }

        public double Amount
        {
            get { return _bill.Amount; }
            set
            {
                _bill.Amount = value;
                NotifyPropChanged(nameof(Amount));
            }
        }

        public string Description
        {
            get { return _bill.Description; }
            set
            {
                _bill.Description = value;
                NotifyPropChanged(nameof(Description));
            }
        }

        public BillDetailViewModel(IBillReaderWriter billReaderWriter, NavigatorViewModel navigator)
        {
            _bill = new BillInfo
            {
                Type =BillType.Unknown,
                BillName="",
                DueDate =DateTime.Now,
                Amount =0,
                Description =""
            };
            _billReaderWriter = billReaderWriter;
            _navigator = navigator;
            if (_navigator.BillIdSelected > 0)
            {
                LoadBillInfo(_navigator.BillIdSelected);
            }
            navigator.BillIdSelected = 0;

            OnRecordButtonClick = new RelayCommand((_) => CanSave(), (_) => DoSave());
            OnCancel = new RelayCommand((_) => { _navigator.ViewNameToDisplay = Constants.BillSearchView; });
        }

        private bool CanSave()
        {
            if (BillName == null || BillName == "")
                return false;
            else
                return true;
        }

        private void DoSave()
        {
            bool saveSuccess = _billReaderWriter.Record(_bill, out string error);
            bool navigatorNotNull = _navigator != null;

            if (saveSuccess && navigatorNotNull)
            {
               _navigator.ViewNameToDisplay = Constants.BillSearchView;
            }
            else
            {
                ErrorString = error;
            }
        }

        private void LoadBillInfo(int billId)
        {
            IList<BillInfo> listBills = _billReaderWriter.GetBillByFilter(b => b.Id == billId);
            if (listBills.Count > 0)
                _bill = listBills[0];
        }

        private string _errString = "";
        private BillInfo _bill = null;
        private IBillReaderWriter _billReaderWriter = null;
        private NavigatorViewModel _navigator = null;
    }
}
