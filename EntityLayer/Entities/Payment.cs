using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using static EntityLayer.Entities.Enums;

namespace EntityLayer.Entities
{
    public class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PaymentId { get; set; }

        [Required]
        public int ClientId { get; set; } // Ödemeyi yapan müşteri

        [ForeignKey("ClientId")]
        public virtual Client? Client { get; set; } // 🔗 Müşteri ile ilişki

        [Required]
        public CurrencyType Currency { get; set; } // 🔍 Kullanıcı seçmeli (RON, EUR, USD, TL)

        public DateTime Date { get; set; } = DateTime.Now; // Ödeme tarihi

        [StringLength(500)]
        public string? Description { get; set; } // Açıklama (opsiyonel)

        // 🔗 Ödeme Detayları ile ilişki
        public virtual ICollection<PaymentDetail> PaymentDetails { get; set; } = new List<PaymentDetail>();

        // 🔍 `Amount` otomatik hesaplanıyor (PaymentDetail toplamı)
        [NotMapped]
        public decimal TotalAmount => PaymentDetails?.Sum(pd => pd.TotalAmount) ?? 0;
    }
}
