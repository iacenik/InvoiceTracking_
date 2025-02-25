
using BusinessLayer.Common;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceTracking.Controllers
{
    public class ExpenseCategoryController : Controller
    {
        private readonly IExpenseCategoryRepository _categoryRepository;

        public ExpenseCategoryController(IExpenseCategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        // 📌 Tüm Gider Kategorilerini Listeleme
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryRepository.GetAllExpenseCategoriesAsync();
            return View(categories);
        }

        // 📌 Gider Kategori Detayları
        public async Task<IActionResult> Details(int id)
        {
            var category = await _categoryRepository.GetExpenseCategoryByIdAsync(id);
            if (category == null)
                return NotFound();

            return View(category);
        }

        // 📌 Yeni Gider Kategorisi Ekleme Sayfası (Form Göster)
        public IActionResult Create()
        {
            return View();
        }

        // 📌 Yeni Gider Kategorisi Ekleme (Form POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ExpenseCategory category)
        {
            if (ModelState.IsValid)
            {
                await _categoryRepository.AddExpenseCategoryAsync(category);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // 📌 Gider Kategorisi Güncelleme Sayfası (Form Göster)
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryRepository.GetExpenseCategoryByIdAsync(id);
            if (category == null)
                return NotFound();

            return View(category);
        }

        // 📌 Gider Kategorisi Güncelleme (Form POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ExpenseCategory category)
        {
            if (id != category.CategoryId)
                return NotFound();

            if (ModelState.IsValid)
            {
                await _categoryRepository.UpdateExpenseCategoryAsync(category);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // 📌 Gider Kategorisi Silme Sayfası (Onay İçin)
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryRepository.GetExpenseCategoryByIdAsync(id);
            if (category == null)
                return NotFound();

            return View(category);
        }

        // 📌 Gider Kategorisi Silme İşlemi
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _categoryRepository.DeleteExpenseCategoryAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
