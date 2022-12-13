using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Cinema.Models
{
    [Index(nameof(Name))]
    public class AccountingReport : BaseModel
    {
        [Required]
        [Display(Name = "Наименование")]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [Display(Name = "Номер сотрудника")]
        public string EmployeeId { get; set; }
        
        [Required]
        [Display(Name = "Оклад")]
        public int Salary { get; set; }
        
        [Display(Name = "Награда")]
        public int? Bonus { get; set; } = 0;
        
        [Display(Name = "Неявки")]
        public int? Absence { get; set; }
        
        [Required]
        [Display(Name = "Итог")]
        public int Total { get; set; }

        [Display(Name = "Сотрудник")]
        public IdentityUser? Employee { get; set; }
    }
}
