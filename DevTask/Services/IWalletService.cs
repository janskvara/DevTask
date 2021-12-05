using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevTask.Services
{
    public interface IWalletService
    {
        Task<Guid> CreateWalletAsync();

        Task<decimal> GetBalanceAsync(Guid id);
    }
}
