using System;
using Xunit;
using Moneybox.App;
using Moneybox.App.DataAccess;
using Moneybox.App.Domain.Services;
using Moneybox.App.Features;

namespace MoneyBoxTestXUnit
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            IAccountRepository _mockAccountRepository = new AccountRepository();
            TransferMoney testTransferMoney = new TransferMoney(_mockAccountRepository.Object, _mockNotificationService.Object);

        }
    }
}
