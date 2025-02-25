using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json;
using static EntityLayer.Entities.Enums;

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

        public string SoldProducts { get; set; } = "[]"; 
        [Required]
        public CurrencyType Currency { get; set; } 

     
        [NotMapped]
        public decimal TotalAmount { get; private set; }

        public void CalculateTotalAmount()
        {
            var products = JsonSerializer.Deserialize<List<SoldProduct>>(SoldProducts) ?? new List<SoldProduct>();
            TotalAmount = products.Sum(p => p.TotalPrice); 
        }
    }

    public class SoldProduct
    {
        public int ProductId { get; set; } 
        public string ProductName { get; set; } = string.Empty; 
        public decimal UnitPrice { get; set; } 
        public int Quantity { get; set; }
        public decimal TotalPrice => UnitPrice * Quantity; 

        public CurrencyType Currency { get; set; }
    }
}
