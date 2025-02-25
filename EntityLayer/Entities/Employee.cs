using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Entities
{
   public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeeId { get; set; }

        [Required]
        public string? EmployeeName { get; set; }

        [Required]
        public string? EmployeeSurname { get; set; }
        public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>(); // Çalışanın yaptığı giderler

    }
}
