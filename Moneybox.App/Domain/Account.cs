using System;

namespace Moneybox.App
{

    public class Account
    {
        private decimal balance;
        private decimal paidIn;
        public decimal PayInLimit = 4000m;

        public Guid Id { get; set; }

        public User User { get; set; }

        public decimal Balance
        {
            get { return balance; }
            set
            {
                if (value < 0m)
                {
                    throw new InvalidOperationException("Insufficient funds to make transfer");
                }
                else {
                    balance = value;
                }
            }
        }

        public decimal Withdrawn { get;set;}

        public decimal PaidIn {
            get { return paidIn; }
            set
            {
                if (value > this.PayInLimit)
                {
                    throw new InvalidOperationException("Account pay in limit reached");
                }
                else {
                    paidIn = value;
                }
            }
        }
    }
}
