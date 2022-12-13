using System.ComponentModel.DataAnnotations;

namespace Cinema.Models
{
    public class Ticket : BaseModel
    {
        [Required]
        [Display(Name = "Номер сеанса")]
        public long SessionId { get; set; }

        [Required]
        [Display(Name = "Номер ряда")]
        public int RowNumber { get; set; }

        [Required]
        [Display(Name = "Номер места")]
        public int SeatNumber { get; set; }
        
        [Required]
        [Display(Name = "Куплен")]
        public bool IsBought { get; set; }

        [Display(Name = "Сеанс")]
        public MovieSession? Session { get; set; }

        public ICollection<Order> Orders { get; } = new List<Order>();
    }
}
