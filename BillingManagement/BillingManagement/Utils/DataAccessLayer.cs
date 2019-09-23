using BillingManagement.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;

namespace BillingManagement.Utils
{
    public class DataAccessLayer : DbContext, IDataProvider
    {
        private const string _connectionString = @"Data Source=C:\Users\Tuan D Tran\Desktop\CSharp\BillingManagement\BillingManagementTest\bin\Debug\db\bill.sqlite3;";

        public DataAccessLayer() : base()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_connectionString);
        }

        public DbSet<BillInfo> BillInfos { get; set; }

        public bool AddData(object obj)
        {
            if (obj is BillInfo)
            {
                BillInfos.Add(obj as BillInfo);
                SaveChanges();
                return true;
            }
            return false;
        }

        public bool FindData(int id, out object output)
        {
            IQueryable<BillInfo> setBill = BillInfos.Where(b => b.Id == id);
            if (setBill.Count() == 0)
            {
                output = null;
                return false;
            }

            output = setBill.First();
            return true;
        }

        public bool ModifyData(int id, object obj)
        {
            if(obj is BillInfo && FindData(id, out object dbObj))
            {
                BillInfo existingBillInfo = dbObj as BillInfo;
                BillInfo inComingBillInfo = obj as BillInfo;
                existingBillInfo.Amount = inComingBillInfo.Amount;
                existingBillInfo.Description = inComingBillInfo.Description;
                existingBillInfo.DueDate = inComingBillInfo.DueDate;
                existingBillInfo.BillName = inComingBillInfo.BillName;
                SaveChanges();
                return true;
            }
            return false;
        }
    }
}
