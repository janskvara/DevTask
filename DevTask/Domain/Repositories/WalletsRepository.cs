using DevTask.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevTask.Domain.Repositories
{
    public class WalletsRepository : IWalletsRepository
    {
        private readonly IList<Wallet> wallets = new List<Wallet>();

        public async Task<Wallet> GetAsync(Guid id)
        {
            var wallet = wallets.Where(wallet => wallet.Id == id).SingleOrDefault();
            return await Task.FromResult(wallet);
        }

        public async Task AddAsync(Wallet wallet)
        {
            wallets.Add(wallet);
            await Task.CompletedTask;
        }
    }
}
