using BusinessLayer.Services;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using static EntityLayer.Entities.Enums;

namespace InvoiceSystem.Controllers
{
    public class CashRegisterController : Controller
    {
        private readonly ICashRegisterService _cashRegisterService;

        public CashRegisterController(ICashRegisterService cashRegisterService)
        {
            _cashRegisterService = cashRegisterService;
        }

        // 📌 1️⃣ Kasa bilgilerini görüntüleme
        public async Task<IActionResult> Index()
        {
            var cashRegister = await _cashRegisterService.GetCashRegisterAsync();
            if (cashRegister == null)
            {
                ModelState.AddModelError("", "Kasa bilgileri bulunamadı!");
                return View(new CashRegister());
            }
            return View(cashRegister);
        }

        // 📌 2️⃣ Kasaya para ekleme (Gelir ekleme)
        [HttpPost]
        public async Task<IActionResult> AddIncome(decimal amount, CurrencyType currency)
        {
            if (amount <= 0)
            {
                ModelState.AddModelError("", "Geçerli bir tutar giriniz!");
                return RedirectToAction(nameof(Index));
            }

            await _cashRegisterService.AddIncomeAsync(amount, currency);
            TempData["SuccessMessage"] = "Gelir başarıyla eklendi!";
            return RedirectToAction(nameof(Index));
        }

        // 📌 3️⃣ Kasadan para düşme (Gider)
        [HttpPost]
        public async Task<IActionResult> DeductExpense(decimal amount, CurrencyType currency)
        {
            if (amount <= 0)
            {
                ModelState.AddModelError("", "Geçerli bir tutar giriniz!");
                return RedirectToAction(nameof(Index));
            }

            await _cashRegisterService.DeductExpenseAsync(amount, currency);
            TempData["SuccessMessage"] = "Gider başarıyla düşüldü!";
            return RedirectToAction(nameof(Index));
        }
    }
}
