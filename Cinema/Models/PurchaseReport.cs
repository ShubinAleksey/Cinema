using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinema.Models
{
    [Index(nameof(Name))]
    public class PurchaseReport : BaseModel
    {
        [Required]
        [Display(Name = "Наименование")]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [Display(Name = "Описание")]
        public string Description { get; set; } = string.Empty;
        
        [Required]
        [Display(Name = "Номер отдела")]
        public long DepartmentId { get; set; }

        [Display(Name = "Отдел")]
        public Department? Department { get; set; }
    }
}
