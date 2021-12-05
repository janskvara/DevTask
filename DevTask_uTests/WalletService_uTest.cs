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
            var wallet = new Wallet
            {
                Id = Guid.Empty,
                Balance = 10,
                Transactions = new List<Transaction>()
            };
            var walletsRepositoryMock = new Mock<IWalletsRepository>();
            walletsRepositoryMock.Setup(_ => _.GetAsync(Guid.Empty)).ReturnsAsync(wallet);
            var walletService = new WalletService(walletsRepositoryMock.Object);

            //Action
            var result = await walletService.GetBalanceAsync(Guid.Empty);

            //Assert
            Assert.Equal(10, result);
        }
    }
}
