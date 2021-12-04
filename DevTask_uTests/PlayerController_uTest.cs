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
        public async void RegistrationAsync_PlayerServiceDontCreateNewPlayer_NoContent()
        {
            //Arrange
            var registationDto = new RegistrationDto
            {
                UserName = "userName"
            };
            var playerServiceMock = new Mock<IPlayerService>();
            playerServiceMock.Setup(_ => _.CreateNewPlayerAsync(registationDto.UserName)).ReturnsAsync((Player)null);

            var playerContoller = new PlayerController(playerServiceMock.Object);

            //Act
            var result = await playerContoller.RegistrationAsync(registationDto);

            //Assert
            Assert.IsType<NoContentResult>(result.Result);
        }

        [Fact]
        public async void RegistrationAsync_PlayerServiceCreateNewPlayer_CreatedAtActionResult()
        {
            //Arrange
            var registationDto = new RegistrationDto
            {
                UserName = "userName"
            };
            var player = new Player
            {
                Id = Guid.Empty,
                UserName = String.Empty,
                Wallet = new Wallet()
                {
                    Id = Guid.Empty,
                    Balance = 0,
                    Transactions = new List<Transaction>()
                },
            };

            var playerServiceMock = new Mock<IPlayerService>();
            playerServiceMock.Setup(_ => _.CreateNewPlayerAsync(registationDto.UserName)).ReturnsAsync(player);
            var playerContoller = new PlayerController(playerServiceMock.Object);

            //Act
            var result = await playerContoller.RegistrationAsync(registationDto);

            //Assert
            Assert.IsType<CreatedAtActionResult>(result.Result);
        }

        [Fact]
        public async void GetBalanceAsync_PlayerServiceDidntFindPlayer_NotFoundResult()
        {
            //Arrange
            var userName = "userName";
            var playerServiceMock = new Mock<IPlayerService>();
            playerServiceMock.Setup(_ => _.GetPlayerAsync(userName)).ReturnsAsync((Player)null);

            var playerContoller = new PlayerController(playerServiceMock.Object);

            //Act
            var result = await playerContoller.GetBalanceAsync(userName);

            //Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Theory]
        [InlineData(10, 10)]
        [InlineData(0, 0)]
        public async void GetBalanceAsync_PlayerServiceFountPlayer_ReturnWalletBalance(decimal settedValue, decimal expectedValue)
        {
            //Arrange
            var userName = "userName";
            var player = new Player
            {
                Id = Guid.Empty,
                UserName = userName,
                Wallet = new Wallet()
                {
                    Id = Guid.Empty,
                    Balance = settedValue,
                    Transactions = new List<Transaction>()
                },
            };

            var playerServiceMock = new Mock<IPlayerService>();
            playerServiceMock.Setup(_ => _.GetPlayerAsync(userName)).ReturnsAsync(player);
            var playerContoller = new PlayerController(playerServiceMock.Object);

            //Act
            var result = await playerContoller.GetBalanceAsync(userName);

            //Assert
            Assert.IsType<CreatedAtActionResult>(result.Result);
            //todo: result.Value = null ?? Zajtra analyzovat
            //Assert.Equal(expectedValue, result.Result);
        }
    }
}
