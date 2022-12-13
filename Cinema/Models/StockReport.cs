using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Cinema.Models
{
    [Index(nameof(Name))]
    public class StockReport : BaseModel
    {
        [Required]
        [Display(Name = "Атрибутика")]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [Display(Name = "Тип")]
        public string Type { get; set; } = string.Empty;
        
        [Required]
        [Display(Name = "Количество")]
        public int Amount { get; set; }
    }
}
