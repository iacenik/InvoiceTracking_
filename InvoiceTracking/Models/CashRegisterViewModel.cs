using static EntityLayer.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace InvoiceTracking.Models
{
    public class CashRegisterViewModel
    {
        [Required(ErrorMessage = "Tutar zorunludur.")]
        [Display(Name = "Tutar")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Tutar 0'dan büyük olmalıdır.")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Para birimi seçimi zorunludur.")]
        [Display(Name = "Para Birimi")]
        public CurrencyType Currency { get; set; }

        [Display(Name = "Açıklama")]
        public string? Description { get; set; }
    }
}
