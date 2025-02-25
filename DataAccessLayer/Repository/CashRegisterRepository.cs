using BusinessLayer.Common;
using DataAccessLayer.Data;
using EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using static EntityLayer.Entities.Enums;

namespace DataAccessLayer.Repository
{
    public class CashRegisterRepository : GenericRepository<CashRegister>, ICashRegisterRepository
    {
        private readonly ApplicationDbContext _context;

        public CashRegisterRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        // 📌 Kasa bilgisini getir (Eğer kayıt yoksa yeni oluştur)
        public async Task<CashRegister> GetCashRegisterAsync()
        {
            var cashRegister = await _context.CashRegisters.FirstOrDefaultAsync();

            if (cashRegister == null) // Eğer veritabanında hiç kayıt yoksa
            {
                cashRegister = new CashRegister();
                await _context.CashRegisters.AddAsync(cashRegister);
                await _context.SaveChangesAsync();
            }

            return cashRegister;
        }

        // 📌 Kasa Güncelleme
        public async Task UpdateCashRegisterAsync(CashRegister cashRegister)
        {
            _context.CashRegisters.Update(cashRegister);
            await _context.SaveChangesAsync();
        }

        // 📌 GELİR EKLE (Ödeme Eklendiğinde)
        public async Task AddIncomeAsync(decimal amount, CurrencyType currency)
        {
            var cashRegister = await GetCashRegisterAsync();

            switch (currency)
            {
                case CurrencyType.EUR:
                    cashRegister.TotalIncomeEUR += amount;
                    break;
                case CurrencyType.RON:
                    cashRegister.TotalIncomeRON += amount;
                    break;
                case CurrencyType.DOLAR:
                    cashRegister.TotalIncomeUSD += amount;
                    break;
            }

            await UpdateCashRegisterAsync(cashRegister);
        }

        // 📌 GELİR SİL (Ödeme Silindiğinde veya Güncellendiğinde Eski Tutarı Düş)
        public async Task RemoveIncomeAsync(decimal amount, CurrencyType currency)
        {
            var cashRegister = await GetCashRegisterAsync();

            switch (currency)
            {
                case CurrencyType.EUR:
                    cashRegister.TotalIncomeEUR -= amount;
                    break;
                case CurrencyType.RON:
                    cashRegister.TotalIncomeRON -= amount;
                    break;
                case CurrencyType.DOLAR:
                    cashRegister.TotalIncomeUSD -= amount;
                    break;
            }

            await UpdateCashRegisterAsync(cashRegister);
        }

        // 📌 GİDER EKLE (Gider Eklendiğinde)
        public async Task AddExpenseAsync(decimal amount, CurrencyType currency)
        {
            var cashRegister = await GetCashRegisterAsync();

            switch (currency)
            {
                case CurrencyType.EUR:
                    cashRegister.TotalExpenseEUR += amount;
                    break;
                case CurrencyType.RON:
                    cashRegister.TotalExpenseRON += amount;
                    break;
                case CurrencyType.DOLAR:
                    cashRegister.TotalExpenseUSD += amount;
                    break;
            }

            await UpdateCashRegisterAsync(cashRegister);
        }

        // 📌 GİDER SİL (Gider Silindiğinde veya Güncellendiğinde Eski Tutarı Geri Ekle)
        public async Task RemoveExpenseAsync(decimal amount, CurrencyType currency)
        {
            var cashRegister = await GetCashRegisterAsync();

            switch (currency)
            {
                case CurrencyType.EUR:
                    cashRegister.TotalExpenseEUR -= amount;
                    break;
                case CurrencyType.RON:
                    cashRegister.TotalExpenseRON -= amount;
                    break;
                case CurrencyType.DOLAR:
                    cashRegister.TotalExpenseUSD -= amount;
                    break;
            }

            await UpdateCashRegisterAsync(cashRegister);
        }

        // 📌 KASAYI BAŞTAN HESAPLA (Tüm Ödeme ve Giderleri Yeniden Hesapla)
        public async Task RecalculateCashRegisterAsync()
        {
            var cashRegister = await GetCashRegisterAsync();

            // 📌 Tüm ödemeleri topla
            cashRegister.TotalIncomeEUR = await _context.Payments
                .Where(p => p.Currency == CurrencyType.EUR)
                .SumAsync(p => p.Amount);

            cashRegister.TotalIncomeRON = await _context.Payments
                .Where(p => p.Currency == CurrencyType.RON)
                .SumAsync(p => p.Amount);

            cashRegister.TotalIncomeUSD = await _context.Payments
                .Where(p => p.Currency == CurrencyType.DOLAR)
                .SumAsync(p => p.Amount);

            // 📌 Tüm giderleri topla
            cashRegister.TotalExpenseEUR = await _context.Expenses
                .Where(e => e.Currency == CurrencyType.EUR)
                .SumAsync(e => e.Amount);

            cashRegister.TotalExpenseRON = await _context.Expenses
                .Where(e => e.Currency == CurrencyType.RON)
                .SumAsync(e => e.Amount);

            cashRegister.TotalExpenseUSD = await _context.Expenses
                .Where(e => e.Currency == CurrencyType.DOLAR)
                .SumAsync(e => e.Amount);

            await UpdateCashRegisterAsync(cashRegister);
        }
    }
}
