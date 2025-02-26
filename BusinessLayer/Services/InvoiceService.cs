using BusinessLayer.Common;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public InvoiceService(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<List<Invoice>> GetAllInvoicesAsync()
        {
            return (await _invoiceRepository.GetAllAsync()).ToList();
        }

        public async Task<List<Invoice>> GetInvoicesByClientIdAsync(int clientId)
        {
            return await _invoiceRepository.GetInvoicesByClientIdAsync(clientId);
        }

        public async Task<List<Invoice>> GetUnpaidInvoicesAsync()
        {
            return await _invoiceRepository.GetUnpaidInvoicesAsync();
        }

        public async Task ApproveInvoiceAsync(int invoiceId, CashRegister cashRegister)
        {
            var invoice = await _invoiceRepository.GetByIdAsync(invoiceId);
            if (invoice == null)
                throw new Exception("Fatura bulunamadı!");

            await _invoiceRepository.ApproveInvoiceAsync(invoice, cashRegister);
        }

        public async Task AddInvoiceAsync(Invoice invoice)
        {
            await _invoiceRepository.AddAsync(invoice);
        }

        public async Task UpdateInvoiceAsync(Invoice invoice)
        {
            await _invoiceRepository.UpdateAsync(invoice);
        }

        public async Task DeleteInvoiceAsync(int invoiceId)
        {
            var invoice = await _invoiceRepository.GetByIdAsync(invoiceId);
            if (invoice != null)
            {
                await _invoiceRepository.DeleteAsync(invoice);
            }
        }
    }
}
