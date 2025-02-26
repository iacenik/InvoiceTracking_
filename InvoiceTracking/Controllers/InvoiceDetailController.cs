using BusinessLayer.Services;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceSystem.Controllers
{
    public class InvoiceDetailController : Controller
    {
        private readonly IInvoiceDetailService _invoiceDetailService;
        private readonly IInvoiceService _invoiceService;

        public InvoiceDetailController(
            IInvoiceDetailService invoiceDetailService,
            IInvoiceService invoiceService)
        {
            _invoiceDetailService = invoiceDetailService;
            _invoiceService = invoiceService;
        }

        // 📌 1️⃣ Tüm fatura detaylarını listele
        public async Task<IActionResult> Index()
        {
            var invoiceDetails = await _invoiceDetailService.GetAllInvoiceDetailsAsync();
            return View(invoiceDetails);
        }

        // 📌 2️⃣ Belirli bir faturaya ait detayları listele
        public async Task<IActionResult> InvoiceDetails(int invoiceId)
        {
            var details = await _invoiceDetailService.GetDetailsByInvoiceIdAsync(invoiceId);
            ViewBag.InvoiceId = invoiceId; // Fatura ID'yi View'a gönderiyoruz
            return View(details);
        }

        // 📌 3️⃣ Yeni fatura detayı ekleme sayfası
        public async Task<IActionResult> Create(int invoiceId)
        {
            await PopulateDropDowns(invoiceId);
            return View(new InvoiceDetail { InvoiceId = invoiceId });
        }

        // 📌 4️⃣ Yeni fatura detayı ekleme işlemi
        [HttpPost]
        public async Task<IActionResult> Create(InvoiceDetail invoiceDetail)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropDowns(invoiceDetail.InvoiceId);
                return View(invoiceDetail);
            }

            await _invoiceDetailService.AddInvoiceDetailAsync(invoiceDetail);
            return RedirectToAction(nameof(InvoiceDetails), new { invoiceId = invoiceDetail.InvoiceId });
        }

        // 📌 5️⃣ Fatura detayını güncelleme sayfası
        public async Task<IActionResult> Edit(int id)
        {
            var invoiceDetail = await _invoiceDetailService.GetDetailByIdAsync(id);
            if (invoiceDetail == null)
                return NotFound();

            await PopulateDropDowns(invoiceDetail.InvoiceId);
            return View(invoiceDetail);
        }

        // 📌 6️⃣ Fatura detayını güncelleme işlemi
        [HttpPost]
        public async Task<IActionResult> Edit(InvoiceDetail invoiceDetail)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropDowns(invoiceDetail.InvoiceId);
                return View(invoiceDetail);
            }

            await _invoiceDetailService.UpdateInvoiceDetailAsync(invoiceDetail);
            return RedirectToAction(nameof(InvoiceDetails), new { invoiceId = invoiceDetail.InvoiceId });
        }

        // 📌 7️⃣ Fatura detayını silme işlemi
        public async Task<IActionResult> Delete(int id)
        {
            var invoiceDetail = await _invoiceDetailService.GetDetailByIdAsync(id);
            if (invoiceDetail == null)
                return NotFound();

            return View(invoiceDetail);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var invoiceDetail = await _invoiceDetailService.GetDetailByIdAsync(id);
            if (invoiceDetail != null)
            {
                await _invoiceDetailService.DeleteInvoiceDetailAsync(id);
                return RedirectToAction(nameof(InvoiceDetails), new { invoiceId = invoiceDetail.InvoiceId });
            }

            return NotFound();
        }

        // 📌 🔥 Fatura listesi dropdown'ını doldurma
        private async Task PopulateDropDowns(int invoiceId)
        {
            var invoices = await _invoiceService.GetAllInvoicesAsync();
            ViewBag.Invoices = new SelectList(invoices, "InvoiceId", "InvoiceId", invoiceId);
        }
    }
}
