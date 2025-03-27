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
            if (products == null || !products.Any())
            {
                SoldProducts = "[]";
                TotalAmount = 0;
                return;
            }

            // Ürünlerin para birimini kontrol et ve güncelle
            foreach (var product in products)
            {
                if (product.Currency != Currency)
                {
                    product.Currency = Currency;
                }
                Console.WriteLine($"Setting product in InvoiceDetail: Name={product.ProductName}, Price={product.UnitPrice}, Currency={product.Currency}, Total={product.TotalPrice}");
            }

            SoldProducts = JsonSerializer.Serialize(products);
            Console.WriteLine($"Serialized products: {SoldProducts}");

            try
            {
                TotalAmount = products.Sum(p => p.TotalPrice);
                Console.WriteLine($"Calculated TotalAmount: {TotalAmount}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calculating total amount: {ex.Message}");
                TotalAmount = 0;
            }
        }
    }
}
