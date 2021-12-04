
using System.ComponentModel.DataAnnotations;

namespace DevTask.Domain.Dtos
{
    public class RegistrationDto
    {
        [Required]
        public string UserName { get; set; }
    }
}
