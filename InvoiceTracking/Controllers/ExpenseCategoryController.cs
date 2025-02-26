using BusinessLayer.Services;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InvoiceSystem.Controllers
{
    public class ExpenseCategoryController : Controller
    {
        private readonly IExpenseCategoryService _expenseCategoryService;

        public ExpenseCategoryController(IExpenseCategoryService expenseCategoryService)
        {
            _expenseCategoryService = expenseCategoryService;
        }

        // 📌 1️⃣ Tüm gider kategorilerini listele
        public async Task<IActionResult> Index()
        {
            var categories = await _expenseCategoryService.GetAllCategoriesAsync();
            return View(categories);
        }

        // 📌 2️⃣ Yeni gider kategorisi ekleme formu
        public IActionResult Create()
        {
            return View();
        }

        // 📌 3️⃣ Yeni gider kategorisi ekleme işlemi
        [HttpPost]
        public async Task<IActionResult> Create(ExpenseCategory category)
        {
            if (ModelState.IsValid)
            {
                await _expenseCategoryService.AddCategoryAsync(category);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // 📌 4️⃣ Gider kategorisi güncelleme formu
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _expenseCategoryService.GetCategoryByIdAsync(id);
            if (category == null)
                return NotFound();

            return View(category);
        }

        // 📌 5️⃣ Gider kategorisi güncelleme işlemi
        [HttpPost]
        public async Task<IActionResult> Edit(ExpenseCategory category)
        {
            if (ModelState.IsValid)
            {
                await _expenseCategoryService.UpdateCategoryAsync(category);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // 📌 6️⃣ Gider kategorisi silme işlemi
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _expenseCategoryService.GetCategoryByIdAsync(id);
            if (category == null)
                return NotFound();

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _expenseCategoryService.DeleteCategoryAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
