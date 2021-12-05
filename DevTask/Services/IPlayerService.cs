using DevTask.Domain.Dtos;
using DevTask.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevTask.Services
{
    public interface IPlayerService
    {
        Task<Player> CreateNewPlayerAsync(string userName);

        Task<decimal> GetPlayerBalanceAsync(string userName);

        Task<EStateOfTransaction> AddTransactionAsync(string userName, Transaction newTransaction);

        Task<IList<Transaction>> GetPlayerTransactionseAsync(string userName);
    }
}
