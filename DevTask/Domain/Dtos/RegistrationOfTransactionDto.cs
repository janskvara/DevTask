using DevTask.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace DevTask.Domain.Dtos
{
    public class RegistrationOfTransactionDto
    {
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public ETypeOfTransaction Type { get; set; }
    }
}
