using BillingManagement.Model;
using BillingManagement.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading;

namespace BillingManagementTest
{
    // not ideal for unittest, but we need to test the data access layer
    // ignore most of the times, until we need to specifically test it
    
    [TestClass]
    public class TestEFDataAccessLayer
    {
        private string _connString = @"Data Source=.\billTestWriteRead.sqlite3;";
        private int _currId = 1;

        public object SqlLiteDbBillReaderWriter { get; private set; }

        private static Mutex _mutex = new Mutex();
        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void SetUp()
        {
            _mutex.WaitOne();
            System.Diagnostics.Debug.WriteLine("Begin test " + TestContext.TestName);
            using (DataAccessLayer dbAccess = new DataAccessLayer(_connString))
            {
                BillInfo dummy = new BillInfo { Type = BillType.Unknown, BillName = "", Amount = 0.0, DueDate = new DateTime(1990, 1, 1) };
                dbAccess.AddAndSave(dummy);
                _currId = dummy.Id;
            }

            try
            {
                CleanDatabase();
            }
            catch(Exception e)
            {
                ConsoleColor original = Console.BackgroundColor;
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(e.Message);
                Console.BackgroundColor = original;
            }
        }

        [TestCleanup]
        public void TearDown()
        {
            CleanDatabase();
            System.Diagnostics.Debug.WriteLine("Finish test " + TestContext.TestName);
            _mutex.ReleaseMutex();
        }

        private void CleanDatabase()
        {
            using (DataAccessLayer dbAccess = new DataAccessLayer(_connString))
            {
                dbAccess.BillInfos.RemoveRange(dbAccess.BillInfos);
                dbAccess.SaveChanges();
            }
        }

        private BillInfo GenerateRandomBillInfo(int expectedId)
        {
            Random rnd = new Random();
            BillType type = (BillType)rnd.Next(0, 4);
            double amount = Math.Round(rnd.NextDouble(), 2);
            string description = $"test description {expectedId}";
            string name = $"test name {expectedId}";
            DateTime dueDate = new DateTime(rnd.Next(2010, 2020), rnd.Next(1, 12), rnd.Next(1, 20));
            byte[] attachement = new byte[10];
            for (int i = 0; i < 10; i++)
                attachement[i] = (byte)i;
            return new BillInfo
            {
                Type = type,
                BillName = name,
                DueDate = dueDate,
                Amount = amount,
                Description = description,
                IsAlreadyPaid = amount > 0.5,
                Attachement = attachement,
            };
        }

        private void AddToDb(BillInfo bill)
        {
            using (DataAccessLayer dbAccess = new DataAccessLayer(_connString))
            {
                dbAccess.AddAndSave(bill);
            }
        }

        private void CompareDbBillWithExpectedBill( BillInfo expectedBill)
        {
            using (DataAccessLayer dbAccess2 = new DataAccessLayer(_connString))
            {
                dbAccess2.FindBillById(expectedBill.Id, out BillInfo actualBill);
                Assert.IsTrue(actualBill.SameData(expectedBill));
            }
        }

        [TestMethod]
        public void TestWriteRead()
        {
            foreach (int expectedId in new List<int> { _currId + 1, _currId + 2, _currId + 3 })
            {
                BillInfo bill = GenerateRandomBillInfo(expectedId);
                AddToDb(bill);
                CompareDbBillWithExpectedBill(bill);
            }
        }

        private void ModifyAndSave(BillInfo bill)
        {
            using (DataAccessLayer dbAccess = new DataAccessLayer(_connString))
            {
                dbAccess.ModifyAndSave(bill);
            }
        }

        [TestMethod]
        public void TestUpdate()
        {
            BillInfo bill = GenerateRandomBillInfo(_currId + 1);
            bill.Type = BillType.Food;
            AddToDb(bill);
            CompareDbBillWithExpectedBill(bill);

            bill.Amount += 1;
            ModifyAndSave(bill);
            CompareDbBillWithExpectedBill(bill);

            bill.BillName += " additional part of name";
            ModifyAndSave(bill);
            CompareDbBillWithExpectedBill(bill);

            bill.Description += "additional description";
            ModifyAndSave(bill);
            CompareDbBillWithExpectedBill(bill);

            bill.IsAlreadyPaid = !bill.IsAlreadyPaid;
            ModifyAndSave(bill);
            CompareDbBillWithExpectedBill(bill);

            bill.DueDate = bill.DueDate.AddDays(3);
            ModifyAndSave(bill);
            CompareDbBillWithExpectedBill(bill);

            bill.Type = BillType.MiscSpending;
            ModifyAndSave(bill);
            CompareDbBillWithExpectedBill(bill);
        }

