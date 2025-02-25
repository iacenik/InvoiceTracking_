using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityLayer.Entities
{
    public class Item
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemId { get; set; } // Ürünün benzersiz kimliği

        [Required]
        [MaxLength(200)]
        public string ItemName { get; set; } = string.Empty; // Ürün adı (Örn: Toner, Masa)

        public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; } = new List<InvoiceDetail>(); // Fatura detayları ile ilişki
    }
}
