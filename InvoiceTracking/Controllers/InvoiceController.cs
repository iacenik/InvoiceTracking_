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
    public class InvoiceController : Controller
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IClientService _clientService;
        private readonly ICashRegisterService _cashRegisterService;
        private readonly IInvoiceDetailService _invoiceDetailService;

        public InvoiceController(
            IInvoiceService invoiceService,
            IClientService clientService,
            ICashRegisterService cashRegisterService,
            IInvoiceDetailService invoiceDetailService)
        {
            _invoiceService = invoiceService;
            _clientService = clientService;
            _cashRegisterService = cashRegisterService;
            _invoiceDetailService = invoiceDetailService;
        }

        // 📌 1️⃣ Tüm faturaları listele
        public async Task<IActionResult> Index()
        {
            var invoices = await _invoiceService.GetAllInvoicesAsync();
            return View(invoices);
        }

        // 📌 2️⃣ Yeni fatura ekleme sayfası
        public async Task<IActionResult> Create()
        {
            await PopulateDropDowns();
            return View();
        }

        // 📌 3️⃣ Yeni fatura ekleme işlemi
        [HttpPost]
        public async Task<IActionResult> Create(Invoice invoice)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropDowns();
                return View(invoice);
            }

            await _invoiceService.AddInvoiceAsync(invoice);
            return RedirectToAction(nameof(Index));
        }

        // 📌 4️⃣ Fatura güncelleme sayfası
        public async Task<IActionResult> Edit(int id)
        {
            var invoice = await _invoiceService.GetInvoicesByClientIdAsync(id);
            if (invoice == null)
                return NotFound();

            await PopulateDropDowns(invoice.FirstOrDefault());
            return View(invoice.FirstOrDefault());
        }

        // 📌 5️⃣ Fatura güncelleme işlemi
        [HttpPost]
        public async Task<IActionResult> Edit(Invoice invoice)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropDowns(invoice);
                return View(invoice);
            }

            await _invoiceService.UpdateInvoiceAsync(invoice);
            return RedirectToAction(nameof(Index));
        }

        // 📌 6️⃣ Fatura silme işlemi
        public async Task<IActionResult> Delete(int id)
        {
            var invoice = await _invoiceService.GetInvoicesByClientIdAsync(id);
            if (invoice == null)
                return NotFound();

            return View(invoice.FirstOrDefault());
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _invoiceService.DeleteInvoiceAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // 📌 7️⃣ Belirli bir müşteriye ait faturaları listeleme
        public async Task<IActionResult> ClientInvoices(int clientId)
        {
            var invoices = await _invoiceService.GetInvoicesByClientIdAsync(clientId);
            return View("Index", invoices);
        }

        // 📌 8️⃣ Fatura onaylama (Kasadan düşme işlemi)
        public async Task<IActionResult> ApproveInvoice(int invoiceId)
        {
            var cashRegister = await _cashRegisterService.GetFirstAsync();
            if (cashRegister == null)
            {
                ModelState.AddModelError("", "Kasa bulunamadı!");
                return RedirectToAction(nameof(Index));
            }

            await _invoiceService.ApproveInvoiceAsync(invoiceId, cashRegister);
            return RedirectToAction(nameof(Index));
        }

        // 📌 9️⃣ Fatura detaylarını listeleme
        public async Task<IActionResult> InvoiceDetails(int invoiceId)
        {
            var details = await _invoiceDetailService.GetDetailsByInvoiceIdAsync(invoiceId);
            return View(details);
        }

        // 📌 🔥 Form dropdownlarını doldurma metodu
        private async Task PopulateDropDowns(Invoice? invoice = null)
        {
            var clients = await _clientService.GetAllClientsAsync();
            ViewBag.Clients = new SelectList(clients, "ClientId", "CompanyName", invoice?.ClientId);
        }
    }
}
