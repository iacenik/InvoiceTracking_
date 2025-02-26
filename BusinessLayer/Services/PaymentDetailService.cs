using BusinessLayer.Common;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class PaymentDetailService : IPaymentDetailService
    {
        private readonly IPaymentDetailRepository _paymentDetailRepository;

        public PaymentDetailService(IPaymentDetailRepository paymentDetailRepository)
        {
            _paymentDetailRepository = paymentDetailRepository;
        }

        public async Task<List<PaymentDetail>> GetAllPaymentDetailsAsync()
        {
            return (await _paymentDetailRepository.GetAllAsync()).ToList();
        }

        public async Task<List<PaymentDetail>> GetDetailsByPaymentIdAsync(int paymentId)
        {
            return await _paymentDetailRepository.GetDetailsByPaymentIdAsync(paymentId);
        }

        public async Task<PaymentDetail?> GetDetailByIdAsync(int paymentDetailId)
        {
            return await _paymentDetailRepository.GetDetailByIdAsync(paymentDetailId);
        }

        public async Task AddPaymentDetailAsync(PaymentDetail paymentDetail)
        {
            await _paymentDetailRepository.AddAsync(paymentDetail);
        }

        public async Task UpdatePaymentDetailAsync(PaymentDetail paymentDetail)
        {
            await _paymentDetailRepository.UpdateAsync(paymentDetail);
        }

        public async Task DeletePaymentDetailAsync(int paymentDetailId)
        {
            var paymentDetail = await _paymentDetailRepository.GetByIdAsync(paymentDetailId);
            if (paymentDetail != null)
            {
                await _paymentDetailRepository.DeleteAsync(paymentDetail);
            }
        }
    }
}
