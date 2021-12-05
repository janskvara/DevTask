using System.ComponentModel.DataAnnotations;

namespace DevTask.Domain.Dtos
{
    public class RegistrationOfPlayerDto
    {
        [Required]
        public string UserName { get; set; }
    }
}
