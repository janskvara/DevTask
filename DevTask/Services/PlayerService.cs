using DevTask.Domain.Models;
using DevTask.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevTask.Services
{
    public class PlayerService: IPlayerService
    {
        private readonly IPlayersRepository playersRepository;
        private readonly IWalletService walletService;

        public PlayerService(IPlayersRepository playersRepository, IWalletService walletService)
        {
            this.playersRepository = playersRepository;
            this.walletService = walletService;
        }

        public async Task<EStateOfTransaction> AddTransactionAsync(string userName, Transaction newTransaction)
        {
            var player = await playersRepository.GetAsync(userName);
            if (player == null)
            {
                return EStateOfTransaction.UserDoesntExist;
            }
            return await walletService.RegisterTransaction(player.Wallet, newTransaction);
        }

        public async Task<Player> CreateNewPlayerAsync(string userName)
        {
            var player =  await playersRepository.GetAsync(userName);
            if(player != null)
            {
                return null;
            }

            player = new Player {
                Id = Guid.NewGuid(),
                UserName = userName,
                Wallet = await walletService.CreateWalletAsync(),
            };

            await playersRepository.AddAsync(player);
            return player;
        }

        public async Task<decimal> GetPlayerBalanceAsync(string userName)
        {
            var player = await playersRepository.GetAsync(userName);
            if (player == null)
            {
                return decimal.MinusOne;
            }
            var balance = await walletService.GetBalanceAsync(player.Wallet);
            return balance;
        }

        public async Task<IList<Transaction>> GetPlayerTransactionseAsync(string userName)
        {
            var player = await playersRepository.GetAsync(userName);
            if (player == null)
            {
                return null;
            }
            var transactions = await walletService.GetTransactionsAsync(player.Wallet);
            return transactions.ToList();
        }
    }
}
