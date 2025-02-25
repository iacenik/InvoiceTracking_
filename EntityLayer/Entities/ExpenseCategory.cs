using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityLayer.Entities
{
    public class ExpenseCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(100)]
        public string CategoryName { get; set; } = string.Empty; // 🔍 Boş değer olmaması için varsayılan değer

        [MaxLength(300)]
        public string? Description { get; set; } // Açıklama (opsiyonel)

        // 🔗 Giderlerle ilişki
        public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();

        // 🔗 Faturalar ile ilişki
        public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
    }
}
