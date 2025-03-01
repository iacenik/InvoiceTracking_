using EntityLayer.Entities;
using System.ComponentModel.DataAnnotations;
using static EntityLayer.Entities.Enums;

namespace InvoiceTracking.Models
{
    public class PaymentDetailViewModel
    {
        public int PaymentDetailId { get; set; }

        [Required]
        public int PaymentId { get; set; }

        [Required]
        public CurrencyType Currency { get; set; }

        [Required(ErrorMessage = "En az bir ürün eklemelisiniz.")]
        public List<SoldProduct> Products { get; set; } = new List<SoldProduct>();

        public Payment? Payment { get; set; }

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
