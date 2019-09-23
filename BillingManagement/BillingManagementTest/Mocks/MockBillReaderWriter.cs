using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BillingManagement.Model;
using BillingManagement.Utils;

namespace BillingManagementTest.Mocks
{
    class MockBillReaderWriter : IBillReaderWriter
    {
        public BillInfo BillSaved = null;
        public IList<BillInfo> GetAllBills()
        {
            BillInfo bill1 = new BillInfo
            {
                Type = BillType.Food,
                DueDate = new DateTime(2019, 1, 1),
                Amount = 2.3,
                Description = "Test description food",
                BillName = "Test name 1",
                Id = 1
            };
            BillInfo bill2 =  new BillInfo
            {
                Type = BillType.Utility,
                DueDate = new DateTime(2019, 2, 5),
                Amount = 4.5,
                Description = "Test description utility",
                BillName = "Test name 2",
                Id = 2
            };
            BillInfo bill3 = new BillInfo
            {
                Type = BillType.MiscSpending,
                DueDate = new DateTime(2019, 3, 2),
                Amount = 6.7,
                Description = "Test description misc",
                BillName = "Test name 3",
                Id = 3
            };
            return new List<BillInfo> { bill1, bill2, bill3 };
        }

        public bool Record(BillInfo bill, out string errorString)
        {
            errorString = "";
            BillSaved = bill;
            return true;
        }
    }
}
