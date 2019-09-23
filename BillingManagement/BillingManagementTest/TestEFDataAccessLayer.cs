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

    [TestClass]
    public class TestEFDataAccessLayer
    {
        private string _connString = @"Data Source=.\billTestWriteRead.sqlite3;";
        private int _currId = 1;

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

        [TestMethod]
        public void TestWriteRead()
        {
            foreach (int expectedId in new List<int> { _currId + 1, _currId + 2, _currId + 3 })
            {
                Random rnd = new Random();
                BillType type = (BillType)rnd.Next(0, 4);
                double amount = Math.Round(rnd.NextDouble(), 2);
                string description = $"test description {expectedId}";
                string name = $"test name {expectedId}";
                DateTime dueDate = new DateTime(rnd.Next(2010, 2020), rnd.Next(1, 12), rnd.Next(1, 20));

                using (DataAccessLayer dbAccess = new DataAccessLayer(_connString))
                {
                    BillInfo bill = new BillInfo
                    {
                        Type = type,
                        BillName = name,
                        DueDate = dueDate,
                        Amount = amount,
                        Description = description
                    };

                    dbAccess.AddAndSave(bill);
                }

                using (DataAccessLayer dbAccess2 = new DataAccessLayer(_connString))
                {
                    dbAccess2.FindData(expectedId, out object obj);
                    Assert.IsTrue(obj is BillInfo);
                    BillInfo bill = obj as BillInfo;
                    Assert.AreEqual(bill.BillName, name);
                    Assert.AreEqual(bill.Type, type);
                    Assert.AreEqual(bill.DueDate, dueDate);
                    Assert.AreEqual(bill.Description, description);
                    Assert.AreEqual(bill.Id, expectedId);
                }
            }
        }
    }
}
