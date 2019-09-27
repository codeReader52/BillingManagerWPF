using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingManagement.Utils
{
    public interface IPopUpWinService<I, O>
    {
        I Input { get; set; }
        O Output { get; set; }
        void DoModal();
    }
}
