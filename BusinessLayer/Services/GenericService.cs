using DataAccessLayer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static DataAccessLayer.Repository.IGenericRepository;


namespace BusinessLayer.Services
{
    public class GenericService<T> : IGenericService<T> where T : class
    {
        protected readonly IGenericRepository<T> _repository;
        protected readonly IUnitOfWork _unitOfWork;

        public GenericService(IUnitOfWork unitOfWork, IGenericRepository<T> repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public virtual T GetById(int id)
        {
            return _repository.GetById(id);
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _repository.GetAll().ToList();
        }

        public virtual IEnumerable<T> GetWhere(Expression<Func<T, bool>> predicate)
        {
            return _repository.GetWhere(predicate).ToList();
        }

        public virtual void Add(T entity)
        {
            _repository.Add(entity);
            _unitOfWork.Complete();
        }

        public virtual async Task AddAsync(T entity)
        {
            await _repository.AddAsync(entity);
            await _unitOfWork.CompleteAsync();
        }

        public virtual void Update(T entity)
        {
            _repository.Update(entity);
            _unitOfWork.Complete();
        }

        public virtual void Delete(T entity)
        {
            _repository.Delete(entity);
            _unitOfWork.Complete();
        }

        public virtual void DeleteById(int id)
        {
            _repository.DeleteById(id);
            _unitOfWork.Complete();
        }

        public virtual void AddRange(IEnumerable<T> entities)
        {
            _repository.AddRange(entities);
            _unitOfWork.Complete();
        }

        public virtual async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _repository.AddRangeAsync(entities);
            await _unitOfWork.CompleteAsync();
        }

        public virtual void UpdateRange(IEnumerable<T> entities)
        {
            _repository.UpdateRange(entities);
            _unitOfWork.Complete();
        }

        public virtual void DeleteRange(IEnumerable<T> entities)
        {
            _repository.DeleteRange(entities);
            _unitOfWork.Complete();
        }
    }

    // CashRegister Service Implementation
    public class CashRegisterService : GenericService<EntityLayer.Entities.CashRegister>, ICashRegisterService
    {
        private readonly ICashRegisterRepository _cashRegisterRepository;

        public CashRegisterService(IUnitOfWork unitOfWork, ICashRegisterRepository repository)
            : base(unitOfWork, repository)
        {
            _cashRegisterRepository = repository;
        }

        public EntityLayer.Entities.CashRegister GetCashRegister()
        {
            return _cashRegisterRepository.GetCashRegister();
        }

        public async Task<EntityLayer.Entities.CashRegister> GetCashRegisterAsync()
        {
            return await _cashRegisterRepository.GetCashRegisterAsync();
        }

        public decimal GetTotalBalanceInEUR()
        {
            return _cashRegisterRepository.GetTotalBalanceInEUR();
        }

        public decimal GetTotalBalanceInRON()
        {
            return _cashRegisterRepository.GetTotalBalanceInRON();
        }

        public decimal GetTotalBalanceInUSD()
        {
            return _cashRegisterRepository.GetTotalBalanceInUSD();
        }

        public void AddIncome(decimal amount, EntityLayer.Entities.Enums.CurrencyType currency)
        {
            var cashRegister = GetCashRegister();
            if (cashRegister != null)
            {
                cashRegister.AddIncome(amount, currency);
                Update(cashRegister);
            }
        }

        public void DeductExpense(decimal amount, EntityLayer.Entities.Enums.CurrencyType currency)
        {
            var cashRegister = GetCashRegister();
            if (cashRegister != null)
            {
                cashRegister.DeductExpense(amount, currency);
                Update(cashRegister);
            }
        }
    }

    // Client Service Implementation
    public class ClientService : GenericService<EntityLayer.Entities.Client>, IClientService
    {
        private readonly IClientRepository _clientRepository;

        public ClientService(IUnitOfWork unitOfWork, IClientRepository repository)
            : base(unitOfWork, repository)
        {
            _clientRepository = repository;
        }

        public IEnumerable<EntityLayer.Entities.Client> GetClientsWithPaymentsAndInvoices()
        {
            return _clientRepository.GetClientsWithPaymentsAndInvoices().ToList();
        }

        public async Task<EntityLayer.Entities.Client> GetClientWithDetailsAsync(int clientId)
        {
            return await _clientRepository.GetClientWithDetailsAsync(clientId);
        }

