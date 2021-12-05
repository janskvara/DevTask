using DevTask.Domain.Repositories;
using DevTask.Domain.Models;
using DevTask.Services;
using Moq;
using Xunit;

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
    }
}
