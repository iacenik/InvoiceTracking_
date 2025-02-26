using EntityLayer.Entities;
using System.Threading.Tasks;
using static EntityLayer.Entities.Enums;

namespace BusinessLayer.Services
{
    public interface ICashRegisterService
    {
        Task<CashRegister?> GetCashRegisterAsync();
        Task DeductExpenseAsync(decimal amount, CurrencyType currency);
        Task AddIncomeAsync(decimal amount, CurrencyType currency);
        Task<CashRegister> GetFirstAsync();
    }
}
