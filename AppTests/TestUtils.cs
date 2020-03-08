using App;
using System;

namespace AppTests
{
    class TestUtils
    {

        public static Customer DefaultValidCustomer()
        {
            return new Customer()
            {
                Id = 1,
                Firstname = "Jeff",
                Surname = "Goldbloom",
                DateOfBirth = new DateTime(1952, 10, 22),
                EmailAddress = "JeffGoldbloom@goldmail.com",
                HasCreditLimit = true,
                CreditLimit = 10000,
                Company = DefaultGoldCompany()
            };
        }

        public static Company DefaultGoldCompany()
        {
            return new Company()
            {
                Classification = Classification.Gold,
                Id = 1,
                Name = "GoldMine"
            };
        }
    }
}
