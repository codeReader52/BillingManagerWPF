using BillingManagement.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using System.Collections.Generic;

namespace BillingManagement.Utils
{
    public class DataAccessLayer : DbContext
    {
        private string _connectionString = "";
        public DataAccessLayer(string connectionString) : base()
        {
            _connectionString = connectionString;
        }
        public DataAccessLayer(): this(Constants.CONN_STRING)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_connectionString);
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<BillInfo> BillInfos { get; set; }

        private bool Safe_SaveChange()
        {
            try
            {
                SaveChanges();
                return true;
            }
            catch // horrible handling of exception here, dodgy jobs...
            {
                return false;
            }
        }

        public bool AddAndSave(object obj)
        {
            if (obj is BillInfo)
            {
                BillInfos.Add(obj as BillInfo);
                return Safe_SaveChange();
            }
            return false;
        }

        public bool FindData(Func<BillInfo, bool> filter, out IList<BillInfo> output)
        {
            IEnumerable<BillInfo> setBill = BillInfos.Where(filter);
            if (setBill.Count() == 0)
            {
                output = new List<BillInfo>();
                return false;
            }

            output = setBill.ToList();
            return true;
        }

        public bool FindBillById(int id, out BillInfo bill)
        {
            IQueryable<BillInfo> bills = BillInfos.Where(_bill => _bill.Id == id);
            bill = new BillInfo();
            if (bills.Count() > 0)
            {
                bill = bills.First();
                return true;
            }
            return false;
        }

        public bool ModifyAndSave(BillInfo inComingBillInfo)
        {
            if(FindBillById(inComingBillInfo.Id, out BillInfo existingBillInfo))
            {
                existingBillInfo.Amount = inComingBillInfo.Amount;
                existingBillInfo.Description = inComingBillInfo.Description;
                existingBillInfo.DueDate = inComingBillInfo.DueDate;
                existingBillInfo.BillName = inComingBillInfo.BillName;
                return Safe_SaveChange();
            }
            else
            {
                return false;
            }
        }
    }
}
