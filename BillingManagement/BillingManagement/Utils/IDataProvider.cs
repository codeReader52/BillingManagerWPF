﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingManagement.Utils
{
    public interface IDataProvider
    {
        bool FindData(int id, out object output);
        bool ModifyData(int id, object obj);
        bool AddData(object obj);
    }
}