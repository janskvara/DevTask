using DevTask.Domain.Models;
using DevTask.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevTask.Services
{
    public class WalletService: IWalletService
    {
        private readonly IWalletsRepository walletsRepository;

        public WalletService(IWalletsRepository walletsRepository)
        {
            this.walletsRepository = walletsRepository;
        }

        public async Task<decimal> GetBalanceAsync(Guid id)
        {
            var wallet = await walletsRepository.GetAsync(id);
            if (wallet == null) { 
                return decimal.MinusOne;
            }

            return wallet.Balance;
        }

        public async Task<Guid> CreateWalletAsync()
        {
            var newWallet = new Wallet()
            {
                Id = Guid.NewGuid(),
                Balance = 0,
                Transactions = new List<Transaction>()
            };
            await walletsRepository.AddAsync(newWallet);

            return newWallet.Id;
        }
    }
}
