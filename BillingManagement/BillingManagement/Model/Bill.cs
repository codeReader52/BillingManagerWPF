using System;

namespace BillingManagement.Model
{
    public enum BillType
    {
        Unknown,
        Utility,
        Food,
        MiscSpending,
    }

    public class BillInfo
    {

        public BillType Type { get; set; }
        public DateTime DueDate { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
        public string BillName { get; set; }
        public int Id { get; set; }

        public BillInfo(BillType type, DateTime dueDate, double amount, string description)
        {
            Type = type;
            DueDate = dueDate;
            Amount = amount;
            Description = description;
        }
    }
}
