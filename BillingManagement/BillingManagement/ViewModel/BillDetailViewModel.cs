﻿using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using BillingManagement.Model;
using BillingManagement.Utils;

namespace BillingManagement.ViewModel
{
    public class BillDetailViewModel: ViewModelWithNotifierBase
    {
        public ICommand OnRecordButtonClick { get; private set; }

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

        public BillDetailViewModel(IBillReaderWriter billwriter, ISequenceManager seqManager)
        {
            _bill = new BillInfo(BillType.Unknown, DateTime.Now, 0, "");
            _billWriter = billwriter;
            _seqMan = seqManager;
            OnRecordButtonClick = new RelayCommand((_) => CanSave(), (_) => DoSave());
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
            int newIndex = _seqMan.SyncIncreaseIndex();
            _bill.Id = newIndex;
            _billWriter.Record(_bill, out string error);
        }

        private BillInfo _bill = null;
        private IBillReaderWriter _billWriter = null;
        private ISequenceManager _seqMan = null;
    }
}
