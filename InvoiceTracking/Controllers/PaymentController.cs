using BusinessLayer.Services;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.Common;

namespace InvoiceSystem.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;
        private readonly IClientService _clientService;
        private readonly ICashRegisterService _cashRegisterService;

        public PaymentController(
            IPaymentService paymentService,
            IClientService clientService,
            ICashRegisterService cashRegisterService)
        {
            _paymentService = paymentService;
            _clientService = clientService;
            _cashRegisterService = cashRegisterService;
        }

        // 📌 1️⃣ Tüm ödemeleri listeleme
        public async Task<IActionResult> Index()
        {
            var payments = await _paymentService.GetAllPaymentsAsync();
            return View(payments);
        }

        // 📌 2️⃣ Müşteri bazlı ödemeleri listeleme
        public async Task<IActionResult> ClientPayments(int clientId)
        {
            var payments = await _paymentService.GetPaymentsByClientIdAsync(clientId);
            return View("Index", payments);
        }

        // 📌 3️⃣ Yeni ödeme ekleme sayfası
        public async Task<IActionResult> Create()
        {
            await PopulateDropDowns();
            return View();
        }

        // 📌 4️⃣ Yeni ödeme ekleme işlemi
        [HttpPost]
        public async Task<IActionResult> Create(Payment payment)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropDowns();
                return View(payment);
            }

            var cashRegister = await _cashRegisterService.GetFirstAsync();
            if (cashRegister == null)
            {
                ModelState.AddModelError("", "Kasa bulunamadı!");
                await PopulateDropDowns();
                return View(payment);
            }

            await _paymentService.ProcessPaymentAsync(payment, cashRegister);
            return RedirectToAction(nameof(Index));
        }

        // 📌 5️⃣ Ödeme düzenleme sayfası
        public async Task<IActionResult> Edit(int id)
        {
            var payment = await _paymentService.GetAllPaymentsAsync();
            if (payment == null)
                return NotFound();

            await PopulateDropDowns();
            return View(payment.FirstOrDefault());
        }

        // 📌 6️⃣ Ödeme düzenleme işlemi
        [HttpPost]
        public async Task<IActionResult> Edit(Payment payment)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropDowns();
                return View(payment);
            }

            await _paymentService.UpdatePaymentAsync(payment);
            return RedirectToAction(nameof(Index));
        }

        // 📌 7️⃣ Ödeme silme sayfası
        public async Task<IActionResult> Delete(int id)
        {
            var payment = await _paymentService.GetAllPaymentsAsync();
            if (payment == null)
                return NotFound();

            return View(payment.FirstOrDefault());
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _paymentService.DeletePaymentAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // 📌 8️⃣ Form dropdownlarını doldurma metodu
        private async Task PopulateDropDowns()
        {
            var clients = await _clientService.GetAllClientsAsync();
            ViewBag.Clients = new SelectList(clients, "ClientId", "CompanyName");
        }
    }
}
