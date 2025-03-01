using BusinessLayer.Services;
using EntityLayer.Entities;
using InvoiceTracking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InvoiceTracking.Controllers
{
    public class ExpenseController : Controller
    {
        private readonly IExpenseService _expenseService;
        private readonly IExpenseCategoryService _categoryService;
        private readonly IEmployeeService _employeeService;
        private readonly ICashRegisterService _cashRegisterService;

        public ExpenseController(
            IExpenseService expenseService,
            IExpenseCategoryService categoryService,
            IEmployeeService employeeService,
            ICashRegisterService cashRegisterService)
        {
            _expenseService = expenseService;
            _categoryService = categoryService;
            _employeeService = employeeService;
            _cashRegisterService = cashRegisterService;
        }

        public IActionResult Index()
        {
            var expenses = _expenseService.GetExpensesWithCategoryAndEmployee();
            return View(expenses);
        }

        public async Task<IActionResult> Details(int id)
        {
            var expense = await _expenseService.GetByIdAsync(id);
            if (expense == null)
            {
                return NotFound();
            }
            return View(expense);
        }

        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_categoryService.GetAll(), "CategoryId", "CategoryName");
            ViewBag.Employees = new SelectList(_employeeService.GetAll(), "EmployeeId", "EmployeeName");
            ViewBag.CurrencyTypes = new SelectList(Enum.GetValues(typeof(Enums.CurrencyType)));

            return View(new ExpenseViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ExpenseViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var expense = new Expense
                {
                    Amount = viewModel.Amount,
                    Date = viewModel.Date,
                    CategoryId = viewModel.CategoryId,
                    Description = viewModel.Description,
                    Currency = viewModel.Currency,
                    EmployeeId = viewModel.EmployeeId
                };

                await _expenseService.AddExpenseWithCashRegisterUpdateAsync(expense);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = new SelectList(_categoryService.GetAll(), "CategoryId", "CategoryName", viewModel.CategoryId);
            ViewBag.Employees = new SelectList(_employeeService.GetAll(), "EmployeeId", "EmployeeName", viewModel.EmployeeId);
            ViewBag.CurrencyTypes = new SelectList(Enum.GetValues(typeof(Enums.CurrencyType)), viewModel.Currency);

            return View(viewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var expense = await _expenseService.GetByIdAsync(id);
            if (expense == null)
            {
                return NotFound();
            }

            ViewBag.Categories = new SelectList(_categoryService.GetAll(), "CategoryId", "CategoryName", expense.CategoryId);
            ViewBag.Employees = new SelectList(_employeeService.GetAll(), "EmployeeId", "EmployeeName", expense.EmployeeId);
            ViewBag.CurrencyTypes = new SelectList(Enum.GetValues(typeof(Enums.CurrencyType)), expense.Currency);

            var viewModel = new ExpenseViewModel
            {
                ExpenseId = expense.ExpenseId,
                Amount = expense.Amount,
                Date = expense.Date,
                CategoryId = expense.CategoryId,
                Description = expense.Description,
                Currency = expense.Currency,
                EmployeeId = expense.EmployeeId
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ExpenseViewModel viewModel)
        {
            if (id != viewModel.ExpenseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var expense = await _expenseService.GetByIdAsync(id);
                if (expense == null)
                {
                    return NotFound();
                }

                expense.Amount = viewModel.Amount;
                expense.Date = viewModel.Date;
                expense.CategoryId = viewModel.CategoryId;
                expense.Description = viewModel.Description;
                expense.Currency = viewModel.Currency;
                expense.EmployeeId = viewModel.EmployeeId;

                _expenseService.Update(expense);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = new SelectList(_categoryService.GetAll(), "CategoryId", "CategoryName", viewModel.CategoryId);
            ViewBag.Employees = new SelectList(_employeeService.GetAll(), "EmployeeId", "EmployeeName", viewModel.EmployeeId);
            ViewBag.CurrencyTypes = new SelectList(Enum.GetValues(typeof(Enums.CurrencyType)), viewModel.Currency);

            return View(viewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var expense = await _expenseService.GetByIdAsync(id);
            if (expense == null)
            {
                return NotFound();
            }
            return View(expense);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _expenseService.DeleteById(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
