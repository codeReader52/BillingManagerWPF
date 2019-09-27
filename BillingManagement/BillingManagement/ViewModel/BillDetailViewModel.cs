using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;
using BillingManagement.Model;
using BillingManagement.Utils;
using BillingManagement.View;
using System.Collections.Generic;
using System.IO;

namespace BillingManagement.ViewModel
{
    public class BillDetailViewModel: ViewModelWithNotifierBase
    {
        public ICommand OnRecordButtonClick { get; private set; }
        public ICommand OnCancel { get; private set; }

        public ICommand ImportBillAttachment { get; private set; }
        public ICommand SaveBillAttachment { get; private set; }

        public ObservableCollection<BillType> AllBillTypes
        {
            get
            {
                var allTypes = Enum.GetValues(typeof(BillType)).Cast<BillType>();
                ObservableCollection<BillType> retCollection = new ObservableCollection<BillType>(allTypes);
                return retCollection;
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

        public bool IsAlreadyPaid
        {
            get { return _bill.IsAlreadyPaid; }
            set
            {
                _bill.IsAlreadyPaid = value;
                NotifyPropChanged(nameof(IsAlreadyPaid));
            }
        }

        public byte[] Attachement
        {
            get { return _bill.Attachement; }
            private set
            {
                _bill.Attachement = value;
            }
        }

        //TODO: implement IoC and put the following property together with billreaderwriter, filepicker, navigatorview into the IoC container 
        public IPopUpWinService<string, string> PopUpService { get; set; } = null;

        public BillDetailViewModel(IBillReaderWriter billReaderWriter, IPopUpWinService<string, byte[]> filePickerParser ,NavigatorViewModel navigator)
        {
            _bill.DueDate = DateTime.Now;
            _billReaderWriter = billReaderWriter;
            _navigator = navigator;
            _filePickerParser = filePickerParser;

            if (_navigator.BillIdSelected > 0)
            {
                LoadBillInfo(_navigator.BillIdSelected);
            }
            navigator.BillIdSelected = 0;

            OnRecordButtonClick = new RelayCommand(_ => CanSave(), _ => DoSave());
            OnCancel = new RelayCommand(_ => { _navigator.ViewNameToDisplay = Constants.BillSearchView; });

            ImportBillAttachment = new RelayCommand(_ => 
            {
                // TODO: disable this button while doing modal then enable it
                // TODO: only save/load pdf
                Attachement = new byte[] { };
                _filePickerParser.DoModal();
                Attachement = _filePickerParser.Output;
            });

            SaveBillAttachment = new RelayCommand(_ =>
            {
                if (BillName == "" || BillName == null)
                    return;

                if (Attachement == null || Attachement.Length < 1)
                    return;

                try
                {
                    File.WriteAllBytes($@".\{BillName}.pdf", Attachement);
                }
                catch(Exception e)
                {
                    if (PopUpService == null)
                        return;
                    PopUpService.Input = e.ToString();
                    PopUpService.DoModal();
                }
            });
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
            // TODO: disable this while saving to db
            bool saveSuccess = _billReaderWriter.Record(_bill, out string error);
            bool navigatorNotNull = _navigator != null;

            if (saveSuccess)
            {
                if (navigatorNotNull)
                    _navigator.ViewNameToDisplay = Constants.BillSearchView;

                return;
            }

            if (PopUpService == null)
                return;
            
            PopUpService.Input = error;
            PopUpService.DoModal();
        }

        private void LoadBillInfo(int billId)
        {
            IList<BillInfo> listBills = _billReaderWriter.GetBillByFilter(b => b.Id == billId);
            if (listBills.Count > 0)
                _bill = listBills[0];
        }

        private BillInfo _bill = new BillInfo();
        private IBillReaderWriter _billReaderWriter = null;
        private NavigatorViewModel _navigator = null;
        IPopUpWinService<string, byte[]> _filePickerParser;
    }
}
