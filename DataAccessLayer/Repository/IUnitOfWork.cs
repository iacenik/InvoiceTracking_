using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataAccessLayer.Repository.IGenericRepository;

namespace DataAccessLayer.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        // Repository Properties
        ICashRegisterRepository CashRegisters { get; }
        IClientRepository Clients { get; }
        IEmployeeRepository Employees { get; }
        IExpenseRepository Expenses { get; }
        IExpenseCategoryRepository ExpenseCategories { get; }
        IInvoiceRepository Invoices { get; }
        IInvoiceDetailRepository InvoiceDetails { get; }
        IItemRepository Items { get; }
        IPaymentRepository Payments { get; }
        IPaymentDetailRepository PaymentDetails { get; }

        // Save Methods
        int Complete();
        Task<int> CompleteAsync();
    }
}
