using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Cinema.Models
{
    [Index(nameof(Name))]
    public class Department : BaseModel
    {
        [Required]
        [Display(Name = "Название")]
        public string Name { get; set; } = string.Empty;

        public ICollection<PurchaseReport> PurchaseReports { get; } = new List<PurchaseReport>();
    }
}
