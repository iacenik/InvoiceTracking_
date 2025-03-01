using EntityLayer.Entities;
using System.ComponentModel.DataAnnotations;
using static EntityLayer.Entities.Enums;

namespace InvoiceTracking.Models
{
    public class InvoiceDetailViewModel
    {
        public int InvoiceDetailId { get; set; }

        [Required]
        public int InvoiceId { get; set; }

        [Required]
        public CurrencyType Currency { get; set; }

        [Required(ErrorMessage = "En az bir ürün eklemelisiniz.")]
        public List<SoldProduct> Products { get; set; } = new List<SoldProduct>();

        public Invoice? Invoice { get; set; }

        public decimal TotalAmount
        {
            get
            {
                decimal total = 0;
                if (Products != null)
                {
                    foreach (var product in Products)
                    {
                        total += product.TotalPrice;
                    }
                }
                return total;
            }
        }
    }
}
