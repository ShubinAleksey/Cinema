using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Cinema.Models
{
    public class Order : BaseModel
    {
        [Required]
        [Display(Name = "Код покупателя")]
        public string BuyerId { get; set; } = string.Empty;
        
        [Required]
        [Display(Name = "Код билета")]
        public long TicketId { get; set; }

        [Display(Name = "Подтверждён")]
        public bool IsConfirmed { get; set; }

        [Display(Name = "Отклонён")]
        public bool IsRejected { get; set; }

        [Display(Name = "Покупатель")]
        public IdentityUser Buyer { get; set; } = default!;

        [Display(Name = "Билет")]
        public Ticket Ticket { get; set; } = null!;
    }
}
