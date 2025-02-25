using BusinessLayer.Common;
using DataAccessLayer.Repository;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;
using static EntityLayer.Entities.Enums;

namespace InvoiceTracking.Controllers
{
    public class PaymentDetailController : Controller
    {
        private readonly IPaymentDetailRepository _paymentDetailRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IClientRepository _clientRepository;

        public PaymentDetailController(IPaymentDetailRepository paymentDetailRepository, IPaymentRepository paymentRepository, IClientRepository clientRepository)
        {
            _paymentDetailRepository = paymentDetailRepository;
            _paymentRepository = paymentRepository;
            _clientRepository = clientRepository;
        }

        public async Task<IActionResult> Index()
        {
            var paymentDetails = await _paymentDetailRepository.GetAllPaymentDetailsAsync();
            return View(paymentDetails);
        }

        public async Task<IActionResult> Create()
        {
            ViewData["Clients"] = new SelectList(await _clientRepository.GetAllClientsAsync(), "ClientId", "CompanyName");
            ViewData["PaymentId"] = new SelectList(new List<SelectListItem>(), "Value", "Text");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PaymentDetail paymentDetail)
        {
            if (!string.IsNullOrEmpty(paymentDetail.SoldProducts))
            {
                if (!TryDeserializeSoldProducts(paymentDetail.SoldProducts, out var products))
                {
                    ModelState.AddModelError("SoldProducts", "Ürün listesi geçersiz.");
                }
                else if (!products.Any())
                {
                    ModelState.AddModelError("SoldProducts", "Ürün listesi boş olamaz.");
                }
                else
                {
                    paymentDetail.CalculateTotalAmount();
                }
            }

            if (ModelState.IsValid)
            {
                await _paymentDetailRepository.AddPaymentDetailAsync(paymentDetail);
                await UpdatePaymentAmount(paymentDetail.PaymentId);
                return RedirectToAction(nameof(Index));
            }

            await PopulateDropdowns(paymentDetail.PaymentId);
            return View(paymentDetail);
        }

        private async Task UpdatePaymentAmount(int paymentId)
        {
            var relatedPayment = await _paymentRepository.GetPaymentByIdAsync(paymentId);
            if (relatedPayment != null)
            {
                relatedPayment.CalculateAmount();
                await _paymentRepository.UpdatePaymentAsync(relatedPayment);
            }
        }

        private bool TryDeserializeSoldProducts(string json, out List<SoldProduct> products)
        {
            try
            {
                products = JsonSerializer.Deserialize<List<SoldProduct>>(json) ?? new List<SoldProduct>();
                return true;
            }
            catch (JsonException)
            {
                products = new List<SoldProduct>();
                return false;
            }
        }

        private async Task PopulateDropdowns(int? selectedPaymentId = null)
        {
            var payments = await _paymentRepository.GetAllPaymentsAsync();
            ViewData["PaymentId"] = new SelectList(
                payments.Select(p => new
                {
                    p.PaymentId,
                    DisplayText = $"{p.Client?.CompanyName} - {p.Date:dd/MM/yyyy} - {p.Amount} {p.Currency}"
                }),
                "PaymentId",
                "DisplayText",
                selectedPaymentId
            );

            ViewData["CurrencyTypes"] = Enum.GetValues(typeof(CurrencyType))
                .Cast<CurrencyType>()
                .Select(ct => new SelectListItem { Value = ct.ToString(), Text = ct.ToString() })
                .ToList();
        }
    }
}
