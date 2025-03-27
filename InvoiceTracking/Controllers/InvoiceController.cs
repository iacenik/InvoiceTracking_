using BusinessLayer.Services;
using EntityLayer.Entities;
using InvoiceTracking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace InvoiceTracking.Controllers
{
    [Authorize]
    public class InvoiceController : Controller
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IInvoiceDetailService _invoiceDetailService;
        private readonly IClientService _clientService;
        private readonly IEmployeeService _employeeService;
        private readonly IExpenseCategoryService _categoryService;
        private readonly IItemService _itemService;
        private readonly ICashRegisterService _cashRegisterService;

        public InvoiceController(
            IInvoiceService invoiceService,
            IInvoiceDetailService invoiceDetailService,
            IClientService clientService,
            IEmployeeService employeeService,
            IExpenseCategoryService categoryService,
            IItemService itemService,
            ICashRegisterService cashRegisterService)
        {
            _invoiceService = invoiceService;
            _invoiceDetailService = invoiceDetailService;
            _clientService = clientService;
            _employeeService = employeeService;
            _categoryService = categoryService;
            _itemService = itemService;
            _cashRegisterService = cashRegisterService;
        }

        public IActionResult Index()
        {
            var invoices = _invoiceService.GetInvoicesWithDetails();
            return View(invoices);
        }

        public async Task<IActionResult> Details(int id)
        {
            var invoice = await _invoiceService.GetInvoiceWithDetailsAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }
            return View(invoice);
        }

        public IActionResult Create(int? clientId = null)
        {
            ViewBag.Clients = new SelectList(_clientService.GetAll(), "ClientId", "CompanyName", clientId);
            ViewBag.Employees = new SelectList(_employeeService.GetAll(), "EmployeeId", "EmployeeName");
            ViewBag.Categories = new SelectList(_categoryService.GetAll(), "CategoryId", "CategoryName");
            ViewBag.CurrencyTypes = new SelectList(Enum.GetValues(typeof(Enums.CurrencyType)));
            ViewBag.InvoiceTypes = new SelectList(Enum.GetValues(typeof(Enums.InvoiceType)));
            ViewBag.Items = new SelectList(_itemService.GetAll(), "ItemId", "ItemName");

            var viewModel = new InvoiceViewModel
            {
                ClientId = clientId ?? 0,
                Date = DateTime.Today
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] object viewModelObject)
        {
            try
            {
                InvoiceViewModel viewModel = JsonConvert.DeserializeObject<InvoiceViewModel>(viewModelObject.ToString());
                Console.WriteLine($"Received viewModel - Products Count: {viewModel.Products?.Count ?? 0}");
                if (viewModel.Products != null)
                {
                    foreach (var product in viewModel.Products)
                    {
                        Console.WriteLine($"Product: {product.ProductName}, Quantity: {product.Quantity}, Price={product.UnitPrice}, Currency={product.Currency}");
                    }
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var invoice = new Invoice
                {
                    Date = viewModel.Date,
                    CategoryId = viewModel.CategoryId,
                    EmployeeId = viewModel.EmployeeId,
                    ClientId = viewModel.ClientId,
                    Currency = viewModel.Currency,
                    InvoiceType = viewModel.InvoiceType,
                    IsPaid = viewModel.IsPaid
                };

                Console.WriteLine($"Created Invoice: Date={invoice.Date}, ClientId={invoice.ClientId}, Currency={invoice.Currency}");

                var details = new List<InvoiceDetail>();
                if (viewModel.Products != null && viewModel.Products.Count > 0)
                {
                    var invoiceDetail = new InvoiceDetail
                    {
                        Currency = viewModel.Currency
                    };
                    invoiceDetail.SetSoldProducts(viewModel.Products);
                    details.Add(invoiceDetail);

                    Console.WriteLine($"Created InvoiceDetail: Currency={invoiceDetail.Currency}, TotalAmount={invoiceDetail.TotalAmount}, Products Count={viewModel.Products.Count}");
                }

                await _invoiceService.CreateInvoiceWithDetailsAsync(invoice, details);

                // Faturayı tekrar çek çünkü TotalAmount değeri güncellendi
                invoice = await _invoiceService.GetInvoiceWithDetailsAsync(invoice.InvoiceId);

                if (viewModel.IsPaid && invoice != null)
                {
                    var result = await _invoiceService.ApproveInvoiceAsync(invoice.InvoiceId);
                    if (result)
                    {
                        // Kasa güncellemesi
                        var cashRegister = new CashRegister
                        {
                            Date = invoice.Date,
                            Amount = invoice.TotalAmount,
                            Currency = invoice.Currency,
                            Description = $"Fatura Onayı: {invoice.InvoiceId}",
                            TransactionType = Enums.TransactionType.Expense
                        };
                        await _cashRegisterService.AddAsync(cashRegister);
                        Console.WriteLine($"Cash register updated for invoice {invoice.InvoiceId}: Amount={invoice.TotalAmount} {invoice.Currency}");
                    }
                }

                return Json(new { success = true, message = "Fatura başarıyla oluşturuldu." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Create action: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                return Json(new { success = false, message = $"Bir hata oluştu: {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<JsonResult> ApproveInvoice(int id)
        {
            try
            {
                var invoice = await _invoiceService.GetByIdAsync(id);
                if (invoice == null)
                {
                    return Json(new { success = false, message = "Fatura bulunamadı." });
                }

                var result = await _invoiceService.ApproveInvoiceAsync(id);
                if (result)
                {
                    // Kasa güncellemesi
                    var cashRegister = new CashRegister
                    {
                        Date = invoice.Date,
                        Amount = invoice.TotalAmount,
                        Currency = invoice.Currency,
                        Description = $"Fatura Onayı: {invoice.InvoiceId}",
                        TransactionType = Enums.TransactionType.Expense
                    };
                    await _cashRegisterService.AddAsync(cashRegister);

                    return Json(new { success = true, message = "Fatura başarıyla onaylandı ve kasa güncellendi." });
                }
                return Json(new { success = false, message = "Fatura onaylanırken bir hata oluştu." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Bir hata oluştu: {ex.Message}" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> AddInvoiceDetail(int invoiceId)
        {
            var invoice = await _invoiceService.GetByIdAsync(invoiceId);
            if (invoice == null)
            {
                return NotFound();
            }

            ViewBag.Items = new SelectList( _itemService.GetAll(), "ItemId", "ItemName");
            ViewBag.InvoiceId = invoiceId;
            ViewBag.Currency = invoice.Currency;

            return View(new InvoiceDetailViewModel { InvoiceId = invoiceId, Currency = invoice.Currency });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddInvoiceDetail(InvoiceDetailViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                await _invoiceDetailService.AddInvoiceDetailWithProductsAsync(viewModel.InvoiceId, viewModel.Products);
                return RedirectToAction(nameof(Details), new { id = viewModel.InvoiceId });
            }

            ViewBag.Items = new SelectList( _itemService.GetAll(), "ItemId", "ItemName");
            ViewBag.PaymentId = viewModel.InvoiceId;
            ViewBag.Currency = viewModel.Currency;

            return View(viewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var invoice = await _invoiceService.GetInvoiceWithDetailsAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }
            return View(invoice);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var invoice = await _invoiceService.GetInvoiceWithDetailsAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }

            _invoiceService.DeleteById(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