        public IEnumerable<EntityLayer.Entities.Client> GetClientsByCountry(string country)
        {
            return _clientRepository.GetClientsByCountry(country).ToList();
        }

        public async Task<EntityLayer.Entities.Client> AddNewClientAsync(EntityLayer.Entities.Client client)
        {
            await _clientRepository.AddAsync(client);
            await _unitOfWork.CompleteAsync();
            return client;
        }
    }

    // Employee Service Implementation
    public class EmployeeService : GenericService<EntityLayer.Entities.Employee>, IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IExpenseRepository _expenseRepository;

        public EmployeeService(IUnitOfWork unitOfWork, IEmployeeRepository repository, IExpenseRepository expenseRepository)
            : base(unitOfWork, repository)
        {
            _employeeRepository = repository;
            _expenseRepository = expenseRepository;
        }

        public IEnumerable<EntityLayer.Entities.Employee> GetEmployeesWithExpenses()
        {
            return _employeeRepository.GetEmployeesWithExpenses().ToList();
        }

        public async Task<EntityLayer.Entities.Employee> GetEmployeeWithExpensesAsync(int employeeId)
        {
            return await _employeeRepository.GetEmployeeWithExpensesAsync(employeeId);
        }

        public async Task<decimal> GetTotalExpensesByEmployeeAsync(int employeeId, EntityLayer.Entities.Enums.CurrencyType currency)
        {
            var expenses = await _expenseRepository.GetExpensesByEmployeeAsync(employeeId);
            return expenses
                .Where(e => e.Currency == currency)
                .Sum(e => e.Amount);
        }
    }

    // Expense Service Implementation
    public class ExpenseService : GenericService<EntityLayer.Entities.Expense>, IExpenseService
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly ICashRegisterRepository _cashRegisterRepository;

        public ExpenseService(IUnitOfWork unitOfWork, IExpenseRepository repository, ICashRegisterRepository cashRegisterRepository)
            : base(unitOfWork, repository)
        {
            _expenseRepository = repository;
            _cashRegisterRepository = cashRegisterRepository;
        }

        public IEnumerable<EntityLayer.Entities.Expense> GetExpensesWithCategoryAndEmployee()
        {
            return _expenseRepository.GetExpensesWithCategoryAndEmployee().ToList();
        }

        public async Task<IEnumerable<EntityLayer.Entities.Expense>> GetExpensesByCategoryAsync(int categoryId)
        {
            return await _expenseRepository.GetExpensesByCategoryAsync(categoryId);
        }

        public async Task<IEnumerable<EntityLayer.Entities.Expense>> GetExpensesByEmployeeAsync(int employeeId)
        {
            return await _expenseRepository.GetExpensesByEmployeeAsync(employeeId);
        }

        public async Task<IEnumerable<EntityLayer.Entities.Expense>> GetExpensesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _expenseRepository.GetExpensesByDateRangeAsync(startDate, endDate);
        }

        public async Task<IEnumerable<EntityLayer.Entities.Expense>> GetExpensesByCurrencyAsync(EntityLayer.Entities.Enums.CurrencyType currency)
        {
            return await _expenseRepository.GetExpensesByCurrencyAsync(currency);
        }

        public async Task<EntityLayer.Entities.Expense> AddExpenseWithCashRegisterUpdateAsync(EntityLayer.Entities.Expense expense)
        {
            // İlk önce gideri ekle
            await _expenseRepository.AddAsync(expense);

            // Kasadan düş
            var cashRegister = await _cashRegisterRepository.GetCashRegisterAsync();
            if (cashRegister != null)
            {
                cashRegister.DeductExpense(expense.Amount, expense.Currency);
                _cashRegisterRepository.Update(cashRegister);
            }

            await _unitOfWork.CompleteAsync();
            return expense;
        }
    }

    // ExpenseCategory Service Implementation
    public class ExpenseCategoryService : GenericService<EntityLayer.Entities.ExpenseCategory>, IExpenseCategoryService
    {
        private readonly IExpenseCategoryRepository _expenseCategoryRepository;
        private readonly IExpenseRepository _expenseRepository;

        public ExpenseCategoryService(IUnitOfWork unitOfWork, IExpenseCategoryRepository repository, IExpenseRepository expenseRepository)
            : base(unitOfWork, repository)
        {
            _expenseCategoryRepository = repository;
            _expenseRepository = expenseRepository;
        }

        public IEnumerable<EntityLayer.Entities.ExpenseCategory> GetCategoriesWithExpenses()
        {
            return _expenseCategoryRepository.GetCategoriesWithExpenses().ToList();
        }

        public async Task<EntityLayer.Entities.ExpenseCategory> GetCategoryWithExpensesAsync(int categoryId)
        {
            return await _expenseCategoryRepository.GetCategoryWithExpensesAsync(categoryId);
        }

        public async Task<decimal> GetTotalExpensesByCategoryAsync(int categoryId, EntityLayer.Entities.Enums.CurrencyType currency)
        {
            var expenses = await _expenseRepository.GetExpensesByCategoryAsync(categoryId);
            return expenses
                .Where(e => e.Currency == currency)
                .Sum(e => e.Amount);
        }
    }

    // Invoice Service Implementation
    public class InvoiceService : GenericService<EntityLayer.Entities.Invoice>, IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IInvoiceDetailRepository _invoiceDetailRepository;
        private readonly ICashRegisterRepository _cashRegisterRepository;

        public InvoiceService(IUnitOfWork unitOfWork, IInvoiceRepository repository,
            IInvoiceDetailRepository invoiceDetailRepository, ICashRegisterRepository cashRegisterRepository)
            : base(unitOfWork, repository)
        {
            _invoiceRepository = repository;
            _invoiceDetailRepository = invoiceDetailRepository;
            _cashRegisterRepository = cashRegisterRepository;
        }

        public IEnumerable<EntityLayer.Entities.Invoice> GetInvoicesWithDetails()
        {
            return _invoiceRepository.GetInvoicesWithDetails().ToList();
        }

        public async Task<EntityLayer.Entities.Invoice> GetInvoiceWithDetailsAsync(int invoiceId)
        {
            return await _invoiceRepository.GetInvoiceWithDetailsAsync(invoiceId);
        }

        public async Task<IEnumerable<EntityLayer.Entities.Invoice>> GetInvoicesByClientAsync(int clientId)
        {
            return await _invoiceRepository.GetInvoicesByClientAsync(clientId);
        }

        public async Task<IEnumerable<EntityLayer.Entities.Invoice>> GetInvoicesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _invoiceRepository.GetInvoicesByDateRangeAsync(startDate, endDate);
        }

        public async Task<IEnumerable<EntityLayer.Entities.Invoice>> GetUnpaidInvoicesAsync()
        {
            return await _invoiceRepository.GetUnpaidInvoicesAsync();
        }

        public async Task<IEnumerable<EntityLayer.Entities.Invoice>> GetInvoicesByCurrencyAsync(EntityLayer.Entities.Enums.CurrencyType currency)
        {
            return await _invoiceRepository.GetInvoicesByCurrencyAsync(currency);
        }

        public async Task<IEnumerable<EntityLayer.Entities.Invoice>> GetInvoicesByTypeAsync(EntityLayer.Entities.Enums.InvoiceType invoiceType)
        {
            return await _invoiceRepository.GetInvoicesByTypeAsync(invoiceType);
        }

        public async Task<EntityLayer.Entities.Invoice> CreateInvoiceWithDetailsAsync(EntityLayer.Entities.Invoice invoice, List<EntityLayer.Entities.InvoiceDetail> details)
        {
            try
            {
                Console.WriteLine($"Creating invoice with {details?.Count ?? 0} details");
                Console.WriteLine($"Invoice Currency: {invoice.Currency}");

                await _invoiceRepository.AddAsync(invoice);
                await _unitOfWork.CompleteAsync(); // Invoice ID oluşması için kaydet

                Console.WriteLine($"Invoice created with ID: {invoice.InvoiceId}");

                foreach (var detail in details)
                {
                    detail.InvoiceId = invoice.InvoiceId;
                    detail.Currency = invoice.Currency; // Para birimini faturadan al
                    
                    // Ürünleri kontrol et
                    var products = detail.SoldProductsList;
                    Console.WriteLine($"Detail has {products.Count} products");
                    foreach (var product in products)
                    {
                        Console.WriteLine($"Product before save: Name={product.ProductName}, Price={product.UnitPrice}, Currency={product.Currency}, Total={product.TotalPrice}");
                    }

                    await _invoiceDetailRepository.AddAsync(detail);
                }

                await _unitOfWork.CompleteAsync();

                // Faturayı detaylarıyla birlikte tekrar yükle
                var savedInvoice = await _invoiceRepository.GetInvoiceWithDetailsAsync(invoice.InvoiceId);
                Console.WriteLine($"Saved invoice total amount: {savedInvoice?.TotalAmount ?? 0}");

                return savedInvoice;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CreateInvoiceWithDetailsAsync: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<bool> ApproveInvoiceAsync(int invoiceId)
        {
            try
            {
                // Faturayı detaylarıyla birlikte al
                var invoice = await _invoiceRepository.GetInvoiceWithDetailsAsync(invoiceId);
                if (invoice == null || invoice.IsPaid)
                {
                    Console.WriteLine($"Invoice {invoiceId} is either null or already paid");
                    return false;
                }

                // Kasayı al
                var cashRegister = await _cashRegisterRepository.GetCashRegisterAsync();
                if (cashRegister == null)
                {
                    Console.WriteLine("Cash register not found");
                    return false;
                }

                // Tek transaction içinde hem faturayı hem kasayı güncelle
                invoice.IsPaid = true;
                _invoiceRepository.Update(invoice);

                cashRegister.DeductExpense(invoice.TotalAmount, invoice.Currency);
                _cashRegisterRepository.Update(cashRegister);

                // Tek seferde kaydet
                await _unitOfWork.CompleteAsync();

                Console.WriteLine($"Invoice {invoiceId} approved and cash register updated: Amount={invoice.TotalAmount} {invoice.Currency}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ApproveInvoiceAsync: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return false;
            }
        }
    }

    // InvoiceDetail Service Implementation
    public class InvoiceDetailService : GenericService<EntityLayer.Entities.InvoiceDetail>, IInvoiceDetailService
    {
        private readonly IInvoiceDetailRepository _invoiceDetailRepository;

        public InvoiceDetailService(IUnitOfWork unitOfWork, IInvoiceDetailRepository repository)
            : base(unitOfWork, repository)
        {
            _invoiceDetailRepository = repository;
        }

        public IEnumerable<EntityLayer.Entities.InvoiceDetail> GetDetailsByInvoiceId(int invoiceId)
        {
            return _invoiceDetailRepository.GetDetailsByInvoiceId(invoiceId).ToList();
        }

        public async Task<EntityLayer.Entities.InvoiceDetail> AddInvoiceDetailWithProductsAsync(int invoiceId, List<EntityLayer.Entities.SoldProduct> products)
        {
            var invoiceDetail = new EntityLayer.Entities.InvoiceDetail
            {
                InvoiceId = invoiceId,
                Currency = products.FirstOrDefault()?.Currency ?? EntityLayer.Entities.Enums.CurrencyType.EUR // Varsayılan para birimi
            };

            invoiceDetail.SetSoldProducts(products);

            await _invoiceDetailRepository.AddAsync(invoiceDetail);
            await _unitOfWork.CompleteAsync();

            return invoiceDetail;
        }

        public async Task<bool> UpdateSoldProductsAsync(int invoiceDetailId, List<EntityLayer.Entities.SoldProduct> products)
        {
            var invoiceDetail = await _invoiceDetailRepository.GetByIdAsync(invoiceDetailId);
            if (invoiceDetail == null)
            {
                return false;
            }

            invoiceDetail.SetSoldProducts(products);
            _invoiceDetailRepository.Update(invoiceDetail);
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }

    // Item Service Implementation
    public class ItemService : GenericService<EntityLayer.Entities.Item>, IItemService
    {
        private readonly IItemRepository _itemRepository;

        public ItemService(IUnitOfWork unitOfWork, IItemRepository repository)
            : base(unitOfWork, repository)
        {
            _itemRepository = repository;
        }

        public async Task<IEnumerable<EntityLayer.Entities.Item>> SearchItemsByNameAsync(string searchTerm)
        {
            return await _itemRepository.SearchItemsByNameAsync(searchTerm);
        }
    }

    // Payment Service Implementation
    public class PaymentService : GenericService<EntityLayer.Entities.Payment>, IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IPaymentDetailRepository _paymentDetailRepository;
        private readonly ICashRegisterRepository _cashRegisterRepository;

        public PaymentService(IUnitOfWork unitOfWork, IPaymentRepository repository,
            IPaymentDetailRepository paymentDetailRepository, ICashRegisterRepository cashRegisterRepository)
            : base(unitOfWork, repository)
        {
            _paymentRepository = repository;
            _paymentDetailRepository = paymentDetailRepository;
            _cashRegisterRepository = cashRegisterRepository;
        }

        public IEnumerable<EntityLayer.Entities.Payment> GetPaymentsWithDetails()
        {
            return _paymentRepository.GetPaymentsWithDetails().ToList();
        }

        public async Task<EntityLayer.Entities.Payment> GetPaymentWithDetailsAsync(int paymentId)
        {
            return await _paymentRepository.GetPaymentWithDetailsAsync(paymentId);
        }

        public async Task<IEnumerable<EntityLayer.Entities.Payment>> GetPaymentsByClientAsync(int clientId)
        {
            return await _paymentRepository.GetPaymentsByClientAsync(clientId);
        }

        public async Task<IEnumerable<EntityLayer.Entities.Payment>> GetPaymentsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _paymentRepository.GetPaymentsByDateRangeAsync(startDate, endDate);
        }

        public async Task<IEnumerable<EntityLayer.Entities.Payment>> GetPaymentsByCurrencyAsync(EntityLayer.Entities.Enums.CurrencyType currency)
        {
            return await _paymentRepository.GetPaymentsByCurrencyAsync(currency);
        }

        public async Task<EntityLayer.Entities.Payment> CreatePaymentWithDetailsAsync(EntityLayer.Entities.Payment payment, List<EntityLayer.Entities.PaymentDetail> details)
        {
            // Ödemeyi ekle
            await _paymentRepository.AddAsync(payment);
            await _unitOfWork.CompleteAsync(); // Payment ID oluşması için kaydet

            // Ödeme detaylarını ekle
            foreach (var detail in details)
            {
                detail.PaymentId = payment.PaymentId;
                await _paymentDetailRepository.AddAsync(detail);
            }

            await _unitOfWork.CompleteAsync();

            // Güncel ödemeyi al (detaylarla birlikte)
            payment = await _paymentRepository.GetPaymentWithDetailsAsync(payment.PaymentId);

            // Kasaya gelir ekle
            var cashRegister = await _cashRegisterRepository.GetCashRegisterAsync();
            if (cashRegister != null)
            {
                cashRegister.AddIncome(payment.TotalAmount, payment.Currency);
                _cashRegisterRepository.Update(cashRegister);
            }

            await _unitOfWork.CompleteAsync();
            return payment;
        }

        public async Task<decimal> GetTotalPaymentsByClientAsync(int clientId, EntityLayer.Entities.Enums.CurrencyType currency)
        {
            var payments = await _paymentRepository.GetPaymentsByClientAsync(clientId);
            return payments.Where(p => p.Currency == currency).Sum(p => p.TotalAmount);
        }
    }

    // PaymentDetail Service Implementation
    public class PaymentDetailService : GenericService<EntityLayer.Entities.PaymentDetail>, IPaymentDetailService
    {
        private readonly IPaymentDetailRepository _paymentDetailRepository;

        public PaymentDetailService(IUnitOfWork unitOfWork, IPaymentDetailRepository repository)
            : base(unitOfWork, repository)
        {
            _paymentDetailRepository = repository;
        }

        public IEnumerable<EntityLayer.Entities.PaymentDetail> GetDetailsByPaymentId(int paymentId)
        {
            return _paymentDetailRepository.GetDetailsByPaymentId(paymentId).ToList();
        }

        public async Task<EntityLayer.Entities.PaymentDetail> AddPaymentDetailWithProductsAsync(int paymentId, List<EntityLayer.Entities.SoldProduct> products)
        {
            var paymentDetail = new EntityLayer.Entities.PaymentDetail
            {
                PaymentId = paymentId,
                Currency = products.FirstOrDefault()?.Currency ?? EntityLayer.Entities.Enums.CurrencyType.EUR // Varsayılan para birimi
            };

            paymentDetail.SetSoldProducts(products);

            await _paymentDetailRepository.AddAsync(paymentDetail);
            await _unitOfWork.CompleteAsync();

            return paymentDetail;
        }

        public async Task<bool> UpdateSoldProductsAsync(int paymentDetailId, List<EntityLayer.Entities.SoldProduct> products)
        {
            var paymentDetail = await _paymentDetailRepository.GetByIdAsync(paymentDetailId);
            if (paymentDetail == null)
            {
                return false;
            }

            paymentDetail.SetSoldProducts(products);
            _paymentDetailRepository.Update(paymentDetail);
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}