        [TestMethod]
        public void TestSqlListBillReaderWithFilterById()
        {
            IList<BillInfo> bills = new List<BillInfo>();
            foreach (int expectedId in new List<int> { _currId + 1, _currId + 2, _currId + 3 })
            {
                BillInfo bill = GenerateRandomBillInfo(expectedId);
                AddToDb(bill);
                bills.Add(bill);
            }

            SqliteDbBillReaderWriter sqliteReader = new SqliteDbBillReaderWriter(_connString);
            IList<BillInfo> retBills = sqliteReader.GetBillByFilter(bill => bill.Id == _currId + 2);
            Assert.IsTrue(retBills[0].SameData(bills[1]));
            Assert.AreEqual(retBills[0].Id, bills[1].Id);

            IList<BillInfo> retBills2 = sqliteReader.GetBillByFilter(bill => bill.Id == -1);
            Assert.AreEqual(retBills2.Count, 0);
        }

        [TestMethod]
        public void TestSqlListBillReaderWithFilterByDateTime()
        {
            IList<BillInfo> bills = new List<BillInfo>();
            List<int> newIds = new List<int> { _currId + 1, _currId + 2, _currId + 3, _currId + 4 };
            IDictionary<int, DateTime> duedates = new Dictionary<int, DateTime>
            {
                {newIds[0] , new DateTime(1990, 10, 1) },
                {newIds[1] , new DateTime(1990, 10, 2) },
                {newIds[2] , new DateTime(1990, 10, 3) },
                {newIds[3] , new DateTime(1990, 10, 4) }
            };
            foreach (int expectedId in newIds)
            {
                BillInfo bill = GenerateRandomBillInfo(expectedId);
                bill.DueDate = duedates[expectedId];
                AddToDb(bill);
                bills.Add(bill);
            }

            SqliteDbBillReaderWriter sqliteReader = new SqliteDbBillReaderWriter(_connString);
            IList<BillInfo> retBills = sqliteReader.GetBillByFilter(
                bill => bill.DueDate >= new DateTime(1990, 10, 2) && bill.DueDate <= new DateTime(1990, 10, 3));

            Assert.AreEqual(retBills.Count, 2);
            Assert.AreEqual(retBills[0].Id, newIds[1]);
            Assert.AreEqual(retBills[1].Id, newIds[2]);
        }

        [TestMethod]
        public void TestSqlListBillReaderWithFilterByPaidStatus()
        {
            List<BillInfo> bills = new List<BillInfo>();
            List<int> newIds = new List<int> { _currId + 1, _currId + 2, _currId + 3, _currId + 4 };
            IDictionary<int, bool> paidstatus = new Dictionary<int, bool>
            {
                {newIds[0] , true },
                {newIds[1] , false },
                {newIds[2] , true },
                {newIds[3] , true }
            };
            foreach (int expectedId in newIds)
            {
                BillInfo bill = GenerateRandomBillInfo(expectedId);
                bill.IsAlreadyPaid = paidstatus[expectedId];
                AddToDb(bill);
                bills.Add(bill);
            }

            SqliteDbBillReaderWriter sqliteReader = new SqliteDbBillReaderWriter(_connString);
            IList<BillInfo> paidBills = sqliteReader.GetBillByFilter(bill => bill.IsAlreadyPaid);

            Assert.AreEqual(paidBills.Count, 3);
            Assert.AreEqual(paidBills[0].Id, newIds[0]);
            Assert.AreEqual(paidBills[1].Id, newIds[2]);
            Assert.AreEqual(paidBills[2].Id, newIds[3]);

            IList<BillInfo> unpaidBills = sqliteReader.GetBillByFilter(bill => !bill.IsAlreadyPaid);
            Assert.AreEqual(unpaidBills.Count, 1);
            Assert.AreEqual(unpaidBills[0].Id, newIds[1]);
        }

        [TestMethod]
        public void TestFindBillByIdNonExisting()
        {
            int id = _currId + 1;
            GenerateRandomBillInfo(id);
            using (DataAccessLayer dbAccess = new DataAccessLayer(_connString))
            {
                bool ret = dbAccess.FindBillById(id + 1, out BillInfo bill);
                Assert.IsFalse(ret);
                Assert.IsTrue(bill.SameData(new BillInfo()));
            }
        }
    }
}
