using DevTask.Domain.Models;
using System;
using System.Threading.Tasks;

namespace DevTask.Domain.Repositories
{
    public interface IWalletsRepository
    {
        Task<Wallet> GetAsync(Guid id);
        Task AddAsync(Wallet wallet);
        Task<decimal> SetBalanceAsync(Guid id, decimal newBalance);
        Task AddTransactionAsync(Guid id, Transaction transaction);
    }
}
