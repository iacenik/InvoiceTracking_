using DataAccessLayer.Concrete;
using DataAccessLayer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataAccessLayer.Repository.IGenericRepository;

namespace DataAccessLayer.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private bool _disposed = false;

        // Repository instances
        private ICashRegisterRepository _cashRegisters;
        private IClientRepository _clients;
        private IEmployeeRepository _employees;
        private IExpenseRepository _expenses;
        private IExpenseCategoryRepository _expenseCategories;
        private IInvoiceRepository _invoices;
        private IInvoiceDetailRepository _invoiceDetails;
        private IItemRepository _items;
        private IPaymentRepository _payments;
        private IPaymentDetailRepository _paymentDetails;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        // CashRegister Repository
        public ICashRegisterRepository CashRegisters =>
            _cashRegisters ??= new CashRegisterRepository(_context);

        // Client Repository
        public IClientRepository Clients =>
            _clients ??= new ClientRepository(_context);

        // Employee Repository
        public IEmployeeRepository Employees =>
            _employees ??= new EmployeeRepository(_context);

        // Expense Repository
        public IExpenseRepository Expenses =>
            _expenses ??= new ExpenseRepository(_context);

        // ExpenseCategory Repository
        public IExpenseCategoryRepository ExpenseCategories =>
            _expenseCategories ??= new ExpenseCategoryRepository(_context);

        // Invoice Repository
        public IInvoiceRepository Invoices =>
            _invoices ??= new InvoiceRepository(_context);

        // InvoiceDetail Repository
        public IInvoiceDetailRepository InvoiceDetails =>
            _invoiceDetails ??= new InvoiceDetailRepository(_context);

        // Item Repository
        public IItemRepository Items =>
            _items ??= new ItemRepository(_context);

        // Payment Repository
        public IPaymentRepository Payments =>
            _payments ??= new PaymentRepository(_context);

        // PaymentDetail Repository
        public IPaymentDetailRepository PaymentDetails =>
            _paymentDetails ??= new PaymentDetailRepository(_context);

        // Save Methods
        public int Complete()
        {
            return _context.SaveChanges();
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        // Dispose Methods
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

}
