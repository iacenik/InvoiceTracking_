using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityLayer.Entities
{
    public class CashRegister
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CashRegisterId { get; set; }

        // 🔹 EUR Cinsinden Toplam Gelir ve Gider
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalIncomeEUR { get; set; } = 0m; // ✅ Varsayılan değer eklendi

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalExpenseEUR { get; set; } = 0m;

        // 🔹 RON Cinsinden Toplam Gelir ve Gider
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalIncomeRON { get; set; } = 0m;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalExpenseRON { get; set; } = 0m;

        // 🔹 USD Cinsinden Toplam Gelir ve Gider
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalIncomeUSD { get; set; } = 0m;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalExpenseUSD { get; set; } = 0m;

        // 🔹 Mevcut Kasa Bakiye Hesaplamaları
        [NotMapped]
        public decimal BalanceEUR => TotalIncomeEUR - TotalExpenseEUR;

        [NotMapped]
        public decimal BalanceRON => TotalIncomeRON - TotalExpenseRON;

        [NotMapped]
        public decimal BalanceUSD => TotalIncomeUSD - TotalExpenseUSD;
    }
}
