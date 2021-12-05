using System;

namespace DevTask.Domain.Dtos
{
    public class PlayerDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public Guid WalletId { get; set; }
    }
}
