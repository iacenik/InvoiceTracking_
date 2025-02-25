using BusinessLayer.Common;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

// InvoiceDetailController (Fatura Detay Yönetimi)
namespace InvoiceTracking.Controllers
{
    public class InvoiceDetailController : Controller
    {
        private readonly IInvoiceDetailRepository _invoiceDetailRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IItemRepository _itemRepository;

        public InvoiceDetailController(IInvoiceDetailRepository invoiceDetailRepository, IInvoiceRepository invoiceRepository, IItemRepository itemRepository)
        {
            _invoiceDetailRepository = invoiceDetailRepository;
            _invoiceRepository = invoiceRepository;
            _itemRepository = itemRepository;
        }

        // 📌 Tüm Fatura Detaylarını Listeleme
        public async Task<IActionResult> Index()
        {
            var invoiceDetails = await _invoiceDetailRepository.GetAllInvoiceDetailsAsync();
            return View(invoiceDetails);
        }

        // 📌 Fatura Detayı Görüntüleme
        public async Task<IActionResult> Details(int id)
        {
            var invoiceDetail = await _invoiceDetailRepository.GetInvoiceDetailByIdAsync(id);
            if (invoiceDetail == null)
                return NotFound();

            return View(invoiceDetail);
        }

        // 📌 Yeni Fatura Detayı Ekleme Sayfası
        public async Task<IActionResult> Create()
        {
            ViewData["InvoiceId"] = new SelectList(await _invoiceRepository.GetAllInvoicesAsync(), "InvoiceId", "InvoiceId");
            ViewData["ItemId"] = new SelectList(await _itemRepository.GetAllItemsAsync(), "ItemId", "ItemName");

            return View();
        }

        // 📌 Yeni Fatura Detayı Ekleme İşlemi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InvoiceDetail invoiceDetail)
        {
            if (ModelState.IsValid)
            {
                invoiceDetail.CalculateTotalPrice(); // Toplam fiyatı hesapla
                await _invoiceDetailRepository.AddInvoiceDetailAsync(invoiceDetail);
                return RedirectToAction(nameof(Index));
            }

            ViewData["InvoiceId"] = new SelectList(await _invoiceRepository.GetAllInvoicesAsync(), "InvoiceId", "InvoiceId", invoiceDetail.InvoiceId);
            ViewData["ItemId"] = new SelectList(await _itemRepository.GetAllItemsAsync(), "ItemId", "ItemName", invoiceDetail.ItemId);

            return View(invoiceDetail);
        }

        // 📌 Fatura Detayı Güncelleme Sayfası
        public async Task<IActionResult> Edit(int id)
        {
            var invoiceDetail = await _invoiceDetailRepository.GetInvoiceDetailByIdAsync(id);
            if (invoiceDetail == null)
                return NotFound();

            ViewData["InvoiceId"] = new SelectList(await _invoiceRepository.GetAllInvoicesAsync(), "InvoiceId", "InvoiceId", invoiceDetail.InvoiceId);
            ViewData["ItemId"] = new SelectList(await _itemRepository.GetAllItemsAsync(), "ItemId", "ItemName", invoiceDetail.ItemId);

            return View(invoiceDetail);
        }

        // 📌 Fatura Detayı Güncelleme İşlemi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, InvoiceDetail invoiceDetail)
        {
            if (id != invoiceDetail.InvoiceDetailId)
                return NotFound();

            if (ModelState.IsValid)
            {
                invoiceDetail.CalculateTotalPrice();
                await _invoiceDetailRepository.UpdateInvoiceDetailAsync(invoiceDetail);
                return RedirectToAction(nameof(Index));
            }

            ViewData["InvoiceId"] = new SelectList(await _invoiceRepository.GetAllInvoicesAsync(), "InvoiceId", "InvoiceId", invoiceDetail.InvoiceId);
            ViewData["ItemId"] = new SelectList(await _itemRepository.GetAllItemsAsync(), "ItemId", "ItemName", invoiceDetail.ItemId);

            return View(invoiceDetail);
        }

        // 📌 Fatura Detayı Silme Onayı
        public async Task<IActionResult> Delete(int id)
        {
            var invoiceDetail = await _invoiceDetailRepository.GetInvoiceDetailByIdAsync(id);
            if (invoiceDetail == null)
                return NotFound();

            return View(invoiceDetail);
        }

        // 📌 Fatura Detayı Silme İşlemi
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _invoiceDetailRepository.DeleteInvoiceDetailAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

