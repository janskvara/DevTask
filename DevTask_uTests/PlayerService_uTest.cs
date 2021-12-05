using DevTask.Domain.Repositories;
using DevTask.Domain.Models;
using DevTask.Services;
using Moq;
using Xunit;
using System;

namespace DevTask_uTests
{
    public class PlayerService_uTest
    {
        [Fact]
        public async void CreateNewPlayerAsync_UserNameAlreadyExists_Null()
        {
            //Arrange
            var playerRepositoryMock = new Mock<IPlayersRepository>();
            playerRepositoryMock.Setup(_ => _.GetAsync(string.Empty)).ReturnsAsync(new Player());
            var walletService = new Mock<IWalletService>();
            var playerService = new PlayerService(playerRepositoryMock.Object, walletService.Object);

            //Action
            var result = await playerService.CreateNewPlayerAsync(string.Empty);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async void CreateNewPlayerAsync_UserNameDoesntExist_PlayerWithUserName()
        {
            //Arrange
            string expectedUserName = "userName";
            var playerRepositoryMock = new Mock<IPlayersRepository>();
            playerRepositoryMock.Setup(_ => _.GetAsync(expectedUserName)).ReturnsAsync((Player)null);
            var walletService = new Mock<IWalletService>();
            var playerService = new PlayerService(playerRepositoryMock.Object, walletService.Object);

            //Action
            var result = await playerService.CreateNewPlayerAsync(expectedUserName);

            //Assert
            Assert.Equal(expectedUserName, result.UserName);
        }

        [Fact]
        public async void CreateNewPlayerAsync_UserNameAlreadyExists_CreatePlayerAsyncIsNeverCalled()
        {
            //Arrange
            string userName = "userName";
            var playerRepositoryMock = new Mock<IPlayersRepository>();
            playerRepositoryMock.Setup(_ => _.GetAsync(userName)).ReturnsAsync(new Player());
            var walletService = new Mock<IWalletService>();
            var playerService = new PlayerService(playerRepositoryMock.Object, walletService.Object);

            //Action
            var result = await playerService .CreateNewPlayerAsync(userName);

            //Assert
            playerRepositoryMock.Verify(_ => _.AddAsync(It.IsAny<Player>()), Times.Never);
        }

        [Fact]
        public async void CreateNewPlayerAsync_UserNameDoesntExist_CreatePlayerAsyncIsCalledOnce()
        {
            //Arrange
            var playerRepositoryMock = new Mock<IPlayersRepository>();
            playerRepositoryMock.Setup(_ => _.GetAsync(string.Empty)).ReturnsAsync((Player)null);
            var walletService = new Mock<IWalletService>();
            var playerService = new PlayerService(playerRepositoryMock.Object, walletService.Object);

            //Action
            var result = await playerService.CreateNewPlayerAsync(string.Empty);

            //Assert
            playerRepositoryMock.Verify(_ => _.AddAsync(It.IsAny<Player>()), Times.Once);
        }

        [Fact]
        public async void AddTransactionAsync_UserNameDoesntExist_ReturnUserDoesntExist()
        {
            //Arrange
            var playerRepositoryMock = new Mock<IPlayersRepository>();
            playerRepositoryMock.Setup(_ => _.GetAsync(string.Empty)).ReturnsAsync((Player)null);
            var walletService = new Mock<IWalletService>();
            var playerService = new PlayerService(playerRepositoryMock.Object, walletService.Object);

            //Action
            var result = await playerService.AddTransactionAsync(string.Empty, new Transaction());

            //Assert
            Assert.Equal(EStateOfTransaction.UserDoesntExist, result);
        }

        [Fact]
        public async void AddTransactionAsync_UserNameExists_RegisterTransactionCallsOnceWithWalletIdAndInputTransaction()
        {
            //Arrange
            string userName = "userName";
            Guid walletId = Guid.NewGuid();
            var player = new Player
            {
                Id = Guid.Empty,
                UserName = userName,
                Wallet = walletId
            };
            var transaction = new Transaction();

            var playerRepositoryMock = new Mock<IPlayersRepository>();
            playerRepositoryMock.Setup(_ => _.GetAsync(userName)).ReturnsAsync(player);
            var walletService = new Mock<IWalletService>();
            var playerService = new PlayerService(playerRepositoryMock.Object, walletService.Object);

            //Action
            var result = await playerService.AddTransactionAsync(userName, transaction);

            //Assert
            walletService.Verify(_ => _.RegisterTransaction(It.Is<Guid>(_ => _ == walletId), transaction), Times.Once);
        }
    }
}
