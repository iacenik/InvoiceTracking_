using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static EntityLayer.Entities.Enums;
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
        public decimal TotalIncomeEUR { get; set; } = 0m;
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

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public CurrencyType Currency { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public TransactionType TransactionType { get; set; }

        // ✅ **Kasadan gider düşen yeni metot**
        public void DeductExpense(decimal amount, CurrencyType currency)
        {
            Amount = amount;
            Currency = currency;
            TransactionType = TransactionType.Expense;
            Date = DateTime.Now;
            Description = $"Gider: {amount} {currency}";

            switch (currency)
            {
                case CurrencyType.EUR:
                    TotalExpenseEUR += amount;
                    break;
                case CurrencyType.RON:
                    TotalExpenseRON += amount;
                    break;
                case CurrencyType.USD:
                    TotalExpenseUSD += amount;
                    break;
            }
        }

        // ✅ **Kasaya gelir ekleyen yeni metot**
        public void AddIncome(decimal amount, CurrencyType currency)
        {
            Amount = amount;
            Currency = currency;
            TransactionType = TransactionType.Income;
            Date = DateTime.Now;
            Description = $"Gelir: {amount} {currency}";

            switch (currency)
            {
                case CurrencyType.EUR:
                    TotalIncomeEUR += amount;
                    break;
                case CurrencyType.RON:
                    TotalIncomeRON += amount;
                    break;
                case CurrencyType.USD:
                    TotalIncomeUSD += amount;
                    break;
            }
        }

        // ✅ **Kasadan gelir düşen yeni metot**
        public void DeductIncome(decimal amount, CurrencyType currency)
        {
            Amount = amount;
            Currency = currency;
            TransactionType = TransactionType.Expense;
            Date = DateTime.Now;
            Description = $"Gelir İptali: {amount} {currency}";

            switch (currency)
            {
                case CurrencyType.EUR:
                    TotalIncomeEUR -= amount;
                    break;
                case CurrencyType.RON:
                    TotalIncomeRON -= amount;
                    break;
                case CurrencyType.USD:
                    TotalIncomeUSD -= amount;
                    break;
            }
        }
    }
}