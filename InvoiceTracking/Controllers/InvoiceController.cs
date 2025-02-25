using BusinessLayer.Common;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InvoiceTracking.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IExpenseCategoryRepository _categoryRepository;

        public InvoiceController(IInvoiceRepository invoiceRepository, IClientRepository clientRepository, IEmployeeRepository employeeRepository, IExpenseCategoryRepository categoryRepository)
        {
            _invoiceRepository = invoiceRepository;
            _clientRepository = clientRepository;
            _employeeRepository = employeeRepository;
            _categoryRepository = categoryRepository;
        }

        // 📌 Tüm Faturaları Listeleme
        public async Task<IActionResult> Index()
        {
            var invoices = await _invoiceRepository.GetAllInvoicesAsync();
            return View(invoices);
        }

        // 📌 Fatura Detayları
        public async Task<IActionResult> Details(int id)
        {
            var invoice = await _invoiceRepository.GetInvoiceByIdAsync(id);
            if (invoice == null)
                return NotFound();

            return View(invoice);
        }

        // 📌 Yeni Fatura Ekleme Sayfası (Form Göster)
        public async Task<IActionResult> Create()
        {
            ViewData["ClientId"] = new SelectList(await _clientRepository.GetAllClientsAsync(), "ClientId", "CompanyName");
            ViewData["EmployeeId"] = new SelectList(await _employeeRepository.GetAllEmployeesAsync(), "EmployeeId", "EmployeeName");
            ViewData["CategoryId"] = new SelectList(await _categoryRepository.GetAllExpenseCategoriesAsync(), "CategoryId", "CategoryName");

            return View();
        }

        // 📌 Yeni Fatura Ekleme (Form POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                await _invoiceRepository.AddInvoiceAsync(invoice);
                return RedirectToAction(nameof(Index));
            }

            ViewData["ClientId"] = new SelectList(await _clientRepository.GetAllClientsAsync(), "ClientId", "CompanyName", invoice.ClientId);
            ViewData["EmployeeId"] = new SelectList(await _employeeRepository.GetAllEmployeesAsync(), "EmployeeId", "EmployeeName", invoice.EmployeeId);
            ViewData["CategoryId"] = new SelectList(await _categoryRepository.GetAllExpenseCategoriesAsync(), "CategoryId", "CategoryName", invoice.CategoryId);

            return View(invoice);
        }

        // 📌 Fatura Güncelleme Sayfası (Form Göster)
        public async Task<IActionResult> Edit(int id)
        {
            var invoice = await _invoiceRepository.GetInvoiceByIdAsync(id);
            if (invoice == null)
                return NotFound();

            ViewData["ClientId"] = new SelectList(await _clientRepository.GetAllClientsAsync(), "ClientId", "CompanyName", invoice.ClientId);
            ViewData["EmployeeId"] = new SelectList(await _employeeRepository.GetAllEmployeesAsync(), "EmployeeId", "EmployeeName", invoice.EmployeeId);
            ViewData["CategoryId"] = new SelectList(await _categoryRepository.GetAllExpenseCategoriesAsync(), "CategoryId", "CategoryName", invoice.CategoryId);

            return View(invoice);
        }

        // 📌 Fatura Güncelleme (Form POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Invoice invoice)
        {
            if (id != invoice.InvoiceId)
                return NotFound();

            if (ModelState.IsValid)
            {
                await _invoiceRepository.UpdateInvoiceAsync(invoice);
                return RedirectToAction(nameof(Index));
            }

            ViewData["ClientId"] = new SelectList(await _clientRepository.GetAllClientsAsync(), "ClientId", "CompanyName", invoice.ClientId);
            ViewData["EmployeeId"] = new SelectList(await _employeeRepository.GetAllEmployeesAsync(), "EmployeeId", "EmployeeName", invoice.EmployeeId);
            ViewData["CategoryId"] = new SelectList(await _categoryRepository.GetAllExpenseCategoriesAsync(), "CategoryId", "CategoryName", invoice.CategoryId);

            return View(invoice);
        }

        // 📌 Fatura Silme Sayfası (Onay İçin)
        public async Task<IActionResult> Delete(int id)
        {
            var invoice = await _invoiceRepository.GetInvoiceByIdAsync(id);
            if (invoice == null)
                return NotFound();

            return View(invoice);
        }

        // 📌 Fatura Silme İşlemi
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _invoiceRepository.DeleteInvoiceAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // 📌 Faturayı Ödendi Olarak İşaretleme
        public async Task<IActionResult> MarkAsPaid(int id)
        {
            var invoice = await _invoiceRepository.GetInvoiceByIdAsync(id);
            if (invoice == null)
                return NotFound();

            invoice.IsPaid = true;
            await _invoiceRepository.UpdateInvoiceAsync(invoice);

            return RedirectToAction(nameof(Index));
        }
    }

}