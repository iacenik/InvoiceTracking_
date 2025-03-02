using BusinessLayer.Services;
using EntityLayer.Entities;
using InvoiceTracking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace InvoiceTracking.Controllers
{
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

            var viewModel = new InvoiceViewModel
            {
                ClientId = clientId ?? 0
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] object viewModelObject)
        {
            InvoiceViewModel viewModel = JsonConvert.DeserializeObject<InvoiceViewModel>(viewModelObject.ToString());
            if (ModelState.IsValid)
            {
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

                var details = new List<InvoiceDetail>();
                if (viewModel.Products != null && viewModel.Products.Count > 0)
                {
                    var invoiceDetail = new InvoiceDetail
                    {
                        Currency = viewModel.Currency
                    };
                    invoiceDetail.SetSoldProducts(viewModel.Products);
                    details.Add(invoiceDetail);
                }

                await _invoiceService.CreateInvoiceWithDetailsAsync(invoice, details);

                if (viewModel.IsPaid)
                {
                    await _invoiceService.ApproveInvoiceAsync(invoice.InvoiceId);
                }

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Clients = new SelectList(_clientService.GetAll(), "ClientId", "CompanyName", viewModel.ClientId);
            ViewBag.Employees = new SelectList(_employeeService.GetAll(), "EmployeeId", "EmployeeName", viewModel.EmployeeId);
            ViewBag.Categories = new SelectList(_categoryService.GetAll(), "CategoryId", "CategoryName", viewModel.CategoryId);
            ViewBag.CurrencyTypes = new SelectList(Enum.GetValues(typeof(Enums.CurrencyType)), viewModel.Currency);
            ViewBag.InvoiceTypes = new SelectList(Enum.GetValues(typeof(Enums.InvoiceType)), viewModel.InvoiceType);

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ApproveInvoice(int id)
        {
            var result = await _invoiceService.ApproveInvoiceAsync(id);
            if (result)
            {
                return RedirectToAction(nameof(Details), new { id });
            }
            return NotFound();
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
            ViewBag.InvoiceId = viewModel.InvoiceId;
            ViewBag.Currency = viewModel.Currency;

            return View(viewModel);
        }
    }
}
