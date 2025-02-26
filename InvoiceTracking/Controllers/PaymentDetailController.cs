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
    public class PaymentDetailController : Controller
    {
        private readonly IPaymentDetailService _paymentDetailService;
        private readonly IPaymentService _paymentService;

        public PaymentDetailController(IPaymentDetailService paymentDetailService, IPaymentService paymentService)
        {
            _paymentDetailService = paymentDetailService;
            _paymentService = paymentService;
        }

        // 📌 1️⃣ Tüm ödeme detaylarını listeleme
        public async Task<IActionResult> Index()
        {
            var paymentDetails = await _paymentDetailService.GetAllPaymentDetailsAsync();
            return View(paymentDetails);
        }

        // 📌 2️⃣ Belirli bir ödeme için detayları listeleme
        public async Task<IActionResult> PaymentDetailsByPaymentId(int paymentId)
        {
            var details = await _paymentDetailService.GetDetailsByPaymentIdAsync(paymentId);
            return View("Index", details);
        }

        // 📌 3️⃣ Yeni ödeme detayı ekleme sayfası
        public async Task<IActionResult> Create()
        {
            await PopulateDropDowns();
            return View();
        }

        // 📌 4️⃣ Yeni ödeme detayı ekleme işlemi
        [HttpGet]
        public async Task<IActionResult> Create(PaymentDetail paymentDetail, string productName, decimal unitPrice, int quantity)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropDowns();
                return View(paymentDetail);
            }

            var newProduct = new SoldProduct
            {
                ProductName = productName,
                UnitPrice = unitPrice,
                Quantity = quantity,
                // ❌ TotalPrice'a dışarıdan değer atamıyoruz! 
                // ✅ Hesaplama direkt get bloğundan gelecek.
            };

            var productList = paymentDetail.SoldProductsList;
            productList.Add(newProduct);

            paymentDetail.SetSoldProducts(productList);

            await _paymentDetailService.AddPaymentDetailAsync(paymentDetail);
            return RedirectToAction(nameof(Index));
        }
        // 📌 5️⃣ Ödeme detayı düzenleme sayfası
        public async Task<IActionResult> Edit(int id)
        {
            var paymentDetail = await _paymentDetailService.GetDetailByIdAsync(id);
            if (paymentDetail == null)
                return NotFound();

            await PopulateDropDowns();
            return View(paymentDetail);
        }

        // 📌 6️⃣ Ödeme detayı düzenleme işlemi
        [HttpPost]
        public async Task<IActionResult> Edit(PaymentDetail paymentDetail)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropDowns();
                return View(paymentDetail);
            }

            await _paymentDetailService.UpdatePaymentDetailAsync(paymentDetail);
            return RedirectToAction(nameof(Index));
        }

        // 📌 7️⃣ Ödeme detayı silme sayfası
        public async Task<IActionResult> Delete(int id)
        {
            var paymentDetail = await _paymentDetailService.GetDetailByIdAsync(id);
            if (paymentDetail == null)
                return NotFound();

            return View(paymentDetail);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _paymentDetailService.DeletePaymentDetailAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // 📌 8️⃣ Form dropdownlarını doldurma metodu
        private async Task PopulateDropDowns()
        {
            var payments = await _paymentService.GetAllPaymentsAsync();
            ViewBag.Payments = new SelectList(payments, "PaymentId", "Description");
        }
    }
}
