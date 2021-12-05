using System;

namespace DevTask.Domain.Models
{
    public class Player
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public Guid Wallet { get; set; }
    }
}
