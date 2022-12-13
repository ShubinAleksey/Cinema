using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Cinema.Models
{
    [Index(nameof(MovieName))]
    public class MovieSession : BaseModel
    {
        [Required]
        [Display(Name = "Название фильма")]
        public string MovieName { get; set; } = string.Empty;
        
        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Дата сеанса")]
        public DateTime SessionTime { get; set; }

        public ICollection<Ticket> Tickets { get; } = new List<Ticket>();
    }
}
