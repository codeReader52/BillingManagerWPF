﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BillingManagement.ViewModel;
using BillingManagement.Model;
using BillingManagementTest.Mocks;
using BillingManagement.Utils;
using Moq;
using System.Collections.Generic;

namespace BillingManagementTest
{
    [TestClass]
    public class TestBillDetailViewModel
    {
        [TestMethod]
        public void TestCreateNewBill()
        {
            foreach (bool paid in new List<bool> { true, false})
            {
                MockBillReaderWriter billWriter = new MockBillReaderWriter();
                BillDetailViewModel billDetail = new BillDetailViewModel(billWriter, new NavigatorViewModel());
                billDetail.Amount = 1;
                billDetail.BillName = "Test name";
                billDetail.BillType = BillType.Food;
                billDetail.Description = "Test description";
                billDetail.DueDate = new DateTime(1990, 1, 1);
                billDetail.IsAlreadyPaid = paid;

                Assert.IsTrue(billDetail.OnRecordButtonClick.CanExecute(null));
                billDetail.OnRecordButtonClick.Execute(null);
                BillInfo actualBilLSaved = billWriter.BillSaved;

                Assert.AreEqual(actualBilLSaved.Amount, 1);
                Assert.AreEqual(actualBilLSaved.BillName, "Test name");
                Assert.AreEqual(actualBilLSaved.Type, BillType.Food);
                Assert.AreEqual(actualBilLSaved.Description, "Test description");
                Assert.AreEqual(actualBilLSaved.DueDate, new DateTime(1990, 1, 1));
                Assert.AreEqual(actualBilLSaved.IsAlreadyPaid, paid);
            }
            
        }
        [TestMethod]
        public void TestBillDetailViewWillAlwaysGetAllBillTypes()
        {
            Mock<IBillReaderWriter> mockWriter = new Mock<IBillReaderWriter>();
            BillDetailViewModel vm = new BillDetailViewModel(mockWriter.Object, new NavigatorViewModel());

            Assert.AreEqual(vm.AllBillTypes.Count, 4);
            foreach (BillType type in new List<BillType> { BillType.Food, BillType.MiscSpending, BillType.Unknown, BillType.Utility })
            {
                Assert.IsTrue(vm.AllBillTypes.Contains(type));
            }
        }
    }
}
