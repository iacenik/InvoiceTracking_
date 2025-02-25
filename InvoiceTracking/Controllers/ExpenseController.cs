using BusinessLayer.Common;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Threading.Tasks;

namespace InvoiceTracking.Controllers
{
    public class ExpenseController : Controller
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IExpenseCategoryRepository _categoryRepository;

        public ExpenseController(IExpenseRepository expenseRepository, IEmployeeRepository employeeRepository, IExpenseCategoryRepository categoryRepository)
        {
            _expenseRepository = expenseRepository;
            _employeeRepository = employeeRepository;
            _categoryRepository = categoryRepository;
        }

        // 📌 Tüm giderleri listeleme
        public async Task<IActionResult> Index()
        {
            var expenses = await _expenseRepository.GetAllExpensesAsync();
            return View(expenses);
        }

        // 📌 Belirli bir giderin detaylarını görüntüleme
        public async Task<IActionResult> Details(int id)
        {
            var expense = await _expenseRepository.GetExpenseByIdAsync(id);
            if (expense == null)
                return NotFound();

            return View(expense);
        }

        // 📌 Yeni gider ekleme sayfası
        public async Task<IActionResult> Create()
        {
            await PopulateDropdowns();
            return View();
        }

        // 📌 Yeni gider ekleme işlemi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Expense expense)
        {
            if (ModelState.IsValid)
            {
                await _expenseRepository.AddExpenseAsync(expense);
                return RedirectToAction(nameof(Index));
            }

            await PopulateDropdowns();
            return View(expense);
        }

        // 📌 Gider güncelleme sayfası
        public async Task<IActionResult> Edit(int id)
        {
            var expense = await _expenseRepository.GetExpenseByIdAsync(id);
            if (expense == null)
                return NotFound();

            await PopulateDropdowns();
            return View(expense);
        }

        // 📌 Gider güncelleme işlemi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Expense expense)
        {
            if (id != expense.ExpenseId)
                return NotFound();

            if (ModelState.IsValid)
            {
                await _expenseRepository.UpdateExpenseAsync(expense);
                return RedirectToAction(nameof(Index));
            }

            await PopulateDropdowns();
            return View(expense);
        }

        // 📌 Gider silme onayı
        public async Task<IActionResult> Delete(int id)
        {
            var expense = await _expenseRepository.GetExpenseByIdAsync(id);
            if (expense == null)
                return NotFound();

            return View(expense);
        }

        // 📌 Gider silme işlemi
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _expenseRepository.DeleteExpenseAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // 📌 Çalışana göre giderleri getirme
        public async Task<IActionResult> ExpensesByEmployee(int employeeId)
        {
            var expenses = await _expenseRepository.GetExpensesByEmployeeAsync(employeeId);
            return View("Index", expenses);
        }

        // 📌 Kategoriye göre giderleri getirme
        public async Task<IActionResult> ExpensesByCategory(int categoryId)
        {
            var expenses = await _expenseRepository.GetExpensesByCategoryAsync(categoryId);
            return View("Index", expenses);
        }

        // 📌 Belirli bir tarih aralığındaki giderleri getirme
        public async Task<IActionResult> ExpensesByDateRange(DateTime startDate, DateTime endDate)
        {
            var expenses = await _expenseRepository.GetExpensesByDateRangeAsync(startDate, endDate);
            return View("Index", expenses);
        }

        // 📌 Dropdown verilerini hazırlayan yardımcı metot
        private async Task PopulateDropdowns()
        {
            ViewData["EmployeeId"] = new SelectList(await _employeeRepository.GetAllEmployeesAsync(), "EmployeeId", "EmployeeName");
            ViewData["CategoryId"] = new SelectList(await _categoryRepository.GetAllExpenseCategoriesAsync(), "CategoryId", "CategoryName");
        }
    }
}
