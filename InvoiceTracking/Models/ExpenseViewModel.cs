using EntityLayer.Entities;
using static EntityLayer.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace InvoiceTracking.Models
{
    public class ExpenseViewModel
    {
        public int ExpenseId { get; set; }

        [Required(ErrorMessage = "Tutar zorunludur.")]
        [Display(Name = "Tutar")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Tutar 0'dan büyük olmalıdır.")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Tarih zorunludur.")]
        [Display(Name = "Tarih")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Kategori seçimi zorunludur.")]
        [Display(Name = "Kategori")]
        public int CategoryId { get; set; }

        [Display(Name = "Açıklama")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Para birimi seçimi zorunludur.")]
        [Display(Name = "Para Birimi")]
        public CurrencyType Currency { get; set; }

        [Required(ErrorMessage = "Personel seçimi zorunludur.")]
        [Display(Name = "Personel")]
        public int EmployeeId { get; set; }

        // İlişkili nesneler
        public ExpenseCategory? Category { get; set; }
        public Employee? Employee { get; set; }
    }
}
