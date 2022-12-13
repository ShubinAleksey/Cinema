using System.ComponentModel.DataAnnotations;

namespace Cinema.Models
{
    public abstract class BaseModel
    {
        [Display(Name = "Номер")]
        public long Id { get; set; }
    }
}
