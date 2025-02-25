using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EntityLayer.Entities.Enums;

namespace BusinessLayer.Common
{
    public  interface ICashRegisterRepository : IGenericRepository<CashRegister>
    {
        Task<CashRegister> GetCashRegisterAsync(); // Tek kasa kaydı döndür
        Task UpdateCashRegisterAsync(CashRegister cashRegister); // Güncelleme metodu
        Task AddIncomeAsync(decimal amount, CurrencyType currency); // Gelir ekleme
        Task AddExpenseAsync(decimal amount, CurrencyType currency); // Gider ekleme
        Task RemoveIncomeAsync(decimal amount, CurrencyType currency); // Gelir silme
        Task RemoveExpenseAsync(decimal amount, CurrencyType currency); // Gider silme
        Task RecalculateCashRegisterAsync(); // Kasayı tekrar hesapla
    }
}
