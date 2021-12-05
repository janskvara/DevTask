using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevTask.Domain.Dtos
{
    public class TransactionDto
    {
        public Guid Id { get; set; }
        public decimal Amount{ get; set; }
    }
}
