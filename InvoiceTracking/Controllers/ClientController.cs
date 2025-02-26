using BusinessLayer.Services;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InvoiceSystem.Controllers
{
    public class ClientController : Controller
    {
        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        // 📌 1️⃣ Tüm müşterileri listele
        public async Task<IActionResult> Index()
        {
            var clients = await _clientService.GetAllClientsAsync();
            return View(clients);
        }

        // 📌 2️⃣ Yeni müşteri ekleme formu
        public IActionResult Create()
        {
            return View();
        }

        // 📌 3️⃣ Yeni müşteri ekleme işlemi
        [HttpPost]
        public async Task<IActionResult> Create(Client client)
        {
            if (ModelState.IsValid)
            {
                await _clientService.AddClientAsync(client);
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        // 📌 4️⃣ Müşteri güncelleme formu
        public async Task<IActionResult> Edit(int id)
        {
            var client = await _clientService.GetClientByIdAsync(id);
            if (client == null)
                return NotFound();

            return View(client);
        }

        // 📌 5️⃣ Müşteri güncelleme işlemi
        [HttpPost]
        public async Task<IActionResult> Edit(Client client)
        {
            if (ModelState.IsValid)
            {
                await _clientService.UpdateClientAsync(client);
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        // 📌 6️⃣ Müşteri silme işlemi
        public async Task<IActionResult> Delete(int id)
        {
            var client = await _clientService.GetClientByIdAsync(id);
            if (client == null)
                return NotFound();

            return View(client);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _clientService.DeleteClientAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // 📌 7️⃣ Müşteri faturalarını görüntüle
        public async Task<IActionResult> Invoices(int clientId)
        {
            var invoices = await _clientService.GetClientInvoicesAsync(clientId);
            return View(invoices);
        }

        // 📌 8️⃣ Müşteri ödemelerini görüntüle
        public async Task<IActionResult> Payments(int clientId)
        {
            var payments = await _clientService.GetClientPaymentsAsync(clientId);
            return View(payments);
        }
    }
}
