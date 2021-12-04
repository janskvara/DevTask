using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevTask.Domain.Models
{
    public class Wallet
    {
        public Guid Id { get; set; }
        public decimal Balance { get; set; }
        public IList<Transaction> Transactions { get; set; }
    }
}
