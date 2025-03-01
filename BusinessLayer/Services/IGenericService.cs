using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public interface IGenericService<T> where T : class
    {
        // Temel CRUD İşlemleri
        T GetById(int id);
        Task<T> GetByIdAsync(int id);
        IEnumerable<T> GetAll();
        IEnumerable<T> GetWhere(Expression<Func<T, bool>> predicate);
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

    // CashRegister Service Interface
    public interface ICashRegisterService : IGenericService<EntityLayer.Entities.CashRegister>
    {
        EntityLayer.Entities.CashRegister GetCashRegister();
        Task<EntityLayer.Entities.CashRegister> GetCashRegisterAsync();
        decimal GetTotalBalanceInEUR();
        decimal GetTotalBalanceInRON();
        decimal GetTotalBalanceInUSD();
        void AddIncome(decimal amount, EntityLayer.Entities.Enums.CurrencyType currency);
        void DeductExpense(decimal amount, EntityLayer.Entities.Enums.CurrencyType currency);
    }

    // Client Service Interface
    public interface IClientService : IGenericService<EntityLayer.Entities.Client>
    {
        IEnumerable<EntityLayer.Entities.Client> GetClientsWithPaymentsAndInvoices();
        Task<EntityLayer.Entities.Client> GetClientWithDetailsAsync(int clientId);
        IEnumerable<EntityLayer.Entities.Client> GetClientsByCountry(string country);
        Task<EntityLayer.Entities.Client> AddNewClientAsync(EntityLayer.Entities.Client client);
    }

    // Employee Service Interface
    public interface IEmployeeService : IGenericService<EntityLayer.Entities.Employee>
    {
        IEnumerable<EntityLayer.Entities.Employee> GetEmployeesWithExpenses();
        Task<EntityLayer.Entities.Employee> GetEmployeeWithExpensesAsync(int employeeId);
        Task<decimal> GetTotalExpensesByEmployeeAsync(int employeeId, EntityLayer.Entities.Enums.CurrencyType currency);
    }

    // Expense Service Interface
    public interface IExpenseService : IGenericService<EntityLayer.Entities.Expense>
    {
        IEnumerable<EntityLayer.Entities.Expense> GetExpensesWithCategoryAndEmployee();
        Task<IEnumerable<EntityLayer.Entities.Expense>> GetExpensesByCategoryAsync(int categoryId);
        Task<IEnumerable<EntityLayer.Entities.Expense>> GetExpensesByEmployeeAsync(int employeeId);
        Task<IEnumerable<EntityLayer.Entities.Expense>> GetExpensesByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<EntityLayer.Entities.Expense>> GetExpensesByCurrencyAsync(EntityLayer.Entities.Enums.CurrencyType currency);
        Task<EntityLayer.Entities.Expense> AddExpenseWithCashRegisterUpdateAsync(EntityLayer.Entities.Expense expense);
    }

    // ExpenseCategory Service Interface
    public interface IExpenseCategoryService : IGenericService<EntityLayer.Entities.ExpenseCategory>
    {
        IEnumerable<EntityLayer.Entities.ExpenseCategory> GetCategoriesWithExpenses();
        Task<EntityLayer.Entities.ExpenseCategory> GetCategoryWithExpensesAsync(int categoryId);
        Task<decimal> GetTotalExpensesByCategoryAsync(int categoryId, EntityLayer.Entities.Enums.CurrencyType currency);
    }

    // Invoice Service Interface
    public interface IInvoiceService : IGenericService<EntityLayer.Entities.Invoice>
    {
        IEnumerable<EntityLayer.Entities.Invoice> GetInvoicesWithDetails();
        Task<EntityLayer.Entities.Invoice> GetInvoiceWithDetailsAsync(int invoiceId);
        Task<IEnumerable<EntityLayer.Entities.Invoice>> GetInvoicesByClientAsync(int clientId);
        Task<IEnumerable<EntityLayer.Entities.Invoice>> GetInvoicesByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<EntityLayer.Entities.Invoice>> GetUnpaidInvoicesAsync();
        Task<IEnumerable<EntityLayer.Entities.Invoice>> GetInvoicesByCurrencyAsync(EntityLayer.Entities.Enums.CurrencyType currency);
        Task<IEnumerable<EntityLayer.Entities.Invoice>> GetInvoicesByTypeAsync(EntityLayer.Entities.Enums.InvoiceType invoiceType);
        Task<EntityLayer.Entities.Invoice> CreateInvoiceWithDetailsAsync(EntityLayer.Entities.Invoice invoice, List<EntityLayer.Entities.InvoiceDetail> details);
        Task<bool> ApproveInvoiceAsync(int invoiceId);
    }

    // InvoiceDetail Service Interface
    public interface IInvoiceDetailService : IGenericService<EntityLayer.Entities.InvoiceDetail>
    {
        IEnumerable<EntityLayer.Entities.InvoiceDetail> GetDetailsByInvoiceId(int invoiceId);
        Task<EntityLayer.Entities.InvoiceDetail> AddInvoiceDetailWithProductsAsync(int invoiceId, List<EntityLayer.Entities.SoldProduct> products);
        Task<bool> UpdateSoldProductsAsync(int invoiceDetailId, List<EntityLayer.Entities.SoldProduct> products);
    }

    // Item Service Interface
    public interface IItemService : IGenericService<EntityLayer.Entities.Item>
    {
        Task<IEnumerable<EntityLayer.Entities.Item>> SearchItemsByNameAsync(string searchTerm);
    }

    // Payment Service Interface
    public interface IPaymentService : IGenericService<EntityLayer.Entities.Payment>
    {
        IEnumerable<EntityLayer.Entities.Payment> GetPaymentsWithDetails();
        Task<EntityLayer.Entities.Payment> GetPaymentWithDetailsAsync(int paymentId);
        Task<IEnumerable<EntityLayer.Entities.Payment>> GetPaymentsByClientAsync(int clientId);
        Task<IEnumerable<EntityLayer.Entities.Payment>> GetPaymentsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<EntityLayer.Entities.Payment>> GetPaymentsByCurrencyAsync(EntityLayer.Entities.Enums.CurrencyType currency);
        Task<EntityLayer.Entities.Payment> CreatePaymentWithDetailsAsync(EntityLayer.Entities.Payment payment, List<EntityLayer.Entities.PaymentDetail> details);
        Task<decimal> GetTotalPaymentsByClientAsync(int clientId, EntityLayer.Entities.Enums.CurrencyType currency);
    }

    // PaymentDetail Service Interface
    public interface IPaymentDetailService : IGenericService<EntityLayer.Entities.PaymentDetail>
    {
        IEnumerable<EntityLayer.Entities.PaymentDetail> GetDetailsByPaymentId(int paymentId);
        Task<EntityLayer.Entities.PaymentDetail> AddPaymentDetailWithProductsAsync(int paymentId, List<EntityLayer.Entities.SoldProduct> products);
        Task<bool> UpdateSoldProductsAsync(int paymentDetailId, List<EntityLayer.Entities.SoldProduct> products);
    }
}

