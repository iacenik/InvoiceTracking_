using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository
{
   public  interface IGenericRepository
    {
        public interface IGenericRepository<T> where T : class
        {
            // Temel CRUD İşlemleri
            T GetById(int id);
            Task<T> GetByIdAsync(int id);
            IQueryable<T> GetAll();
            IQueryable<T> GetWhere(Expression<Func<T, bool>> predicate);
            void Add(T entity);
            Task AddAsync(T entity);
            void Update(T entity);
            void Delete(T entity);
            void DeleteById(int id);

            // Toplu işlemler
            void AddRange(IEnumerable<T> entities);
            Task AddRangeAsync(IEnumerable<T> entities);
            void UpdateRange(IEnumerable<T> entities);
            void DeleteRange(IEnumerable<T> entities);
        }

        // CashRegister Repository Interface
        public interface ICashRegisterRepository : IGenericRepository<EntityLayer.Entities.CashRegister>
        {
            EntityLayer.Entities.CashRegister GetCashRegister();
            Task<EntityLayer.Entities.CashRegister> GetCashRegisterAsync();
            decimal GetTotalBalanceInEUR();
            decimal GetTotalBalanceInRON();
            decimal GetTotalBalanceInUSD();
        }

        // Client Repository Interface
        public interface IClientRepository : IGenericRepository<EntityLayer.Entities.Client>
        {
            IQueryable<EntityLayer.Entities.Client> GetClientsWithPaymentsAndInvoices();
            Task<EntityLayer.Entities.Client> GetClientWithDetailsAsync(int clientId);
            IQueryable<EntityLayer.Entities.Client> GetClientsByCountry(string country);
        }

        // Employee Repository Interface
        public interface IEmployeeRepository : IGenericRepository<EntityLayer.Entities.Employee>
        {
            IQueryable<EntityLayer.Entities.Employee> GetEmployeesWithExpenses();
            Task<EntityLayer.Entities.Employee> GetEmployeeWithExpensesAsync(int employeeId);
        }

        // Expense Repository Interface
        public interface IExpenseRepository : IGenericRepository<EntityLayer.Entities.Expense>
        {
            IQueryable<EntityLayer.Entities.Expense> GetExpensesWithCategoryAndEmployee();
            Task<IEnumerable<EntityLayer.Entities.Expense>> GetExpensesByCategoryAsync(int categoryId);
            Task<IEnumerable<EntityLayer.Entities.Expense>> GetExpensesByEmployeeAsync(int employeeId);
            Task<IEnumerable<EntityLayer.Entities.Expense>> GetExpensesByDateRangeAsync(DateTime startDate, DateTime endDate);
            Task<IEnumerable<EntityLayer.Entities.Expense>> GetExpensesByCurrencyAsync(EntityLayer.Entities.Enums.CurrencyType currency);
        }

        // ExpenseCategory Repository Interface
        public interface IExpenseCategoryRepository : IGenericRepository<EntityLayer.Entities.ExpenseCategory>
        {
            IQueryable<EntityLayer.Entities.ExpenseCategory> GetCategoriesWithExpenses();
            Task<EntityLayer.Entities.ExpenseCategory> GetCategoryWithExpensesAsync(int categoryId);
        }

        // Invoice Repository Interface
        public interface IInvoiceRepository : IGenericRepository<EntityLayer.Entities.Invoice>
        {
            IQueryable<EntityLayer.Entities.Invoice> GetInvoicesWithDetails();
            Task<EntityLayer.Entities.Invoice> GetInvoiceWithDetailsAsync(int invoiceId);
            Task<IEnumerable<EntityLayer.Entities.Invoice>> GetInvoicesByClientAsync(int clientId);
            Task<IEnumerable<EntityLayer.Entities.Invoice>> GetInvoicesByDateRangeAsync(DateTime startDate, DateTime endDate);
            Task<IEnumerable<EntityLayer.Entities.Invoice>> GetUnpaidInvoicesAsync();
            Task<IEnumerable<EntityLayer.Entities.Invoice>> GetInvoicesByCurrencyAsync(EntityLayer.Entities.Enums.CurrencyType currency);
            Task<IEnumerable<EntityLayer.Entities.Invoice>> GetInvoicesByTypeAsync(EntityLayer.Entities.Enums.InvoiceType invoiceType);
        }

        // InvoiceDetail Repository Interface
        public interface IInvoiceDetailRepository : IGenericRepository<EntityLayer.Entities.InvoiceDetail>
        {
            IQueryable<EntityLayer.Entities.InvoiceDetail> GetDetailsByInvoiceId(int invoiceId);
        }

        // Item Repository Interface
        public interface IItemRepository : IGenericRepository<EntityLayer.Entities.Item>
        {
            Task<IEnumerable<EntityLayer.Entities.Item>> SearchItemsByNameAsync(string searchTerm);
        }

        // Payment Repository Interface
        public interface IPaymentRepository : IGenericRepository<EntityLayer.Entities.Payment>
        {
            IQueryable<EntityLayer.Entities.Payment> GetPaymentsWithDetails();
            Task<EntityLayer.Entities.Payment> GetPaymentWithDetailsAsync(int paymentId);
            Task<IEnumerable<EntityLayer.Entities.Payment>> GetPaymentsByClientAsync(int clientId);
            Task<IEnumerable<EntityLayer.Entities.Payment>> GetPaymentsByDateRangeAsync(DateTime startDate, DateTime endDate);
            Task<IEnumerable<EntityLayer.Entities.Payment>> GetPaymentsByCurrencyAsync(EntityLayer.Entities.Enums.CurrencyType currency);
        }

        // PaymentDetail Repository Interface
        public interface IPaymentDetailRepository : IGenericRepository<EntityLayer.Entities.PaymentDetail>
        {
            IQueryable<EntityLayer.Entities.PaymentDetail> GetDetailsByPaymentId(int paymentId);
        }
    }
}
