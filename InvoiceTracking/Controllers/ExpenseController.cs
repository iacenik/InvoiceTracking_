using BusinessLayer.Services;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.Common;

namespace InvoiceSystem.Controllers
{
    public class ExpenseController : Controller
    {
        private readonly IExpenseService _expenseService;
        private readonly IEmployeeService _employeeService;
        private readonly IExpenseCategoryService _categoryService;
        private readonly ICashRegisterService _cashRegisterService;
        private readonly ICashRegisterRepository _cashRegisterRepository; // 💡 Eksik olan değişkeni ekledik


        public ExpenseController(
            IExpenseService expenseService,
            IEmployeeService employeeService,
            IExpenseCategoryService categoryService,
            ICashRegisterRepository cashRegisterRepository) // 💡 Yeni bağımlılığı ekledik
        {
            _expenseService = expenseService;
            _employeeService = employeeService;
            _categoryService = categoryService;
            _cashRegisterRepository = cashRegisterRepository; // 💡 Burada nesneyi atıyoruz
        }

        // 📌 1️⃣ Tüm giderleri listele
        public async Task<IActionResult> Index()
        {
            var expenses = await _expenseService.GetAllExpensesAsync();
            return View(expenses);
        }

        // 📌 2️⃣ Yeni gider ekleme formu
        public async Task<IActionResult> Create()
        {
            await PopulateDropDowns();
            return View();
        }

        // 📌 3️⃣ Yeni gider ekleme işlemi
        [HttpPost]
        public async Task<IActionResult> Create(Expense expense)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropDowns();
                return View(expense);
            }

            var cashRegister = await _cashRegisterRepository.GetFirstAsync(); // ✅ Artık hata vermez
            if (cashRegister == null)
            {
                ModelState.AddModelError("", "Kasa bulunamadı!");
                await PopulateDropDowns();
                return View(expense);
            }

            // 📌 Varsayılan tarih atanıyor (Kullanıcı girmezse)
            if (expense.Date == default)
                expense.Date = DateTime.Now;

            await _expenseService.AddExpenseAsync(expense, cashRegister);
            return RedirectToAction(nameof(Index));
        }
        // 📌 4️⃣ Gider güncelleme formu
        public async Task<IActionResult> Edit(int id)
        {
            var expense = await _expenseService.GetExpenseByIdAsync(id);
            if (expense == null)
                return NotFound();

            await PopulateDropDowns(expense);
            return View(expense);
        }

        // 📌 5️⃣ Gider güncelleme işlemi
        [HttpPost]
        public async Task<IActionResult> Edit(Expense expense)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropDowns(expense);
                return View(expense);
            }

            await _expenseService.UpdateExpenseAsync(expense);
            return RedirectToAction(nameof(Index));
        }

        // 📌 6️⃣ Gider silme işlemi
        public async Task<IActionResult> Delete(int id)
        {
            var expense = await _expenseService.GetExpenseByIdAsync(id);
            if (expense == null)
                return NotFound();

            return View(expense);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _expenseService.DeleteExpenseAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // 📌 7️⃣ Belirli bir çalışana ait giderleri listeleme
        public async Task<IActionResult> EmployeeExpenses(int employeeId)
        {
            var expenses = await _expenseService.GetExpensesByEmployeeIdAsync(employeeId);
            return View("Index", expenses);
        }

        // 📌 8️⃣ Form dropdownlarını doldurma metodu
        private async Task PopulateDropDowns(Expense? expense = null)
        {
            var employees = await _employeeService.GetAllEmployeesAsync();
            var categories = await _categoryService.GetAllCategoriesAsync();

            ViewBag.Employees = new SelectList(employees, "EmployeeId", "EmployeeName", expense?.EmployeeId);
            ViewBag.Categories = new SelectList(categories, "CategoryId", "CategoryName", expense?.CategoryId);
        }
    }
}
    