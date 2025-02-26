using BusinessLayer.Common;

using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class InvoiceDetailService : IInvoiceDetailService
    {
        private readonly IInvoiceDetailRepository _invoiceDetailRepository;

        public InvoiceDetailService(IInvoiceDetailRepository invoiceDetailRepository)
        {
            _invoiceDetailRepository = invoiceDetailRepository;
        }

        public async Task<List<InvoiceDetail>> GetAllInvoiceDetailsAsync()
        {
            return (await _invoiceDetailRepository.GetAllAsync()).ToList();
        }

        public async Task<List<InvoiceDetail>> GetDetailsByInvoiceIdAsync(int invoiceId)
        {
            return await _invoiceDetailRepository.GetDetailsByInvoiceIdAsync(invoiceId);
        }

        public async Task<InvoiceDetail?> GetDetailByIdAsync(int invoiceDetailId)
        {
            return await _invoiceDetailRepository.GetDetailByIdAsync(invoiceDetailId);
        }

        public async Task AddInvoiceDetailAsync(InvoiceDetail invoiceDetail)
        {
            await _invoiceDetailRepository.AddAsync(invoiceDetail);
        }

        public async Task UpdateInvoiceDetailAsync(InvoiceDetail invoiceDetail)
        {
            await _invoiceDetailRepository.UpdateAsync(invoiceDetail);
        }

        public async Task DeleteInvoiceDetailAsync(int invoiceDetailId)
        {
            var invoiceDetail = await _invoiceDetailRepository.GetByIdAsync(invoiceDetailId);
            if (invoiceDetail != null)
            {
                await _invoiceDetailRepository.DeleteAsync(invoiceDetail);
            }
        }
    }
}
