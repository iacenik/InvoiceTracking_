using BusinessLayer.Common;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Threading.Tasks;

namespace InvoiceTracking.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IClientRepository _clientRepository;
        private readonly ICashRegisterRepository _cashRegisterRepository;

        public PaymentController(IPaymentRepository paymentRepository, IClientRepository clientRepository, ICashRegisterRepository cashRegisterRepository)
        {
            _paymentRepository = paymentRepository;
            _clientRepository = clientRepository;
            _cashRegisterRepository = cashRegisterRepository;
        }

        // 📌 Tüm Ödemeleri Listeleme
        public async Task<IActionResult> Index()
        {
            var payments = await _paymentRepository.GetAllPaymentsAsync();
            return View(payments);
        }

        // 📌 Ödeme Detayı Görüntüleme
        public async Task<IActionResult> Details(int id)
        {
            var payment = await _paymentRepository.GetPaymentByIdAsync(id);
            if (payment == null)
                return NotFound();

            return View(payment);
        }
        [HttpGet]
        public async Task<JsonResult> GetPaymentsByClient(int clientId)
        {
            var payments = await _paymentRepository.GetPaymentsByClientAsync(clientId);
            var paymentList = payments.Select(p => new
            {
                PaymentId = p.PaymentId,
                DisplayText = $"{p.Date:dd/MM/yyyy} - {p.Amount} {p.Currency}"
            });

            return Json(paymentList);
        }

        // 📌 Yeni Ödeme Ekleme Sayfası
        public async Task<IActionResult> Create()
        {
            ViewData["ClientId"] = new SelectList(await _clientRepository.GetAllClientsAsync(), "ClientId", "CompanyName");
            return View();
        }

        // 📌 Yeni Ödeme Ekleme İşlemi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Payment payment)
        {
            if (ModelState.IsValid)
            {
                await _paymentRepository.AddPaymentAsync(payment);
                return RedirectToAction(nameof(Index));
            }

            ViewData["ClientId"] = new SelectList(await _clientRepository.GetAllClientsAsync(), "ClientId", "CompanyName", payment.ClientId);
            return View(payment);
        }

        // 📌 Ödeme Güncelleme Sayfası
        public async Task<IActionResult> Edit(int id)
        {
            var payment = await _paymentRepository.GetPaymentByIdAsync(id);
            if (payment == null)
                return NotFound();

            ViewData["ClientId"] = new SelectList(await _clientRepository.GetAllClientsAsync(), "ClientId", "CompanyName", payment.ClientId);
            return View(payment);
        }

        // 📌 Ödeme Güncelleme İşlemi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Payment payment)
        {
            if (id != payment.PaymentId)
                return NotFound();

            if (ModelState.IsValid)
            {
                await _paymentRepository.UpdatePaymentAsync(payment);
                return RedirectToAction(nameof(Index));
            }

            ViewData["ClientId"] = new SelectList(await _clientRepository.GetAllClientsAsync(), "ClientId", "CompanyName", payment.ClientId);
            return View(payment);
        }

        // 📌 Ödeme Silme Onayı
        public async Task<IActionResult> Delete(int id)
        {
            var payment = await _paymentRepository.GetPaymentByIdAsync(id);
            if (payment == null)
                return NotFound();

            return View(payment);
        }

        // 📌 Ödeme Silme İşlemi
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _paymentRepository.DeletePaymentAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
