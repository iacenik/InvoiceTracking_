using EntityLayer.Entities;
using static EntityLayer.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace InvoiceTracking.Models
{
    public class PaymentViewModel
    {
        public int PaymentId { get; set; }

        [Required(ErrorMessage = "Müşteri seçimi zorunludur.")]
        [Display(Name = "Müşteri")]
        public int ClientId { get; set; }

        [Required(ErrorMessage = "Para birimi seçimi zorunludur.")]
        [Display(Name = "Para Birimi")]
        public string CurrencyString { get; set; }

        [Display(Name = "Para Birimi")]
        public CurrencyType Currency 
        { 
            get => Enum.TryParse<CurrencyType>(CurrencyString, out var result) ? result : CurrencyType.RON;
            set => CurrencyString = value.ToString();
        }

        [Required(ErrorMessage = "Ödeme tarihi zorunludur.")]
        [Display(Name = "Ödeme Tarihi")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; } = DateTime.Now;

        [Display(Name = "Açıklama")]
        public string? Description { get; set; }

        // Satılan ürünler listesi
        [Display(Name = "Ürünler")]
        public List<SoldProduct> Products { get; set; } = new List<SoldProduct>();

        // İlişkili nesneler
        public Client? Client { get; set; }
        public List<PaymentDetail> PaymentDetails { get; set; } = new List<PaymentDetail>();

        // Toplam tutar
        [Display(Name = "Toplam Tutar")]
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
