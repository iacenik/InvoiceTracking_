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
        public virtual ExpenseCategory? Category { get; set; }

        [Required]
        public int EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public virtual Employee? Employee { get; set; }

        public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; } = new List<InvoiceDetail>();

        [NotMapped]
        public decimal TotalAmount => InvoiceDetails?.Sum(d => d.TotalPrice) ?? 0;

        [Required]
        public CurrencyType Currency { get; set; }

        [Required]
        public InvoiceType InvoiceType { get; set; }

        public bool IsPaid { get; set; } = false;

        [Required]
        public int ClientId { get; set; }
        [ForeignKey("ClientId")]
        public virtual Client? Client { get; set; }

        // ✅ Fatura onaylandığında kasadan düş
        public void ApproveInvoice(CashRegister cashRegister)
        {
            if (!IsPaid) // Eğer zaten ödendiyse tekrar düşmesin
            {
                cashRegister.DeductExpense(TotalAmount, Currency); // ✅ Kasadan düş
                IsPaid = true; // ✅ Fatura ödendi olarak işaretlendi
            }
        }
    }
}
