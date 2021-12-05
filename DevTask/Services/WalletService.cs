using DevTask.Domain.Models;
using DevTask.Domain.Repositories;
using System;
using System.Collections.Generic;
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

        public async Task<EStateOfTransaction> RegisterTransaction(Guid idOfWallet, Transaction transaction)
        {
            var wallet = await walletsRepository.GetAsync(idOfWallet);
            if (wallet == null)
            {
                return EStateOfTransaction.WalletDoesntFound;
            }

            decimal newBalance = wallet.Balance;
            switch (transaction.Type){
                case ETypeOfTransaction.Deposite:
                case ETypeOfTransaction.Win:
                    newBalance += transaction.Amount;
                    transaction.State = EStateOfTransaction.Accepted;
                    await walletsRepository.SetBalanceAsync(idOfWallet, newBalance);
                    break;
                case ETypeOfTransaction.Stake:

                    if((wallet.Balance - transaction.Amount) < 0)
                    {
                        transaction.State = EStateOfTransaction.Rejected;
                    }
                    else
                    {
                        newBalance -= transaction.Amount;
                        transaction.State = EStateOfTransaction.Accepted;
                        await walletsRepository.SetBalanceAsync(idOfWallet, newBalance);
                    }
                    break;
            }

            await walletsRepository.AddTransactionAsync(idOfWallet, transaction);
            return transaction.State;
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsAsync(Guid id)
        {
            var wallet = await walletsRepository.GetAsync(id);
            if (wallet == null)
            {
                return null;
            }
            return wallet.Transactions;
        }
    }
}
