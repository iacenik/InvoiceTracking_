using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static EntityLayer.Entities.Enums; // CurrencyType enum'unu kullanabilmek için

namespace EntityLayer.Entities
{
    public class InvoiceDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InvoiceDetailId { get; set; }

        [Required]
        public int InvoiceId { get; set; }
        [ForeignKey("InvoiceId")]
        public virtual Invoice? Invoice { get; set; } // 🔗 Fatura ile ilişki

        [Required]
        public int ItemId { get; set; }
        [ForeignKey("ItemId")]
        public virtual Item? Item { get; set; } // 🔗 Ürün ile ilişki

        [Required]
        public int Quantity { get; set; } // 🔍 Ürün adedi

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; } // 🔍 Kullanıcı manuel olarak fiyatı girer

        [Required]
        public CurrencyType Currency { get; set; } // 🔍 Para birimi (RON, EUR, USD, TL)

        [NotMapped] // 🔍 Veritabanına kaydedilmesini engeller
        public decimal TotalPrice { get; private set; } // 🔍 `private set` ekledik!

        public void CalculateTotalPrice() // 🔍 Setter olmadığı için hesaplamayı burada yapıyoruz
        {
            TotalPrice = UnitPrice * Quantity;
        }
    }
}
