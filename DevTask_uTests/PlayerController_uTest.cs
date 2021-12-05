using DevTask.Domain.Repositories;
using DevTask.Domain.Models;
using DevTask.Services;
using DevTask.Controllers;
using Moq;
using Xunit;
using DevTask.Domain.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace DevTask_uTests
{
    public class PlayerController_uTest
    {
        [Fact]
        public async void Registration_PlayerServiceDontCreateNewPlayer_NoContent()
        {
            //Arrange
            var registationDto = new RegistrationOfPlayerDto
            {
                UserName = "userName"
            };
            var playerServiceMock = new Mock<IPlayerService>();
            playerServiceMock.Setup(_ => _.CreateNewPlayerAsync(registationDto.UserName)).ReturnsAsync((Player)null);

            var playerContoller = new PlayerController(playerServiceMock.Object);

            //Act
            var result = await playerContoller.Registration(registationDto);

            //Assert
            Assert.IsType<NoContentResult>(result.Result);
        }

        [Fact]
        public async void Registration_PlayerServiceCreateNewPlayer_CreatedAtActionResult()
        {
            //Arrange
            var registationDto = new RegistrationOfPlayerDto
            {
                UserName = "userName"
            };
            var player = new Player
            {
                Id = Guid.Empty,
                UserName = String.Empty,
                Wallet = Guid.Empty
            };

            var playerServiceMock = new Mock<IPlayerService>();
            playerServiceMock.Setup(_ => _.CreateNewPlayerAsync(registationDto.UserName)).ReturnsAsync(player);
            var playerContoller = new PlayerController(playerServiceMock.Object);

            //Act
            var result = await playerContoller.Registration(registationDto);

            //Assert
            Assert.IsType<CreatedAtActionResult>(result.Result);
        }

        [Fact]
        public async void GetBalance_PlayerServiceDidntFindBalance_NotFoundResult()
        {
            //Arrange
            var userName = "userName";
            var playerServiceMock = new Mock<IPlayerService>();
            playerServiceMock.Setup(_ => _.GetPlayerBalanceAsync(userName)).ReturnsAsync(decimal.MinusOne);

            var playerContoller = new PlayerController(playerServiceMock.Object);

            //Act
            var result = await playerContoller.GetBalance(userName);

            //Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Theory]
        [InlineData(10, 10)]
        [InlineData(0, 0)]
        public async void GetBalance_PlayerServiceFountBalance_ReturnWalletBalance(decimal settedValue, decimal expectedValue)
        {
            //Arrange
            var userName = "userName";
            var player = new Player
            {
                Id = Guid.Empty,
                UserName = userName,
                Wallet = Guid.Empty
            };

            var playerServiceMock = new Mock<IPlayerService>();
            playerServiceMock.Setup(_ => _.GetPlayerBalanceAsync(userName)).ReturnsAsync(settedValue);
            var playerContoller = new PlayerController(playerServiceMock.Object);

            //Act
            var result = await playerContoller.GetBalance(userName);

            //Assert
            Assert.IsType<CreatedAtActionResult>(result.Result);
            //todo: result.Value = null ?? Zajtra analyzovat
            //Assert.Equal(expectedValue, result.Result);
        }

        [Theory]
        [InlineData(EStateOfTransaction.UserDoesntExist)]
        [InlineData(EStateOfTransaction.WalletDoesntFound)]
        public async void Transaction_UserOrWalletWereNotFound_NotFound(EStateOfTransaction stateOfTransaction)
        {
            //Arrange
            var playerServiceMock = new Mock<IPlayerService>();
            playerServiceMock.Setup(_ => _.AddTransactionAsync(string.Empty, It.IsAny<Transaction>())).ReturnsAsync(stateOfTransaction);
            var playerContoller = new PlayerController(playerServiceMock.Object);

            //Act
            var result = await playerContoller.Transaction(string.Empty, new RegistrationOfTransactionDto());

            //Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Theory]
        [InlineData(EStateOfTransaction.Accepted, "Accepted")]
        [InlineData(EStateOfTransaction.Rejected, "Rejected")]
        public async void Transaction_TransactionWasAdded_CreatedAtActionResult(EStateOfTransaction stateOfTransaction, string expectedResult)
        {
            //Arrange
            var playerServiceMock = new Mock<IPlayerService>();
            playerServiceMock.Setup(_ => _.AddTransactionAsync(string.Empty, It.IsAny<Transaction>())).ReturnsAsync(stateOfTransaction);
            var playerContoller = new PlayerController(playerServiceMock.Object);

            //Act
            var result = await playerContoller.Transaction(string.Empty, new RegistrationOfTransactionDto());

            //Assert
            Assert.IsType<CreatedAtActionResult>(result.Result);
            //Assert.Equal(expectedResult, result.Result.ToString());
        }


    }
}
