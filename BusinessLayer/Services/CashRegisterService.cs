using BusinessLayer.Common;
using EntityLayer.Entities;
using System.Threading.Tasks;
using static EntityLayer.Entities.Enums;

namespace BusinessLayer.Services
{
    public class CashRegisterService : ICashRegisterService
    {
        private readonly ICashRegisterRepository _cashRegisterRepository;

        public CashRegisterService(ICashRegisterRepository cashRegisterRepository)
        {
            _cashRegisterRepository = cashRegisterRepository;
        }

        public async Task<CashRegister?> GetCashRegisterAsync()
        {
            return await _cashRegisterRepository.GetCashRegisterAsync();
        }

        public async Task DeductExpenseAsync(decimal amount, CurrencyType currency)
        {
            var cashRegister = await _cashRegisterRepository.GetCashRegisterAsync();
            if (cashRegister == null) return;

            cashRegister.DeductExpense(amount, currency);
            await _cashRegisterRepository.UpdateCashRegisterAsync(cashRegister);
        }
        public async Task<CashRegister> GetFirstAsync()
        {
            return await _cashRegisterRepository.GetFirstAsync();
        }
        public async Task AddIncomeAsync(decimal amount, CurrencyType currency)
        {
            var cashRegister = await _cashRegisterRepository.GetCashRegisterAsync();
            if (cashRegister == null) return;

            switch (currency)
            {
                case CurrencyType.EUR:
                    cashRegister.TotalIncomeEUR += amount;
                    break;
                case CurrencyType.RON:
                    cashRegister.TotalIncomeRON += amount;
                    break;
                case CurrencyType.USD:
                    cashRegister.TotalIncomeUSD += amount;
                    break;
            }

            await _cashRegisterRepository.UpdateCashRegisterAsync(cashRegister);
        }
    }
}
