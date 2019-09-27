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
        public DateTime DueDate { get; set; }
        [Required]
        public double Amount { get; set; }
        [Required]
        public string BillName { get; set; }

        [Key]
        [DatabaseGenerat‌ed(DatabaseGeneratedOp‌​tion.Identity)]
        public int Id { get; set; } = 0;

        public BillType Type { get; set; }
        public string Description { get; set; }

        [Required]
        public bool IsAlreadyPaid { get; set; }
        public byte[] Attachement { get; set; }

        public bool SameData(BillInfo other)
        {
            return Type == other.Type &&
                Description == other.Description &&
                BillName == other.BillName &&
                Amount == other.Amount &&
                DueDate == other.DueDate;
        }
    }
}
