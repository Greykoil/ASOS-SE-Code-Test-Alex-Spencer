using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using App;
using Moq;

namespace AppTests
{
    [TestClass]
    public class CustomerServiceTests
    {

        CustomerService _customerService;
        Mock<ICustomerCreditServiceWrapper> _creditServiceWrapperMock = new Mock<ICustomerCreditServiceWrapper>();
        Mock<ICustomerDataAccessWrapper> _customerDataAccessMock = new Mock<ICustomerDataAccessWrapper>();
        Mock<ICompanyRepository> _companyRepositoryMock = new Mock<ICompanyRepository>();

        public CustomerServiceTests()
        {
            //Use -1 as a default value for a company that does not exist in the database
            _companyRepositoryMock.Setup(x => x.GetById(It.Is<int>(y => y == -1))).Returns((Company)null);
            _companyRepositoryMock.Setup(x => x.GetById(It.Is<int>(y => y != -1))).Returns(TestUtils.DefaultGoldCompany);

            _creditServiceWrapperMock.Setup(x => x.HasCreditLimit(It.Is<Customer>(y => y.Firstname == "Rich"))).Returns(false);
            _creditServiceWrapperMock.Setup(x => x.HasCreditLimit(It.Is<Customer>(y => y.Firstname != "Rich"))).Returns(true);
            _creditServiceWrapperMock.Setup(x => x.CreditLimit(It.Is<Customer>(y => y.Firstname == "Poor"))).Returns(150);
            _creditServiceWrapperMock.Setup(x => x.CreditLimit(It.Is<Customer>(y => y.Firstname != "Poor"))).Returns(2000);

            _customerDataAccessMock.Setup(x => x.AddCustomer(It.IsAny<Customer>())).Returns(true);

            _customerService = new CustomerService(_companyRepositoryMock.Object, _creditServiceWrapperMock.Object, _customerDataAccessMock.Object);
        }


        [TestMethod]
        public void AddingValidCustomer()
        {
            var defaultCustomer = TestUtils.DefaultValidCustomer();

            var result = _customerService.AddCustomer(
                defaultCustomer.Firstname,
                defaultCustomer.Surname,
                defaultCustomer.EmailAddress,
                defaultCustomer.DateOfBirth,
                defaultCustomer.Company.Id
            );

            Assert.IsTrue(result);

            _creditServiceWrapperMock.Verify(x => x.HasCreditLimit(It.IsAny<Customer>()), Times.Exactly(1));

            _creditServiceWrapperMock.Verify(x => x.CreditLimit(It.IsAny<Customer>()), Times.Exactly(1));

            _companyRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Exactly(1));

            _customerDataAccessMock.Verify(x => x.AddCustomer(It.Is<Customer>(y =>
                y.Firstname == defaultCustomer.Firstname &&
                y.Surname == defaultCustomer.Surname &&
                y.EmailAddress == defaultCustomer.EmailAddress &&
                y.DateOfBirth == defaultCustomer.DateOfBirth &&
                y.Company.Id == defaultCustomer.Company.Id && 
                y.HasCreditLimit == true && 
                y.CreditLimit == 2000
                )), Times.Exactly(1));

        }

