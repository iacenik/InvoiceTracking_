using EntityLayer.Entities;
using static EntityLayer.Entities.Enums;

namespace InvoiceTracking.Models
{
    public class ClientViewModel
    {
        public Client Client { get; set; }
        public IEnumerable<Invoice> Invoices { get; set; } = new List<Invoice>();
        public IEnumerable<Payment> Payments { get; set; } = new List<Payment>();

        // Özet veriler
        public decimal TotalInvoiceAmountEUR => CalculateTotalInvoiceAmount(CurrencyType.EUR);
        public decimal TotalInvoiceAmountRON => CalculateTotalInvoiceAmount(CurrencyType.RON);
        public decimal TotalInvoiceAmountUSD => CalculateTotalInvoiceAmount(CurrencyType.USD);

        public decimal TotalPaymentAmountEUR => CalculateTotalPaymentAmount(CurrencyType.EUR);
        public decimal TotalPaymentAmountRON => CalculateTotalPaymentAmount(CurrencyType.RON);
        public decimal TotalPaymentAmountUSD => CalculateTotalPaymentAmount(CurrencyType.USD);

        // Bakiye hesaplama
        public decimal BalanceEUR => TotalPaymentAmountEUR - TotalInvoiceAmountEUR;
        public decimal BalanceRON => TotalPaymentAmountRON - TotalInvoiceAmountRON;
        public decimal BalanceUSD => TotalPaymentAmountUSD - TotalInvoiceAmountUSD;

        private decimal CalculateTotalInvoiceAmount(CurrencyType currency)
        {
            decimal total = 0;
            if (Invoices != null)
            {
                foreach (var invoice in Invoices)
                {
                    if (invoice.Currency == currency)
                    {
                        total += invoice.TotalAmount;
                    }
                }
            }
            return total;
        }

        private decimal CalculateTotalPaymentAmount(CurrencyType currency)
        {
            decimal total = 0;
            if (Payments != null)
            {
                foreach (var payment in Payments)
                {
                    if (payment.Currency == currency)
                    {
                        foreach (var detail in payment.PaymentDetails)
                        {
                            total += detail.TotalAmount;
                        }
                    }
                }
            }
            return total;
        }
    }
}
