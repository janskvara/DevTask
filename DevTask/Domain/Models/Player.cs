using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevTask.Domain.Models
{
    public class Player
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public Wallet Wallet { get; set; }
    }
}