        [TestMethod]
        public void AddingInvalidFirstname()
        {
            var defaultCustomer = TestUtils.DefaultValidCustomer();

            var result = _customerService.AddCustomer(
                null,
                defaultCustomer.Surname,
                defaultCustomer.EmailAddress,
                defaultCustomer.DateOfBirth,
                defaultCustomer.Company.Id
            );

            Assert.IsFalse(result);

            _creditServiceWrapperMock.Verify(x => x.HasCreditLimit(It.IsAny<Customer>()), Times.Exactly(0));

            _creditServiceWrapperMock.Verify(x => x.CreditLimit(It.IsAny<Customer>()), Times.Exactly(0));

            _companyRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Exactly(0));
        }

        [TestMethod]
        public void AddingInvalidSurname()
        {
            var defaultCustomer = TestUtils.DefaultValidCustomer();

            var result = _customerService.AddCustomer(
                defaultCustomer.Firstname,
                null,
                defaultCustomer.EmailAddress,
                defaultCustomer.DateOfBirth,
                defaultCustomer.Company.Id
            );

            Assert.IsFalse(result);
            _creditServiceWrapperMock.Verify(x => x.HasCreditLimit(It.IsAny<Customer>()), Times.Exactly(0));

            _creditServiceWrapperMock.Verify(x => x.CreditLimit(It.IsAny<Customer>()), Times.Exactly(0));

            _companyRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Exactly(0));
        }

        [TestMethod]
        public void AddingNullEmail()
        {
            var defaultCustomer = TestUtils.DefaultValidCustomer();

            var result = _customerService.AddCustomer(
                defaultCustomer.Firstname,
                defaultCustomer.Surname,
                null,
                defaultCustomer.DateOfBirth,
                defaultCustomer.Company.Id
            );

            Assert.IsFalse(result);
            _creditServiceWrapperMock.Verify(x => x.HasCreditLimit(It.IsAny<Customer>()), Times.Exactly(0));

            _creditServiceWrapperMock.Verify(x => x.CreditLimit(It.IsAny<Customer>()), Times.Exactly(0));

            _companyRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Exactly(0));
        }

        [TestMethod]
        public void AddingInvalidEmail()
        {
            var defaultCustomer = TestUtils.DefaultValidCustomer();

            var result = _customerService.AddCustomer(
                defaultCustomer.Firstname,
                defaultCustomer.Surname,
                "notaproperemail",
                defaultCustomer.DateOfBirth,
                defaultCustomer.Company.Id
            );

            Assert.IsFalse(result);
            _creditServiceWrapperMock.Verify(x => x.HasCreditLimit(It.IsAny<Customer>()), Times.Exactly(0));

            _creditServiceWrapperMock.Verify(x => x.CreditLimit(It.IsAny<Customer>()), Times.Exactly(0));

            _companyRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Exactly(0));
        }

        [TestMethod]
        public void AddingTooYoungCustomer()
        {
            var defaultCustomer = TestUtils.DefaultValidCustomer();

            var result = _customerService.AddCustomer(
                defaultCustomer.Firstname,
                defaultCustomer.Surname,
                defaultCustomer.EmailAddress,
                DateTime.Now,
                defaultCustomer.Company.Id
            );

            Assert.IsFalse(result);
            _creditServiceWrapperMock.Verify(x => x.HasCreditLimit(It.IsAny<Customer>()), Times.Exactly(0));

            _creditServiceWrapperMock.Verify(x => x.CreditLimit(It.IsAny<Customer>()), Times.Exactly(0));

            _companyRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Exactly(0));
        }

        [TestMethod]
        public void InvalidCompanyId()
        {
            var defaultCustomer = TestUtils.DefaultValidCustomer();

            var result = _customerService.AddCustomer(
                defaultCustomer.Firstname,
                defaultCustomer.Surname,
                defaultCustomer.EmailAddress,
                defaultCustomer.DateOfBirth,
                -1
            );

            Assert.IsFalse(result);
            
            _companyRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Exactly(1));

            _creditServiceWrapperMock.Verify(x => x.HasCreditLimit(It.IsAny<Customer>()), Times.Exactly(0));

            _creditServiceWrapperMock.Verify(x => x.CreditLimit(It.IsAny<Customer>()), Times.Exactly(0));

        }

        [TestMethod]
        public void AddingUnlimitedCreditCustomer()
        {
            var defaultCustomer = TestUtils.DefaultValidCustomer();

            var result = _customerService.AddCustomer(
                "Rich",
                defaultCustomer.Surname,
                defaultCustomer.EmailAddress,
                defaultCustomer.DateOfBirth,
                defaultCustomer.Company.Id
            );

            Assert.IsTrue(result);

            _companyRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Exactly(1));

            _creditServiceWrapperMock.Verify(x => x.HasCreditLimit(It.IsAny<Customer>()), Times.Exactly(1));

            _creditServiceWrapperMock.Verify(x => x.CreditLimit(It.IsAny<Customer>()), Times.Exactly(0));

            _customerDataAccessMock.Verify(x => x.AddCustomer(It.Is<Customer>(y =>
                y.Firstname == "Rich" &&
                y.Surname == defaultCustomer.Surname &&
                y.EmailAddress == defaultCustomer.EmailAddress &&
                y.DateOfBirth == defaultCustomer.DateOfBirth &&
                y.Company.Id == defaultCustomer.Company.Id &&
                y.HasCreditLimit == false
                )), Times.Exactly(1));
        }

        [TestMethod]
        public void AddingLowCreditCustomer()
        {
            var defaultCustomer = TestUtils.DefaultValidCustomer();

            var result = _customerService.AddCustomer(
                "Poor",
                defaultCustomer.Surname,
                defaultCustomer.EmailAddress,
                defaultCustomer.DateOfBirth,
                defaultCustomer.Company.Id
            );

            Assert.IsFalse(result);

            _companyRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Exactly(1));

            _creditServiceWrapperMock.Verify(x => x.HasCreditLimit(It.IsAny<Customer>()), Times.Exactly(1));

            _creditServiceWrapperMock.Verify(x => x.CreditLimit(It.IsAny<Customer>()), Times.Exactly(1));
        }
    }
}
