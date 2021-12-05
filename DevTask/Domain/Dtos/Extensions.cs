using DevTask.Domain.Models;
using System;

namespace DevTask.Domain.Dtos
{
    public static class Extensions
    {
        public static PlayerDto AsDto(this Player player)
        {
            return new PlayerDto()
            {
                Id = player.Id,
                UserName = player.UserName,
                WalletId = player.Wallet,
            };
        }

        public static TransactionDto AsDto(this Transaction transaction)
        {
            return new TransactionDto()
            {
                Id = transaction.Id,
                Amount = transaction.Amount,
            };
        }

        public static Transaction ToModel(this RegistrationOfTransactionDto registrationOfTransaction)
        {
            return new Transaction()
            {
                Id = Guid.NewGuid(),
                Amount = Math.Abs(registrationOfTransaction.Amount),
                Type = registrationOfTransaction.Type,
                State = EStateOfTransaction.NotDefine,
                IdempotencyKey = registrationOfTransaction.IdempotencyKey
            };
        }
    }
}
