using BusinessLayer.Services;
using EntityLayer.Entities;
using InvoiceTracking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InvoiceTracking.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;
        private readonly IPaymentDetailService _paymentDetailService;
        private readonly IClientService _clientService;
        private readonly IItemService _itemService;
        private readonly ICashRegisterService _cashRegisterService;

        public PaymentController(
            IPaymentService paymentService,
            IPaymentDetailService paymentDetailService,
            IClientService clientService,
            IItemService itemService,
            ICashRegisterService cashRegisterService)
        {
            _paymentService = paymentService;
            _paymentDetailService = paymentDetailService;
            _clientService = clientService;
            _itemService = itemService;
            _cashRegisterService = cashRegisterService;
        }

        public IActionResult Index()
        {
            var payments = _paymentService.GetPaymentsWithDetails();
            return View(payments);
        }

        public async Task<IActionResult> Details(int id)
        {
            var payment = await _paymentService.GetPaymentWithDetailsAsync(id);
            if (payment == null)
            {
                return NotFound();
            }
            return View(payment);
        }

        public IActionResult Create(int? clientId = null)
        {
            ViewBag.Clients = new SelectList(_clientService.GetAll(), "ClientId", "CompanyName", clientId);
            ViewBag.CurrencyTypes = new SelectList(Enum.GetValues(typeof(Enums.CurrencyType)));

            var viewModel = new PaymentViewModel
            {
                ClientId = clientId ?? 0
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PaymentViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var payment = new Payment
                {
                    ClientId = viewModel.ClientId,
                    Currency = viewModel.Currency,
                    Date = viewModel.Date,
                    Description = viewModel.Description
                };

                var details = new List<PaymentDetail>();
                if (viewModel.Products != null && viewModel.Products.Count > 0)
                {
                    var paymentDetail = new PaymentDetail
                    {
                        Currency = viewModel.Currency
                    };
                    paymentDetail.SetSoldProducts(viewModel.Products);
                    details.Add(paymentDetail);
                }

                await _paymentService.CreatePaymentWithDetailsAsync(payment, details);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Clients = new SelectList(_clientService.GetAll(), "ClientId", "CompanyName", viewModel.ClientId);
            ViewBag.CurrencyTypes = new SelectList(Enum.GetValues(typeof(Enums.CurrencyType)), viewModel.Currency);

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> AddPaymentDetail(int paymentId)
        {
            var payment = await _paymentService.GetByIdAsync(paymentId);
            if (payment == null)
            {
                return NotFound();
            }

            ViewBag.Items = new SelectList( _itemService.GetAll(), "ItemId", "ItemName");
            ViewBag.PaymentId = paymentId;
            ViewBag.Currency = payment.Currency;

            return View(new PaymentDetailViewModel { PaymentId = paymentId, Currency = payment.Currency });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPaymentDetail(PaymentDetailViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                await _paymentDetailService.AddPaymentDetailWithProductsAsync(viewModel.PaymentId, viewModel.Products);
                return RedirectToAction(nameof(Details), new { id = viewModel.PaymentId });
            }

            ViewBag.Items = new SelectList( _itemService.GetAll(), "ItemId", "ItemName");
            ViewBag.PaymentId = viewModel.PaymentId;
            ViewBag.Currency = viewModel.Currency;

            return View(viewModel);
        }

    }
}
