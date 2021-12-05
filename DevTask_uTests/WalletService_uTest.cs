using DevTask.Domain.Repositories;
using DevTask.Domain.Models;
using DevTask.Services;
using Moq;
using Xunit;
using System;
using System.Collections.Generic;

namespace DevTask_uTests
{
    public class WalletService_uTest
    {
        [Fact]
        public async void GetBalanceAsync_IdDoesntExist_ReturnMinusOne()
        {
            //Arrange
            var walletsRepositoryMock = new Mock<IWalletsRepository>();
            walletsRepositoryMock.Setup(_ => _.GetAsync(Guid.Empty)).ReturnsAsync((Wallet)null);
            var walletService = new WalletService(walletsRepositoryMock.Object);

            //Action
            var result = await walletService.GetBalanceAsync(Guid.Empty);

            //Assert
            Assert.Equal(-1, result);
        }

        [Fact]
        public async void GetBalanceAsync_IdExists_ReturnBalanceOfWallet()
        {
            //Arrange
            var wallet = GetWallet(10);
            var walletsRepositoryMock = new Mock<IWalletsRepository>();
            walletsRepositoryMock.Setup(_ => _.GetAsync(wallet.Id)).ReturnsAsync(wallet);
            var walletService = new WalletService(walletsRepositoryMock.Object);

            //Action
            var result = await walletService.GetBalanceAsync(wallet.Id);

            //Assert
            Assert.Equal(10, result);
        }

        [Fact]
        public async void CreateWalletAsync_BalanceIsZeroInNewWallet()
        {
            //Arrange
            var walletsRepositoryMock = new Mock<IWalletsRepository>();
            var walletService = new WalletService(walletsRepositoryMock.Object);

            //Action
            var result = await walletService.CreateWalletAsync();

            //Assert
            walletsRepositoryMock.Verify(_ => _.AddAsync(It.Is<Wallet>(_ => _.Balance == 0)));
        }

        [Fact]
        public async void RegisterTransaction_IdDoesntExist_ReturnWalletDoesntFound()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            var transaction = new Transaction();
            var walletsRepositoryMock = new Mock<IWalletsRepository>();
            walletsRepositoryMock.Setup(_ => _.GetAsync(id)).ReturnsAsync((Wallet)null);
            var walletService = new WalletService(walletsRepositoryMock.Object);

            //Action
            var result = await walletService.RegisterTransaction(id, transaction);

            //Assert
            Assert.Equal(EStateOfTransaction.WalletDoesntFound, result);
        }

        [Theory]
        [InlineData(ETypeOfTransaction.Deposite)]
        [InlineData(ETypeOfTransaction.Win)]
        public async void RegisterTransaction_TypeOfTransactionIsDepositeOrWin_ReturnStateAccepted(ETypeOfTransaction typeOfTransaction)
        {
            //Arrange
            var transaction = GetTransaction(typeOfTransaction, 0);
            var wallet = GetWallet(10);
            var walletsRepositoryMock = new Mock<IWalletsRepository>();
            walletsRepositoryMock.Setup(_ => _.GetAsync(wallet.Id)).ReturnsAsync(wallet);
            walletsRepositoryMock.Setup(_ => _.GetTransaction(wallet.Id, transaction.IdempotencyKey)).ReturnsAsync((Transaction)null);
            var walletService = new WalletService(walletsRepositoryMock.Object);

            //Action
            var result = await walletService.RegisterTransaction(wallet.Id, transaction);

            //Assert
            Assert.Equal(EStateOfTransaction.Accepted, result);
        }

        [Theory]
        [InlineData(12, ETypeOfTransaction.Stake, EStateOfTransaction.Rejected)]
        [InlineData(0, ETypeOfTransaction.Stake, EStateOfTransaction.Accepted)]
        public async void RegisterTransaction_TypeOfTransactionIsStake_ReturnStateOfTransaction(decimal amount, 
                                                                ETypeOfTransaction typeOfTransaction, EStateOfTransaction expectedStateOfTransaction)
        {
            //Arrange
            var transaction = GetTransaction(typeOfTransaction, amount);
            var wallet = GetWallet(10);
            var walletsRepositoryMock = new Mock<IWalletsRepository>();
            walletsRepositoryMock.Setup(_ => _.GetAsync(wallet.Id)).ReturnsAsync(wallet);
            walletsRepositoryMock.Setup(_ => _.GetTransaction(wallet.Id, transaction.IdempotencyKey)).ReturnsAsync((Transaction)null);
            walletsRepositoryMock.Setup(_ => _.GetTransaction(wallet.Id, transaction.IdempotencyKey)).ReturnsAsync((Transaction)null);
            var walletService = new WalletService(walletsRepositoryMock.Object);

            //Action
            var result = await walletService.RegisterTransaction(wallet.Id, transaction);

            //Assert
            Assert.Equal(expectedStateOfTransaction, result);
        }

