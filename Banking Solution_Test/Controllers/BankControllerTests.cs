using Banking_Solution_Test.Models;
using Banking_Solution_Test.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using System.Collections.Generic;

namespace Banking_Solution_Test.Controllers
{
    public class BankControllerTests
    {
        private readonly Mock<IAccountService> _mockService;
        private readonly BankController _controller;

        public BankControllerTests()
        {
            _mockService = new Mock<IAccountService>();
            _controller = new BankController();
        }

        [Fact]
        public void GetAll_ShouldReturnAllAccounts()
        {
            // Arrange
            var accounts = new List<Account>
        {
            new Account { Id = 1, Name = "Alice", Balance = 100 },
            new Account { Id = 2, Name = "Bob", Balance = 200 }
        };
            _mockService.Setup(s => s.GetAll()).Returns(accounts);

            // Act
            var result = _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<ActionResult<List<Account>>>(result);
            Assert.Equal(accounts.Count, okResult.Value.Count);
        }

        [Fact]
        public void Get_ShouldReturnAccount_WhenFound()
        {
            // Arrange
            var account = new Account { Id = 1, Name = "Alice", Balance = 100 };
            _mockService.Setup(s => s.Get(It.IsAny<int>())).Returns(account);

            // Act
            var result = _controller.Get(1);

            // Assert
            var okResult = Assert.IsType<ActionResult<Account>>(result);
            Assert.Equal(account.Id, okResult.Value.Id);
        }

        [Fact]
        public void Get_ShouldReturnNotFound_WhenAccountNotExists()
        {
            // Arrange
            _mockService.Setup(s => s.Get(It.IsAny<int>())).Returns((Account)null);

            // Act
            var result = _controller.Get(1);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void Deposit_ShouldUpdateBalance_WhenValid()
        {
            // Arrange
            var account = new Account { Id = 1, Balance = 100 };
            _mockService.Setup(s => s.Get(It.IsAny<int>())).Returns(account);
            _mockService.Setup(s => s.UpdateBalance(It.IsAny<int>(), It.IsAny<decimal>())).Returns(true);

            // Act
            var result = _controller.Deposit(1, 50);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockService.Verify(s => s.UpdateBalance(1, 50), Times.Once);
        }

        [Fact]
        public void Deposit_ShouldReturnNotFound_WhenAccountNotExists()
        {
            // Arrange
            _mockService.Setup(s => s.Get(It.IsAny<int>())).Returns((Account)null);

            // Act
            var result = _controller.Deposit(1, 50);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Transfer_ShouldReturnNoContent_WhenSuccessful()
        {
            // Arrange
            var sender = new Account { Id = 1, Balance = 100 };
            var recipient = new Account { Id = 2, Balance = 50 };
            _mockService.Setup(s => s.Get(1)).Returns(sender);
            _mockService.Setup(s => s.Get(2)).Returns(recipient);
            _mockService.Setup(s => s.UpdateBalance(1, -50)).Returns(true);
            _mockService.Setup(s => s.UpdateBalance(2, 50)).Returns(true);

            // Act
            var result = _controller.Transfer(1, 2, 50);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void Transfer_ShouldReturnNotFound_WhenSenderOrRecipientNotExists()
        {
            // Arrange
            _mockService.Setup(s => s.Get(1)).Returns((Account)null);

            // Act
            var result = _controller.Transfer(1, 2, 50);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }

}
