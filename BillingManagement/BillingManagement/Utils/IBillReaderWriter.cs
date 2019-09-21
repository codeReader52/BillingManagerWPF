using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BillingManagement.Model;

namespace BillingManagement.Utils
{
    public interface IBillReaderWriter
    {
        IList<BillInfo> GetAllBills();
        bool Record(BillInfo bill, out string errorString);
    }
}