        [Theory]
        [InlineData(10, ETypeOfTransaction.Deposite, 20)]
        [InlineData(10, ETypeOfTransaction.Win, 20)]
        [InlineData(2, ETypeOfTransaction.Stake, 8)]
        public async void RegisterTransaction_TypesOfTransactionAndDifferentAmountsOfTransaction_SetBalanceAsyncCallsOnce(decimal amount,
                                                                ETypeOfTransaction typeOfTransaction, decimal expectedBalance)
        {
            //Arrange
            var transaction = GetTransaction(typeOfTransaction, amount);
            var wallet = GetWallet(10);
            var walletsRepositoryMock = new Mock<IWalletsRepository>();
            walletsRepositoryMock.Setup(_ => _.GetAsync(wallet.Id)).ReturnsAsync(wallet);
            walletsRepositoryMock.Setup(_ => _.GetTransaction(wallet.Id, transaction.IdempotencyKey)).ReturnsAsync((Transaction)null);
            var walletService = new WalletService(walletsRepositoryMock.Object);

            //Action
            var result = await walletService.RegisterTransaction(wallet.Id, transaction);

            //Assert
            walletsRepositoryMock.Verify(_ => _.SetBalanceAsync(wallet.Id, expectedBalance), Times.Once);
        }

        [Theory]
        [InlineData(12, ETypeOfTransaction.Stake, 10)]
        public async void RegisterTransaction_TypesOfTransactionAndDifferentAmountsOfTransaction_SetBalanceAsyncNeverCalls(decimal amount,
                                                                ETypeOfTransaction typeOfTransaction, decimal expectedBalance)
        {
            //Arrange
            var transaction = GetTransaction(typeOfTransaction, amount);
            var wallet = GetWallet(10);
            var walletsRepositoryMock = new Mock<IWalletsRepository>();
            walletsRepositoryMock.Setup(_ => _.GetAsync(wallet.Id)).ReturnsAsync(wallet);
            walletsRepositoryMock.Setup(_ => _.GetTransaction(wallet.Id, transaction.IdempotencyKey)).ReturnsAsync((Transaction)null);
            var walletService = new WalletService(walletsRepositoryMock.Object);

            //Action
            var result = await walletService.RegisterTransaction(wallet.Id, transaction);

            //Assert
            walletsRepositoryMock.Verify(_ => _.SetBalanceAsync(wallet.Id, expectedBalance), Times.Never);
        }

        [Theory]
        [InlineData(EStateOfTransaction.Rejected, EStateOfTransaction.Rejected)]
        [InlineData(EStateOfTransaction.Accepted, EStateOfTransaction.Accepted)]
        public async void RegisterTransaction_GetTransactionIsNotNull_StateFromTransactionWhichWasReturnedFromGetTransaction(EStateOfTransaction stateOfTransaction,
                                                                                                            EStateOfTransaction expectedStateOfTransaction)
        {
            //Arrange
            var transaction = GetTransaction(ETypeOfTransaction.Deposite, 10);
            var transactionWithIdempotencyKey = GetTransaction(ETypeOfTransaction.Deposite, 10, stateOfTransaction);
            var wallet = GetWallet(10);
            var walletsRepositoryMock = new Mock<IWalletsRepository>();
            walletsRepositoryMock.Setup(_ => _.GetAsync(wallet.Id)).ReturnsAsync(wallet);
            walletsRepositoryMock.Setup(_ => _.GetTransaction(wallet.Id, transaction.IdempotencyKey)).ReturnsAsync(transactionWithIdempotencyKey);
            var walletService = new WalletService(walletsRepositoryMock.Object);

            //Action
            var result = await walletService.RegisterTransaction(wallet.Id, transaction);

            //Assert
            Assert.Equal(expectedStateOfTransaction, result);
        }

        private Wallet GetWallet(decimal balance)
        {
            return new Wallet()
            {
                Id = Guid.NewGuid(),
                Balance = balance,
                Transactions = new List<Transaction>()
            };
        }
        private Transaction GetTransaction(ETypeOfTransaction typeOfTransaction, decimal amount)
        {
            return GetTransaction(typeOfTransaction, amount, EStateOfTransaction.NotDefine);
        }
        private Transaction GetTransaction(ETypeOfTransaction typeOfTransaction, decimal amount, EStateOfTransaction stateOfTransaction)
        {
            return new Transaction()
            {
                Id = Guid.NewGuid(),
                Type = typeOfTransaction,
                Amount = amount,
                State = stateOfTransaction,
                IdempotencyKey = "IdempotencyKey"
            };
        }
    }
}
