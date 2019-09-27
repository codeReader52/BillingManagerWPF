using BillingManagement.Model;
using BillingManagement.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;

namespace BillingManagementTest
{
    // not ideal for unittest, but we need to test the data access layer
    // ignore most of the times, until we need to specifically test it
    [Ignore]
    [TestClass]
    public class TestEFDataAccessLayer
    {
        private string _connString = @"Data Source=.\billTestWriteRead.sqlite3;";
        private int _currId = 1;

        public object SqlLiteDbBillReaderWriter { get; private set; }

        [TestInitialize]
        public void SetUp()
        {
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
            return new BillInfo
            {
                Type = type,
                BillName = name,
                DueDate = dueDate,
                Amount = amount,
                Description = description
            };
        }

        [TestMethod]
        public void TestWriteRead()
        {
            foreach (int expectedId in new List<int> { _currId + 1, _currId + 2, _currId + 3 })
            {
                BillInfo bill = GenerateRandomBillInfo(expectedId);
                using (DataAccessLayer dbAccess = new DataAccessLayer(_connString))
                {
                    dbAccess.AddAndSave(bill);
                }

                using (DataAccessLayer dbAccess2 = new DataAccessLayer(_connString))
                {
                    dbAccess2.FindBillById(expectedId, out BillInfo actualBill);
                    Assert.IsTrue(actualBill.SameData(bill));
                    Assert.AreEqual(actualBill.Id, expectedId);
                }
            }
        }

        [TestMethod]
        public void TestSqlListBillReaderWithFilter()
        {
            IList<BillInfo> bills = new List<BillInfo>();
            foreach (int expectedId in new List<int> { _currId + 1, _currId + 2, _currId + 3 })
            {
                BillInfo bill = GenerateRandomBillInfo(expectedId);
                using (DataAccessLayer dbAccess = new DataAccessLayer(_connString))
                {
                    dbAccess.AddAndSave(bill);
                }
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
