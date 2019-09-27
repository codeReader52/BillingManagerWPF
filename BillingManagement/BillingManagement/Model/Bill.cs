using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BillingManagement.Model
{
    public enum BillType
    {
        Unknown,
        Utility,
        Food,
        MiscSpending,
    }

    [Table("BillInfos")]
    public class BillInfo
    {
        [Required]
        public DateTime DueDate { get; set; } = new DateTime();
        [Required]
        public double Amount { get; set; } = 0.0;
        [Required]
        public string BillName { get; set; } = "";

        [Key]
        [DatabaseGenerat‌ed(DatabaseGeneratedOp‌​tion.Identity)]
        public int Id { get; set; } = 0;

        public BillType Type { get; set; } = BillType.Unknown;
        public string Description { get; set; } = "";

        [Required]
        public bool IsAlreadyPaid { get; set; } = false;
        public byte[] Attachement { get; set; } = new byte[0];

        public bool SameData(BillInfo other)
        {
            if (this == null && other == null)
                return true;

            if (this == null && other != null)
                return false;

            if (this != null && other == null)
                return false;
            
            // both are not null
            if (Attachement.Length != other.Attachement.Length)
                return false;

            for (int i = 0; i < Attachement.Length; i++)
                if (Attachement[i] != other.Attachement[i])
                    return false;

            return Type == other.Type &&
                Description == other.Description &&
                BillName == other.BillName &&
                Amount == other.Amount &&
                DueDate == other.DueDate &&
                IsAlreadyPaid == other.IsAlreadyPaid;
        }

        internal void SetDataFrom(BillInfo inComingBillInfo)
        {
            Amount = inComingBillInfo.Amount;
            BillName = inComingBillInfo.BillName;
            Description = inComingBillInfo.Description;
            DueDate = inComingBillInfo.DueDate;
            IsAlreadyPaid = inComingBillInfo.IsAlreadyPaid;
            Type = inComingBillInfo.Type;

        }
    }
}
