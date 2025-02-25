using BusinessLayer.Common;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceTracking.Controllers
{
    public class ClientController : Controller
    {
        private readonly IClientRepository _clientRepository;
        public ClientController(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }


        // ✅ Tüm müşterileri listeleme
        public async Task<IActionResult> Index()
        {
            var clients = await _clientRepository.GetAllClientsAsync();
            return View(clients);
        }

        // ✅ Müşteri detaylarını görüntüleme
        public async Task<IActionResult> Details(int id)
        {
            var client = await _clientRepository.GetClientByIdAsync(id);
            if (client == null)
                return NotFound();
            return View(client);
        }

        // ✅ Yeni müşteri ekleme formu
        public IActionResult Create()
        {
            return View();
        }

        // ✅ Yeni müşteri ekleme işlemi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Client client)
        {
            if (ModelState.IsValid)
            {
                client.CreatedAt = DateTime.Now; // 📌 Oluşturma tarihini elle ata
                await _clientRepository.AddClientAsync(client);
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        // ✅ Müşteri güncelleme formu
        public async Task<IActionResult> Edit(int id)
        {
            var client = await _clientRepository.GetClientByIdAsync(id);
            if (client == null)
                return NotFound();
            return View(client);
        }

        // ✅ Müşteri güncelleme işlemi
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
                var existingClient = await _clientRepository.GetClientByIdAsync(id);
                if (existingClient != null)
                {
                    client.CreatedAt = existingClient.CreatedAt; // 📌 Eski oluşturma tarihini koru
                    await _clientRepository.UpdateClientAsync(client);
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(client);
        }

        // ✅ Müşteri silme onay ekranı
        public async Task<IActionResult> Delete(int id)
        {
            var client = await _clientRepository.GetClientByIdAsync(id);
            if (client == null)
                return NotFound();
            return View(client);
        }

        // ✅ Müşteri silme işlemi
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _clientRepository.DeleteClientAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
