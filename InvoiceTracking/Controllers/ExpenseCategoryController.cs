using BusinessLayer.Services;
using EntityLayer.Entities;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace InvoiceTracking.Controllers
{
    public class ExpenseCategoryController : Controller
    {
        private readonly IExpenseCategoryService _categoryService;

        public ExpenseCategoryController(IExpenseCategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public IActionResult Index()
        {
            var categories = _categoryService.GetAll();
            return View(categories);
        }

        public async Task<IActionResult> Details(int id)
        {
            var category = await _categoryService.GetCategoryWithExpensesAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ExpenseCategory category)
        {
            if (ModelState.IsValid)
            {
                await _categoryService.AddAsync(category);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ExpenseCategory category)
        {
            if (id != category.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _categoryService.Update(category);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _categoryService.DeleteById(id);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult ExportToExcel()
        {
            var categories = _categoryService.GetAll();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("GiderKategorileri");
                worksheet.Cells[1, 1].Value = "ID";
                worksheet.Cells[1, 2].Value = "Kategori Adı";
                worksheet.Cells[1, 3].Value = "Açıklama";
                worksheet.Cells[1, 4].Value = "Gider Sayısı";

                int row = 2;
                foreach (var category in categories)
                {
                    worksheet.Cells[row, 1].Value = category.CategoryId;
                    worksheet.Cells[row, 2].Value = category.CategoryName;
                    worksheet.Cells[row, 3].Value = category.Description ?? "-";
                    worksheet.Cells[row, 4].Value = category.Expenses?.Count ?? 0;
                    row++;
                }

                var stream = new MemoryStream(package.GetAsByteArray());
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "GiderKategorileri.xlsx");
            }

        }
        public IActionResult ExportToPdf()
        {
            var categories = _categoryService.GetAll();

            using (var stream = new MemoryStream())
            {
                Document doc = new Document();
                PdfWriter.GetInstance(doc, stream).CloseStream = false;
                doc.Open();

                var table = new PdfPTable(4);
                table.AddCell("ID");
                table.AddCell("Kategori Adı");
                table.AddCell("Açıklama");
                table.AddCell("Gider Sayısı");

                foreach (var category in categories)
                {
                    table.AddCell(category.CategoryId.ToString());
                    table.AddCell(category.CategoryName);
                    table.AddCell(string.IsNullOrEmpty(category.Description) ? "-" : category.Description);
                    table.AddCell(category.Expenses?.Count.ToString() ?? "0");
                }

                doc.Add(table);
                doc.Close();

                stream.Position = 0;
                return File(stream.ToArray(), "application/pdf", "GiderKategorileri.pdf");
            }
        }

    }
}
