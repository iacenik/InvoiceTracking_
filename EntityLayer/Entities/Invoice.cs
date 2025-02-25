using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq; 
using static EntityLayer.Entities.Enums; 

namespace EntityLayer.Entities
{
    public class Invoice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InvoiceId { get; set; }

        [Required]
        public DateTime Date { get; set; } = DateTime.Now; // Fatura tarihi

        [Required]
        public int CategoryId { get; set; } // Gider kategorisi
        [ForeignKey("CategoryId")]
        public virtual ExpenseCategory? Category { get; set; } // 🔗 Gider kategorisi ile bağlantı

        [Required]
        public int EmployeeId { get; set; } // Harcamayı yapan çalışan
        [ForeignKey("EmployeeId")]
        public virtual Employee? Employee { get; set; } 

        public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; } = new List<InvoiceDetail>(); // 🔗 Fatura detayları

        [NotMapped] // Veritabanına kaydedilmez
        public decimal TotalAmount => InvoiceDetails?.Sum(d => d.TotalPrice) ?? 0; 

        [Required]
        public CurrencyType Currency { get; set; } // 🔗 Para birimi (RON, EUR, USD, TL)

        [Required]
        public InvoiceType InvoiceType { get; set; } // 🔗 Fatura Tipi (A, B, C)

        public bool IsPaid { get; set; } = false; // 🔍 Fatura ödendi mi?

        [Required]
        public int ClientId { get; set; }
        [ForeignKey("ClientId")]
        public virtual Client? Client { get; set; }
    }
}
