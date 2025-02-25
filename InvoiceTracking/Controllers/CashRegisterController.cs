using BusinessLayer.Common;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using static EntityLayer.Entities.Enums;

namespace InvoiceTracking.Controllers
{
    public class CashRegisterController : Controller
    {
        private readonly ICashRegisterRepository _cashRegisterRepository;

        public CashRegisterController(ICashRegisterRepository cashRegisterRepository)
        {
            _cashRegisterRepository = cashRegisterRepository;
        }

        // 📌 Kasa durumunu görüntüleme
        public async Task<IActionResult> Index()
        {
            var cashRegister = await _cashRegisterRepository.GetCashRegisterAsync();

            if (cashRegister == null) // Eğer `null` dönerse, yeni bir nesne oluştur.
            {
                cashRegister = new CashRegister();
            }

            return View(cashRegister);
        }

        // 📌 Kasa bilgilerini güncelleme sayfası
        public async Task<IActionResult> Edit()
        {
            var cashRegister = await _cashRegisterRepository.GetCashRegisterAsync();
            return View(cashRegister);
        }

        // 📌 Kasa bilgilerini güncelleme işlemi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CashRegister cashRegister)
        {
            if (ModelState.IsValid)
            {
                await _cashRegisterRepository.UpdateCashRegisterAsync(cashRegister);
                return RedirectToAction(nameof(Index));
            }

            return View(cashRegister);
        }

        // 📌 Gelir ekleme işlemi
        [HttpPost]
        public async Task<IActionResult> AddIncome(decimal amount, CurrencyType currency)
        {
            if (amount <= 0)
            {
                ModelState.AddModelError(string.Empty, "Tutar sıfırdan büyük olmalıdır.");
                return RedirectToAction(nameof(Index));
            }

            await _cashRegisterRepository.AddIncomeAsync(amount, currency);
            return RedirectToAction(nameof(Index));
        }

        // 📌 Gider ekleme işlemi
        [HttpPost]
        public async Task<IActionResult> AddExpense(decimal amount, CurrencyType currency)
        {
            if (amount <= 0)
            {
                ModelState.AddModelError(string.Empty, "Tutar sıfırdan büyük olmalıdır.");
                return RedirectToAction(nameof(Index));
            }

            await _cashRegisterRepository.AddExpenseAsync(amount, currency);
            return RedirectToAction(nameof(Index));
        }

        // 📌 Kasa bakiyesini yeniden hesapla
        [HttpPost]
        public async Task<IActionResult> Recalculate()
        {
            await _cashRegisterRepository.RecalculateCashRegisterAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
