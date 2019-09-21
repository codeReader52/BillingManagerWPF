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
            BillInfo bill1 = new BillInfo(BillType.Food, new DateTime(2019, 1, 1), 2.3, "Test description food");
            bill1.Id = 1;
            BillInfo bill2 =  new BillInfo(BillType.Utility, new DateTime(2019, 2, 5), 4.5, "Test description utility");
            bill2.Id = 2;
            BillInfo bill3 = new BillInfo(BillType.MiscSpending, new DateTime(2019, 3, 2), 6.7, "Test description misc");
            bill3.Id = 3;
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
