using BillingManagement.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;

namespace BillingManagement.Utils
{
    public class DataAccessLayer : DbContext, IDataProvider
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

        public bool AddAndSave(object obj)
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

        public bool ModifyAndSave(int id, object obj)
        {
            if(obj is BillInfo && FindData(id, out object dbObj))
            {
                if (dbObj == null)
                {
                    return false;
                }

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
