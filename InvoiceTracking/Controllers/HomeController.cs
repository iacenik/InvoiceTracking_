using BusinessLayer.Services;
using InvoiceTracking.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace InvoiceTracking.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICashRegisterService _cashRegisterService;
        private readonly IClientService _clientService;
        private readonly IInvoiceService _invoiceService;

        public HomeController(
            ILogger<HomeController> logger,
            ICashRegisterService cashRegisterService,
            IClientService clientService,
            IInvoiceService invoiceService)
        {
            _logger = logger;
            _cashRegisterService = cashRegisterService;
            _clientService = clientService;
            _invoiceService = invoiceService;
        }

        public async Task<IActionResult> Index()
        {
            var cashRegister = await _cashRegisterService.GetCashRegisterAsync();
            var unpaidInvoices = await _invoiceService.GetUnpaidInvoicesAsync();
            var clients = _clientService.GetAll();

            var dashboardViewModel = new DashboardViewModel
            {
                BalanceEUR = cashRegister?.BalanceEUR ?? 0,
                BalanceRON = cashRegister?.BalanceRON ?? 0,
                BalanceUSD = cashRegister?.BalanceUSD ?? 0,
                TotalClients = clients.Count(),
                TotalInvoices = (_invoiceService.GetAll()).Count(),
                TotalUnpaidInvoices = unpaidInvoices.Count(),
                TotalIncomeEUR = cashRegister?.TotalIncomeEUR ?? 0,
                TotalExpenseEUR = cashRegister?.TotalExpenseEUR ?? 0
            };

            return View(dashboardViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
