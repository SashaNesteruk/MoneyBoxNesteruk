using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moneybox.App;
using Moneybox.App.DataAccess;
using Moneybox.App.Domain.Services;
using Moneybox.App.Features;
using Moq;
using System;
using System.Collections.Generic;

namespace MoneyBoxTest
{
    [TestClass]
    public class FeaturesTest
    {
        private Mock<IAccountRepository> _mockAccountRepository;
        private Mock<INotificationService> _mockNotificationService;
        private TransferMoney _testTransferMoney;
        private WithdrawMoney _testWithdrawMoney;
        [TestInitialize]
        public void Setup()
        {
            _mockAccountRepository = new Mock<IAccountRepository>();
            _mockNotificationService = new Mock<INotificationService>();
            _testTransferMoney = new TransferMoney(_mockAccountRepository.Object, _mockNotificationService.Object);
            _testWithdrawMoney = new WithdrawMoney(_mockAccountRepository.Object, _mockNotificationService.Object);
        }

        [TestMethod]
        public void TestTransfer()
        {
            //Arrange
            User user = new User()
            {
                Id = Guid.NewGuid(),
                Email = "asd@asd.com",
                Name = "John Smith"
            };
            var fromaccount = new Account()
            {
                Id = Guid.NewGuid(),
                Balance = 100,
                User = user
            };
            var toaccount = new Account()
            {
                Id = Guid.NewGuid(),
                Balance = 100,
                User = user
            };
            _mockAccountRepository.Setup(c => c.GetAccountById(fromaccount.Id)).Returns(() => fromaccount);
            _mockAccountRepository.Setup(c => c.GetAccountById(toaccount.Id)).Returns(() => toaccount);
            //Act 
            _testTransferMoney.Execute(fromaccount.Id, toaccount.Id,100);
            _mockNotificationService.Verify(c=>c.NotifyFundsLow(It.IsAny<string>()), Times.Once());
            //Assert
            Assert.AreEqual(fromaccount.Balance,0);
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestTransfer_InsufficientFunds()
        {
            //Arrange
            User user = new User()
            {
                Id = Guid.NewGuid(),
                Email = "asd@asd.com",
                Name = "John Smith"
            };
            var fromaccount = new Account()
            {
                Id = Guid.NewGuid(),
                Balance = 100,
                User = user
            };
            var toaccount = new Account()
            {
                Id = Guid.NewGuid(),
                Balance = 100,
                User = user
            };
            _mockAccountRepository.Setup(c => c.GetAccountById(fromaccount.Id)).Returns(() => fromaccount);
            _mockAccountRepository.Setup(c => c.GetAccountById(toaccount.Id)).Returns(() => toaccount);
            _testTransferMoney.Execute(fromaccount.Id, toaccount.Id, 200);

        }
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestTransfer_PayLimitExceeded()
        {
            //Arrange
            User user = new User()
            {
                Id = Guid.NewGuid(),
                Email = "asd@asd.com",
                Name = "John Smith"
            };
            var fromaccount = new Account()
            {
                Id = Guid.NewGuid(),
                Balance = 5000m,
                User = user
            };
            var toaccount = new Account()
            {
                Id = Guid.NewGuid(),
                Balance = 3000m,
                User = user
            };
            _mockAccountRepository.Setup(c => c.GetAccountById(fromaccount.Id)).Returns(() => fromaccount);
            _mockAccountRepository.Setup(c => c.GetAccountById(toaccount.Id)).Returns(() => toaccount);
            _testTransferMoney.Execute(fromaccount.Id, toaccount.Id, 4500m);
            _mockNotificationService.Verify(c => c.NotifyFundsLow(It.IsAny<string>()), Times.Once());
            _mockNotificationService.Verify(c => c.NotifyApproachingPayInLimit(It.IsAny<string>()), Times.Once());

        }

        [TestMethod]
        public void TestWithdraw()
        {
            //Arrange
            User user = new User()
            {
                Id = Guid.NewGuid(),
                Email = "asd@asd.com",
                Name = "John Smith"
            };
            var fromaccount = new Account()
            {
                Id = Guid.NewGuid(),
                Balance = 100,
                User = user
            };
            _mockAccountRepository.Setup(c => c.GetAccountById(fromaccount.Id)).Returns(() => fromaccount);
            //Act 
            _testWithdrawMoney.Execute(fromaccount.Id, 100);
            _mockNotificationService.Verify(c => c.NotifyFundsLow(It.IsAny<string>()), Times.Once());
            //Assert
            Assert.AreEqual(fromaccount.Balance, 0);
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestWithdraw_InsufficientFunds()
        {
            //Arrange
            User user = new User()
            {
                Id = Guid.NewGuid(),
                Email = "asd@asd.com",
                Name = "John Smith"
            };
            var fromaccount = new Account()
            {
                Id = Guid.NewGuid(),
                Balance = 100,
                User = user
            };
            _mockAccountRepository.Setup(c => c.GetAccountById(fromaccount.Id)).Returns(() => fromaccount);
            //Act 
            _testWithdrawMoney.Execute(fromaccount.Id, 200);
            _mockNotificationService.Verify(c => c.NotifyFundsLow(It.IsAny<string>()), Times.Once());
        }
    }
}

