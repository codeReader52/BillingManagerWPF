using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BillingManagement.Model;

namespace BillingManagement.Utils
{
    public class SqliteDbBillReaderWriter : IBillReaderWriter
    {
        private string _connString = "";
        public SqliteDbBillReaderWriter(string conn)
        {
            _connString = conn;
        }
        public IList<BillInfo> GetAllBills()
        {
            using (DataAccessLayer dbAccess = new DataAccessLayer(_connString))
            {
                return dbAccess.BillInfos.ToList();
            }
        }

        public bool Record(BillInfo bill, out string errorString)
        {
            if (bill == null)
            {
                errorString = "Cannot save null bills";
                return false;
            }

            if (bill.Id == 0)
            {
                using(DataAccessLayer dbAccess = new DataAccessLayer(_connString))
                {
                    errorString = "";
                    return dbAccess.AddAndSave(bill);
                }
            }
            else
            {
                using(DataAccessLayer dbAccess = new DataAccessLayer(_connString))
                {
                    errorString = "";
                    return dbAccess.ModifyAndSave(bill);
                }
            }
        }

        public IList<BillInfo> GetBillByFilter(Func<BillInfo, bool> filter)
        {
            using (DataAccessLayer dbAccess = new DataAccessLayer(_connString))
            {
                if(dbAccess.FindData(filter, out IList<BillInfo> outList))
                {
                    return outList;
                }
                else
                {
                    return new List<BillInfo>();
                }
            }
        }
    }
}
