
using EntityLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using BusinessLayer.Common;

namespace InvoiceTracking.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        // 📌 Çalışanları Listeleme
        public async Task<IActionResult> Index()
        {
            var employees = await _employeeRepository.GetAllEmployeesAsync();
            return View(employees);
        }

        // 📌 Çalışan Detaylarını Görüntüleme
        public async Task<IActionResult> Details(int id)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // 📌 Yeni Çalışan Ekleme (Form Sayfası)
        public IActionResult Create()
        {
            return View();
        }

        // 📌 Yeni Çalışan Ekleme (Post Metodu)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                await _employeeRepository.AddEmployeeAsync(employee);
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // 📌 Çalışan Güncelleme (Form Sayfası)
        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // 📌 Çalışan Güncelleme (Post Metodu)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _employeeRepository.UpdateEmployeeAsync(employee);
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // 📌 Çalışan Silme Onayı (Form Sayfası)
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // 📌 Çalışan Silme (Post Metodu)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _employeeRepository.DeleteEmployeeAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
