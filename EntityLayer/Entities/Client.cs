using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityLayer.Entities
{
    public class Client
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClientId { get; set; }

        [Required]
        public int ClientCode { get; set; }

        [Required]
        [MaxLength(100)]
        public string CompanyName { get; set; } = string.Empty; // Şirketin adı

        [Required]
        [MaxLength(100)]
        public string Country { get; set; } = string.Empty; // Şirketin bulunduğu ülke

        [MaxLength(300)]
        public string? Address { get; set; } // Şirketin açık adresi

        [MaxLength(100)]
        public string? BankName { get; set; } // Banka adı

        [MaxLength(34)]
        public string? Iban { get; set; } // IBAN numarası

        public DateTime CreatedAt { get; set; } = DateTime.Now; // Müşterinin eklendiği tarih

        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
    }
}
