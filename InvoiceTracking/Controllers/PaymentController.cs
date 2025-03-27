using BusinessLayer.Services;
using EntityLayer.Entities;
using InvoiceTracking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

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
            ViewBag.CurrencyTypes = new SelectList(
                Enum.GetValues(typeof(Enums.CurrencyType))
                    .Cast<Enums.CurrencyType>()
                    .Select(c => new { Id = c.ToString(), Name = c.ToString() }),
                "Id",
                "Name"
            );

            var viewModel = new PaymentViewModel
            {
                ClientId = clientId ?? 0,
                Date = DateTime.Today
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] PaymentViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return Json(new { success = false, message = "Geçersiz form verisi: " + string.Join(", ", errors) });
            }

            try
            {
                if (viewModel.ClientId == 0)
                {
                    return Json(new { success = false, message = "Lütfen müşteri seçiniz." });
                }

                var payment = new Payment
                {
                    ClientId = viewModel.ClientId,
                    Currency = viewModel.Currency,
                    Date = viewModel.Date,
                    Description = viewModel.Description ?? ""
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
                else
                {
                    return Json(new { success = false, message = "En az bir ürün eklemelisiniz." });
                }

                await _paymentService.CreatePaymentWithDetailsAsync(payment, details);

                // Kasa güncellemesi
                var cashRegister = new CashRegister
                {
                    Date = payment.Date,
                    Amount = payment.TotalAmount,
                    Currency = payment.Currency,
                    Description = $"Ödeme: {payment.Description}",
                    TransactionType = Enums.TransactionType.Income
                };
                await _cashRegisterService.AddAsync(cashRegister);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Ödeme kaydedilirken bir hata oluştu: " + ex.Message });
            }
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

        public async Task<IActionResult> Delete(int id)
        {
            var payment = await _paymentService.GetPaymentWithDetailsAsync(id);
            if (payment == null)
            {
                return NotFound();
            }
            return View(payment);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var payment = await _paymentService.GetPaymentWithDetailsAsync(id);
            if (payment == null)
            {
                return NotFound();
            }

            try
            {
                // Kasadan düşme işlemi
                var cashRegister = await _cashRegisterService.GetCashRegisterAsync();
                if (cashRegister != null)
                {
                    cashRegister.DeductIncome(payment.TotalAmount, payment.Currency);
                    _cashRegisterService.Update(cashRegister);
                }

                _paymentService.DeleteById(id);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Silme işlemi sırasında bir hata oluştu: {ex.Message}");
                return View("Delete", payment);
            }
        }
    }
}
