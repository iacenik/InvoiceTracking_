using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static EntityLayer.Entities.Enums; // Enums sınıfındaki CurrencyType enum'unu kullanmak için

namespace EntityLayer.Entities
{
    public class Expense
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ExpenseId { get; set; }

        [Required]
        public decimal Amount { get; set; } = 0m; // Tutar

        [Required]
        public DateTime Date { get; set; } = DateTime.Now; // Giderin tarihi

        [Required]
        public int CategoryId { get; set; } // Giderin kategorisi

        [ForeignKey("CategoryId")]
        public virtual ExpenseCategory? Category { get; set; } // 🔗 Gider kategorisi ile ilişki

        public string? Description { get; set; } // Gider açıklaması

        [Required]
        public CurrencyType Currency { get; set; } // Ödeme türü (RON, EUR, TL, USD)

        [Required]
        public int EmployeeId { get; set; } // Gideri yapan kişinin ID'si

        [ForeignKey("EmployeeId")]
        public virtual Employee? Employee { get; set; } // 🔗 Gideri yapan kişi ile ilişki
    }
}