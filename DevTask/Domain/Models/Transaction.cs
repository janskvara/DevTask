using System;

namespace DevTask.Domain.Models
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public ETypeOfTransaction Type { get; set; }
        public EStateOfTransaction State { get; set; }
    }
}
