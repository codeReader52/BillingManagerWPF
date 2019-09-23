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
        public BillType Type { get; set; }
        [Required]
        public DateTime DueDate { get; set; }
        [Required]
        public double Amount { get; set; }
        public string Description { get; set; }
        [Required]
        public string BillName { get; set; }

        [Key]
        [DatabaseGenerat‌ed(DatabaseGeneratedOp‌​tion.Identity)]
        public int Id { get; set; }
    }
}
