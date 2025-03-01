using BusinessLayer.Services;
using InvoiceTracking.Models;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceTracking.Controllers
{
    public class CashRegisterController : Controller
    {
        private readonly ICashRegisterService _cashRegisterService;

        public CashRegisterController(ICashRegisterService cashRegisterService)
        {
            _cashRegisterService = cashRegisterService;
        }

        public async Task<IActionResult> Index()
        {
            var cashRegister = await _cashRegisterService.GetCashRegisterAsync();
            return View(cashRegister);
        }

        [HttpGet]
        public IActionResult AddIncome()
        {
            return View(new CashRegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddIncome(CashRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                _cashRegisterService.AddIncome(model.Amount, model.Currency);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult DeductExpense()
        {
            return View(new CashRegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeductExpense(CashRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                _cashRegisterService.DeductExpense(model.Amount, model.Currency);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
    }
}
