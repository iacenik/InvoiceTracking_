using EntityLayer.Entities;
using static EntityLayer.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace InvoiceTracking.Models
{
    public class InvoiceViewModel
    {
        public int InvoiceId { get; set; }

        [Required(ErrorMessage = "Fatura tarihi zorunludur.")]
        [Display(Name = "Fatura Tarihi")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Kategori seçimi zorunludur.")]
        [Display(Name = "Kategori")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Çalışan seçimi zorunludur.")]
        [Display(Name = "Personel")]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Müşteri seçimi zorunludur.")]
        [Display(Name = "Müşteri")]
        public int ClientId { get; set; }

        [Required(ErrorMessage = "Para birimi seçimi zorunludur.")]
        [Display(Name = "Para Birimi")]
        public CurrencyType Currency { get; set; }

        [Required(ErrorMessage = "Fatura tipi seçimi zorunludur.")]
        [Display(Name = "Fatura Tipi")]
        public InvoiceType InvoiceType { get; set; }

        [Display(Name = "Ödendi")]
        public bool IsPaid { get; set; } = false;

        // Ürünler listesi
        [Display(Name = "Ürünler")]
        public List<SoldProduct> Products { get; set; } = new List<SoldProduct>();

        // İlişkili nesneler
        public ExpenseCategory Category { get; set; }
        public Employee Employee { get; set; }
        public Client Client { get; set; }
        public List<InvoiceDetail> InvoiceDetails { get; set; } = new List<InvoiceDetail>();

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
