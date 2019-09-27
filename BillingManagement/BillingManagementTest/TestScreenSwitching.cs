using BillingManagement.Model;
using BillingManagement.Utils;
using BillingManagement.ViewModel;
using BillingManagementTest.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace BillingManagementTest
{
    [TestClass]
    public class TestScreenSwitching
    {
        [TestMethod]
        public void TestBillSearchWillSwitchToBillDetailOnAdd()
        {
            NavigatorViewModel nav = new NavigatorViewModel();
            nav.ViewNameToDisplay = Constants.BillSearchView;
            BillSearchViewModel bs = new BillSearchViewModel(new MockBillReaderWriter(), nav);
            bs.AddBill.Execute(null);
            Assert.AreEqual(nav.ViewNameToDisplay, Constants.BillDetailView);
        }

        [TestMethod]
        public void TestBillDetailSwitchBackToBillSearchOnSaveSuccessOrCancel()
        {
            NavigatorViewModel nav = new NavigatorViewModel();
            nav.ViewNameToDisplay = Constants.BillDetailView;
            BillDetailViewModel bd = new BillDetailViewModel(new MockBillReaderWriter(), nav);
            bd.OnRecordButtonClick.Execute(null);
            Assert.AreEqual(nav.ViewNameToDisplay, Constants.BillSearchView);

            nav.ViewNameToDisplay = Constants.BillDetailView;
            bd.OnCancel.Execute(null);
            Assert.AreEqual(nav.ViewNameToDisplay, Constants.BillSearchView);
        }

        [TestMethod]
        public void TestBillDetailStaysSameOnSaveFailure()
        {
            MockBillReaderWriter readerWriter = new MockBillReaderWriter(
                () => new List<BillInfo>(), 
                (BillInfo _, out string err) => 
                {
                    err = "";
                    return false;
                });

            NavigatorViewModel nav = new NavigatorViewModel();
            nav.ViewNameToDisplay = Constants.BillDetailView;
            BillDetailViewModel db = new BillDetailViewModel(readerWriter, nav);
            db.OnRecordButtonClick.Execute(null);
            Assert.AreEqual(nav.ViewNameToDisplay, Constants.BillDetailView);
        }

        [TestMethod]
        public void TestBillDetailCanLoadCorrectWhenClickingOnBillOnBillSearch()
        {
            NavigatorViewModel nav = new NavigatorViewModel();
            nav.ViewNameToDisplay = Constants.BillSearchView;
            MockBillReaderWriter billReaderWriter = new MockBillReaderWriter();
            BillSearchViewModel bs = new BillSearchViewModel(billReaderWriter, nav);

            var allBills = billReaderWriter.GetAllBills();
            bs.SelectedBill = allBills[1];
            bs.OnGridDataRowClicked.Execute(null);

            Assert.AreEqual(nav.ViewNameToDisplay, Constants.BillDetailView);
            Assert.AreEqual(nav.BillIdSelected, 2);
            Assert.IsTrue(bs.SelectedBill.SameData(new BillInfo()));
        }

        [TestMethod]
        public void TestBillDetailLoadWillResetNavigatorBillIdSelected()
        {
            NavigatorViewModel nav = new NavigatorViewModel();
            nav.BillIdSelected = 1;
            BillDetailViewModel _ = new BillDetailViewModel(new MockBillReaderWriter(), nav);
            Assert.AreEqual(nav.BillIdSelected, 0);
        }

        [TestMethod]
        public void TestBillDetailLoadWillSetBillInfo()
        {
            MockBillReaderWriter readerWriter = new MockBillReaderWriter();
            IList<BillInfo> allBills = readerWriter.GetAllBills();

            NavigatorViewModel nav = new NavigatorViewModel();
            nav.BillIdSelected = 2;
            BillDetailViewModel billDetail = new BillDetailViewModel(readerWriter, nav);

            BillInfo billInBillDetailModel = new BillInfo()
            {
                Amount = billDetail.Amount,
                BillName = billDetail.BillName,
                Description = billDetail.Description,
                DueDate = billDetail.DueDate,
                Type = billDetail.BillType,
            };

            Assert.IsTrue(billInBillDetailModel.SameData(allBills[1]));
        }
    }
}
