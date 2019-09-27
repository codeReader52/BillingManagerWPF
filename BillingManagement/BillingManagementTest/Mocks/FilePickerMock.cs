using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BillingManagement.Utils;

namespace BillingManagementTest.Mocks
{
    internal class FilePickerMock : IPopUpWinService<string, byte[]>
    {
        public string Input { get; set; }
        public byte[] Output { get; set; }

        public void DoModal()
        {
            return; // do nothing
        }
    }
}
