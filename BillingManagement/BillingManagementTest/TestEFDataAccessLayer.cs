using BillingManagement.Model;
using BillingManagement.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingManagementTest
{
    [TestClass]
    public class TestEFDataAccessLayer
    {
        [TestMethod]
        public void TestWrite()
        {
            using (DataAccessLayer dbAccess = new DataAccessLayer())
            {
                BillInfo bill = new BillInfo(BillType.MiscSpending, new DateTime(2019, 3, 10), 2.3, "test description");
                bill.BillName = "Test bill name";
                dbAccess.AddData(bill);
            }

        }
    }
}
