using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityLayer.Entities
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; } // Ürünün benzersiz kimliği

        [Required]
        [MaxLength(200)]
        public string ProductName { get; set; } = string.Empty; // Ürün adı (Örn: Plintus 60mm, Masa)

        [MaxLength(500)]
        public string? Description { get; set; } // Ürün açıklaması (Opsiyonel)

        // 🔗 Ödeme detayları ile ilişki (One-to-Many)
        
    }
}
