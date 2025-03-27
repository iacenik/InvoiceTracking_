using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json;
using static EntityLayer.Entities.Enums;
using EntityLayer.Entities; // 🔹 SoldProduct buradan geliyor!

namespace EntityLayer.Entities
{
    public class PaymentDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PaymentDetailId { get; set; }

        [Required]
        public int PaymentId { get; set; }

        [ForeignKey("PaymentId")]
        public virtual Payment? Payment { get; set; }

        public string SoldProducts { get; set; } = "[]"; // 📌 JSON olarak saklanan ürünler

        [Required]
        public CurrencyType Currency { get; set; } = CurrencyType.EUR;

        [NotMapped]
        public List<SoldProduct> SoldProductsList => string.IsNullOrWhiteSpace(SoldProducts)
            ? new List<SoldProduct>()
            : JsonSerializer.Deserialize<List<SoldProduct>>(SoldProducts) ?? new List<SoldProduct>();

        [NotMapped]
        public decimal TotalAmount => SoldProductsList.Sum(p => p.TotalPrice);

        public void SetSoldProducts(List<SoldProduct> products)
        {
            SoldProducts = JsonSerializer.Serialize(products);
        }
    }
}
