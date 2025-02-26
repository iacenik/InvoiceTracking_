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
    public class InvoiceDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InvoiceDetailId { get; set; }

        [Required]
        public int InvoiceId { get; set; }
        [ForeignKey("InvoiceId")]
        public virtual Invoice? Invoice { get; set; }

        public string SoldProducts { get; set; } = "[]"; // JSON olarak saklanan ürünler

        [Required]
        public CurrencyType Currency { get; set; } = CurrencyType.EUR;

        [NotMapped]
        public decimal TotalAmount { get; private set; } = 0m;

        // 📌 JSON'dan deserialize edilen ürün listesi
        [NotMapped]
        public List<SoldProduct> SoldProductsList => string.IsNullOrWhiteSpace(SoldProducts)
            ? new List<SoldProduct>()
            : JsonSerializer.Deserialize<List<SoldProduct>>(SoldProducts) ?? new List<SoldProduct>();

        // 📌 Fatura detayının toplam tutarını hesaplar
        [NotMapped]
        public decimal TotalPrice => SoldProductsList.Sum(p => p.TotalPrice);

        public void CalculateTotalAmount()
        {
            try
            {
                TotalAmount = SoldProductsList.Sum(p => p.TotalPrice);
            }
            catch (JsonException)
            {
                TotalAmount = 0;
            }
        }

        public void SetSoldProducts(List<SoldProduct> products)
        {
            SoldProducts = JsonSerializer.Serialize(products);
            CalculateTotalAmount();
        }
    }
}
