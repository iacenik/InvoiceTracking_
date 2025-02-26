using EntityLayer.Entities;
using System.Threading.Tasks;

namespace BusinessLayer.Common
{
    public interface ICashRegisterRepository : IGenericRepository<CashRegister>
    {
        Task<CashRegister?> GetCashRegisterAsync();
        Task UpdateCashRegisterAsync(CashRegister cashRegister);
        Task<CashRegister?> GetFirstAsync(); // 💡 Eksik olan metodu ekledik
    }
}
