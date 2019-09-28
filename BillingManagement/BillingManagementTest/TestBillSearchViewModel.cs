using BillingManagement.ViewModel;
using BillingManagementTest.Mocks;
using BillingManagement.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BillingManagementTest
{
    [TestClass]
    public class TestBillSearchViewModel
    {
        [TestMethod]
        public void TestBillSearchViewModelCanGetAllBills()
        {
            BillSearchViewModel vm = new BillSearchViewModel(new MockBillReaderWriter(), null);
            Assert.AreEqual(vm.BillList.Count, 0);

            vm.RequestBills.Execute(null);

            Assert.AreEqual(vm.BillList.Count, 3);
            Assert.AreEqual(vm.BillList[0].Id, 1);
            Assert.AreEqual(vm.BillList[0].Type, BillType.Food);
            Assert.AreEqual(vm.BillList[1].Id, 2);
            Assert.AreEqual(vm.BillList[1].Type, BillType.Utility);
            Assert.AreEqual(vm.BillList[2].Id, 3);
            Assert.AreEqual(vm.BillList[2].Type, BillType.MiscSpending);
        }

        [TestMethod]
        public void TestFilterByPaidStatus()
        {
            BillSearchViewModel vm = new BillSearchViewModel(new MockBillReaderWriter(), null);
            Assert.AreEqual(vm.BillList.Count, 0);

            vm.RequestBills.Execute(null);
            Assert.AreEqual(vm.BillList.Count, 3);

            vm.FilterByPaidStatus = BillSearchViewModel.PaidStatusFilter.Paid;
            vm.RequestBills.Execute(null);
            Assert.AreEqual(vm.BillList.Count, 1);
            Assert.AreEqual(vm.BillList[0].Id, 2);

            vm.FilterByPaidStatus = BillSearchViewModel.PaidStatusFilter.Unpaid;
            vm.RequestBills.Execute(null);
            Assert.AreEqual(vm.BillList.Count, 2);
            Assert.AreEqual(vm.BillList[0].Id, 1);
            Assert.AreEqual(vm.BillList[1].Id, 3);
        }

        [TestMethod]
        public void TestFilterByFromDate()
        {
            BillSearchViewModel vm = new BillSearchViewModel(new MockBillReaderWriter(), null);
            Assert.AreEqual(vm.BillList.Count, 0);

            vm.RequestBills.Execute(null);
            Assert.AreEqual(vm.BillList.Count, 3);

            vm.FilterByDateFrom = new DateTime(2019, 1, 2);

            vm.RequestBills.Execute(null);
            Assert.AreEqual(vm.BillList.Count, 2);
            Assert.AreEqual(vm.BillList[0].Id, 2);
            Assert.AreEqual(vm.BillList[1].Id, 3);
        }
    }
}
