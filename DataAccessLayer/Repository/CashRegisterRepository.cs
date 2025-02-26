using BusinessLayer.Common;
using DataAccessLayer.Data;
using EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository
{
    public class CashRegisterRepository : GenericRepository<CashRegister>, ICashRegisterRepository
    {
        private readonly ApplicationDbContext _context;

        public CashRegisterRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        // 📌 1️⃣ Kasayı getir (eğer yoksa null döndür)
        public async Task<CashRegister?> GetCashRegisterAsync()
        {
            return await _context.CashRegisters.FirstOrDefaultAsync();
        }

        // 📌 2️⃣ Kasayı güncelle
        public async Task UpdateCashRegisterAsync(CashRegister cashRegister)
        {
            _context.CashRegisters.Update(cashRegister);
            await _context.SaveChangesAsync();
        }

        public async Task<CashRegister?> GetFirstAsync()
        {
            return await _context.CashRegisters.FirstOrDefaultAsync(); // 💡 İlk kayıt döndürülüyor
        }
    }
}
