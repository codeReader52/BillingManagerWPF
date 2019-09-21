using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingManagement.Utils
{
    class SequenceManager : ISequenceManager
    {
        private readonly object _lockObj = new object();
        private int _index = 0;
        public SequenceManager(int startIndex)
        {
            _index = startIndex;
        }

        public int SyncIncreaseIndex()
        {
            int newIndex = -1;
            lock(_lockObj)
            {
                _index += 1;
                newIndex = _index;
            }
            return newIndex;
        }
        
        public int GetCurrIndex()
        {
            return _index;
        }
    }
}
