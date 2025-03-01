using BusinessLayer.Services;
using EntityLayer.Entities;
using InvoiceTracking.Models;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceTracking.Controllers
{
    public class ClientController : Controller
    {
        private readonly IClientService _clientService;
        private readonly IInvoiceService _invoiceService;
        private readonly IPaymentService _paymentService;

        public ClientController(
            IClientService clientService,
            IInvoiceService invoiceService,
            IPaymentService paymentService)
        {
            _clientService = clientService;
            _invoiceService = invoiceService;
            _paymentService = paymentService;
        }

        public IActionResult Index()
        {
            var clients = _clientService.GetAll();
            return View(clients);
        }

        public async Task<IActionResult> Details(int id)
        {
            var client = await _clientService.GetClientWithDetailsAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            var invoices = await _invoiceService.GetInvoicesByClientAsync(id);
            var payments = await _paymentService.GetPaymentsByClientAsync(id);

            var clientViewModel = new ClientViewModel
            {
                Client = client,
                Invoices = invoices,
                Payments = payments
            };

            return View(clientViewModel);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Client client)
        {
            if (ModelState.IsValid)
            {
                await _clientService.AddAsync(client);
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var client = await _clientService.GetByIdAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Client client)
        {
            if (id != client.ClientId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _clientService.Update(client);
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var client = await _clientService.GetByIdAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _clientService.DeleteById(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
