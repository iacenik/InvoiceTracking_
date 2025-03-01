namespace InvoiceTracking.Models
{
    public class DashboardViewModel
    {
        public decimal BalanceEUR { get; set; }
        public decimal BalanceRON { get; set; }
        public decimal BalanceUSD { get; set; }

        // İstatistikler ve grafikler için veriler
        public int TotalClients { get; set; }
        public int TotalInvoices { get; set; }
        public int TotalUnpaidInvoices { get; set; }
        public decimal TotalIncomeEUR { get; set; }
        public decimal TotalExpenseEUR { get; set; }

        // Grafik verileri
        public List<ChartData> MonthlyIncomeData { get; set; } = new List<ChartData>();
        public List<ChartData> MonthlyCurrencyData { get; set; } = new List<ChartData>();
    }

    public class ChartData
    {
        public string? Label { get; set; }
        public decimal Value { get; set; }
    }
}

