using DataAccessLayer.Data;
using DataAccessLayer.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static DataAccessLayer.Repository.IGenericRepository;

namespace DataAccessLayer.Concrete
{
    public class GenericRepository<T>:IGenericRepository<T> where T:class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public T GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet;
        }

        public IQueryable<T> GetWhere(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            _dbSet.Remove(entity);
        }

        public void DeleteById(int id)
        {
            var entity = GetById(id);
            if (entity != null)
            {
                Delete(entity);
            }
        }

        public void AddRange(IEnumerable<T> entities)
        {
            _dbSet.AddRange(entities);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                Update(entity);
            }
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }
    }

    // CashRegister Repository Implementation
    public class CashRegisterRepository : GenericRepository<EntityLayer.Entities.CashRegister>, ICashRegisterRepository
    {
        public CashRegisterRepository(ApplicationDbContext context) : base(context) { }

        public EntityLayer.Entities.CashRegister GetCashRegister()
        {
            // Sistemde sadece tek bir kasa olduğunu varsayıyoruz
            return _dbSet.FirstOrDefault();
        }

        public async Task<EntityLayer.Entities.CashRegister> GetCashRegisterAsync()
        {
            return await _dbSet.FirstOrDefaultAsync();
        }

        public decimal GetTotalBalanceInEUR()
        {
            var cashRegister = GetCashRegister();
            return cashRegister?.BalanceEUR ?? 0;
        }

        public decimal GetTotalBalanceInRON()
        {
            var cashRegister = GetCashRegister();
            return cashRegister?.BalanceRON ?? 0;
        }

        public decimal GetTotalBalanceInUSD()
        {
            var cashRegister = GetCashRegister();
            return cashRegister?.BalanceUSD ?? 0;
        }
    }

    // Client Repository Implementation
    public class ClientRepository : GenericRepository<EntityLayer.Entities.Client>, IClientRepository
    {
        public ClientRepository(ApplicationDbContext context) : base(context) { }

        public IQueryable<EntityLayer.Entities.Client> GetClientsWithPaymentsAndInvoices()
        {
            return _dbSet
                .Include(c => c.Payments)
                .Include(c => c.Invoices);
        }

        public async Task<EntityLayer.Entities.Client> GetClientWithDetailsAsync(int clientId)
        {
            return await _dbSet
                .Include(c => c.Payments)
                .Include(c => c.Invoices)
                .FirstOrDefaultAsync(c => c.ClientId == clientId);
        }

        public IQueryable<EntityLayer.Entities.Client> GetClientsByCountry(string country)
        {
            return _dbSet.Where(c => c.Country == country);
        }
    }

    // Employee Repository Implementation
    public class EmployeeRepository : GenericRepository<EntityLayer.Entities.Employee>, IEmployeeRepository
    {
        public EmployeeRepository(ApplicationDbContext context) : base(context) { }

        public IQueryable<EntityLayer.Entities.Employee> GetEmployeesWithExpenses()
        {
            return _dbSet.Include(e => e.Expenses);
        }

        public async Task<EntityLayer.Entities.Employee> GetEmployeeWithExpensesAsync(int employeeId)
        {
            return await _dbSet
                .Include(e => e.Expenses)
                .FirstOrDefaultAsync(e => e.EmployeeId == employeeId);
        }
    }

    // Expense Repository Implementation
    public class ExpenseRepository : GenericRepository<EntityLayer.Entities.Expense>, IExpenseRepository
    {
        public ExpenseRepository(ApplicationDbContext context) : base(context) { }

        public IQueryable<EntityLayer.Entities.Expense> GetExpensesWithCategoryAndEmployee()
        {
            return _dbSet
                .Include(e => e.Category)
                .Include(e => e.Employee);
        }

        public async Task<IEnumerable<EntityLayer.Entities.Expense>> GetExpensesByCategoryAsync(int categoryId)
        {
            return await _dbSet
                .Where(e => e.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<IEnumerable<EntityLayer.Entities.Expense>> GetExpensesByEmployeeAsync(int employeeId)
        {
            return await _dbSet
                .Where(e => e.EmployeeId == employeeId)
                .ToListAsync();
        }

        public async Task<IEnumerable<EntityLayer.Entities.Expense>> GetExpensesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Where(e => e.Date >= startDate && e.Date <= endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<EntityLayer.Entities.Expense>> GetExpensesByCurrencyAsync(EntityLayer.Entities.Enums.CurrencyType currency)
        {
            return await _dbSet
                .Where(e => e.Currency == currency)
                .ToListAsync();
        }
    }

    // ExpenseCategory Repository Implementation
    public class ExpenseCategoryRepository : GenericRepository<EntityLayer.Entities.ExpenseCategory>, IExpenseCategoryRepository
    {
        public ExpenseCategoryRepository(ApplicationDbContext context) : base(context) { }

        public IQueryable<EntityLayer.Entities.ExpenseCategory> GetCategoriesWithExpenses()
        {
            return _dbSet
                .Include(c => c.Expenses);
        }

        public async Task<EntityLayer.Entities.ExpenseCategory> GetCategoryWithExpensesAsync(int categoryId)
        {
            return await _dbSet
                .Include(c => c.Expenses)
                .FirstOrDefaultAsync(c => c.CategoryId == categoryId);
        }
    }

    // Invoice Repository Implementation
    public class InvoiceRepository : GenericRepository<EntityLayer.Entities.Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(ApplicationDbContext context) : base(context) { }

        public IQueryable<EntityLayer.Entities.Invoice> GetInvoicesWithDetails()
        {
            return _dbSet
                .Include(i => i.Category)
                .Include(i => i.Employee)
                .Include(i => i.Client)
                .Include(i => i.InvoiceDetails);
        }

        public async Task<EntityLayer.Entities.Invoice> GetInvoiceWithDetailsAsync(int invoiceId)
        {
            return await _dbSet
                .Include(i => i.Category)
                .Include(i => i.Employee)
                .Include(i => i.Client)
                .Include(i => i.InvoiceDetails)
                .FirstOrDefaultAsync(i => i.InvoiceId == invoiceId);
        }

        public async Task<IEnumerable<EntityLayer.Entities.Invoice>> GetInvoicesByClientAsync(int clientId)
        {
            return await _dbSet
                .Where(i => i.ClientId == clientId)
                .ToListAsync();
        }

        public async Task<IEnumerable<EntityLayer.Entities.Invoice>> GetInvoicesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Where(i => i.Date >= startDate && i.Date <= endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<EntityLayer.Entities.Invoice>> GetUnpaidInvoicesAsync()
        {
            return await _dbSet
                .Where(i => !i.IsPaid)
                .ToListAsync();
        }

        public async Task<IEnumerable<EntityLayer.Entities.Invoice>> GetInvoicesByCurrencyAsync(EntityLayer.Entities.Enums.CurrencyType currency)
        {
            return await _dbSet
                .Where(i => i.Currency == currency)
                .ToListAsync();
        }

        public async Task<IEnumerable<EntityLayer.Entities.Invoice>> GetInvoicesByTypeAsync(EntityLayer.Entities.Enums.InvoiceType invoiceType)
        {
            return await _dbSet
                .Where(i => i.InvoiceType == invoiceType)
                .ToListAsync();
        }
    }

    // InvoiceDetail Repository Implementation
    public class InvoiceDetailRepository : GenericRepository<EntityLayer.Entities.InvoiceDetail>, IInvoiceDetailRepository
    {
        public InvoiceDetailRepository(ApplicationDbContext context) : base(context) { }

        public IQueryable<EntityLayer.Entities.InvoiceDetail> GetDetailsByInvoiceId(int invoiceId)
        {
            return _dbSet.Where(d => d.InvoiceId == invoiceId);
        }
    }

    // Item Repository Implementation
    public class ItemRepository : GenericRepository<EntityLayer.Entities.Item>, IItemRepository
    {
        public ItemRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<EntityLayer.Entities.Item>> SearchItemsByNameAsync(string searchTerm)
        {
            return await _dbSet
                .Where(i => i.ItemName.Contains(searchTerm))
                .ToListAsync();
        }
    }

    // Payment Repository Implementation
    public class PaymentRepository : GenericRepository<EntityLayer.Entities.Payment>, IPaymentRepository
    {
        public PaymentRepository(ApplicationDbContext context) : base(context) { }

        public IQueryable<EntityLayer.Entities.Payment> GetPaymentsWithDetails()
        {
            return _dbSet
                .Include(p => p.Client)
                .Include(p => p.PaymentDetails);
        }

        public async Task<EntityLayer.Entities.Payment> GetPaymentWithDetailsAsync(int paymentId)
        {
            return await _dbSet
                .Include(p => p.Client)
                .Include(p => p.PaymentDetails)
                .FirstOrDefaultAsync(p => p.PaymentId == paymentId);
        }

        public async Task<IEnumerable<EntityLayer.Entities.Payment>> GetPaymentsByClientAsync(int clientId)
        {
            return await _dbSet
                .Where(p => p.ClientId == clientId)
                .ToListAsync();
        }

        public async Task<IEnumerable<EntityLayer.Entities.Payment>> GetPaymentsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Where(p => p.Date >= startDate && p.Date <= endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<EntityLayer.Entities.Payment>> GetPaymentsByCurrencyAsync(EntityLayer.Entities.Enums.CurrencyType currency)
        {
            return await _dbSet
                .Where(p => p.Currency == currency)
                .ToListAsync();
        }
    }

    // PaymentDetail Repository Implementation
    public class PaymentDetailRepository : GenericRepository<EntityLayer.Entities.PaymentDetail>, IPaymentDetailRepository
    {
        public PaymentDetailRepository(ApplicationDbContext context) : base(context) { }

        public IQueryable<EntityLayer.Entities.PaymentDetail> GetDetailsByPaymentId(int paymentId)
        {
            return _dbSet.Where(d => d.PaymentId == paymentId);
        }
    }
}

