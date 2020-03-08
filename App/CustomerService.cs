using System;

namespace App
{
    public class CustomerService : ICustomerService
    {

        private ICompanyRepository _companyRepository;
        private ICustomerCreditServiceWrapper _customerCreditServiceWrapper;
        private ICustomerDataAccessWrapper _customerDataAccessWrapper;

        public CustomerService()
        {
            SetWrappers(new CompanyRepository(), new CustomerCreditServiceWrapper(), new CustomerDataAccessWrapper());
        }

        public CustomerService(ICompanyRepository companyRepository, ICustomerCreditServiceWrapper customerCreditServiceWrapper, ICustomerDataAccessWrapper customerDataAccessWrapper)
        {
            SetWrappers(companyRepository, customerCreditServiceWrapper, customerDataAccessWrapper);
        }

        private void SetWrappers(ICompanyRepository companyRepository, ICustomerCreditServiceWrapper customerCreditServiceWrapper, ICustomerDataAccessWrapper customerDataAccessWrapper)
        {
            _companyRepository = companyRepository;
            _customerDataAccessWrapper = customerDataAccessWrapper;
            _customerCreditServiceWrapper = customerCreditServiceWrapper;
        }

        public bool AddCustomer(string firstname, string surname, string email, DateTime dateOfBirth, int companyId)
        {
            if (!ValidateCustomerDetails(firstname, surname, email, dateOfBirth, companyId))
            {
                return false;
            }

            var company = _companyRepository.GetById(companyId);

            if (company == null)
            {
                return false;
            }

            var customer = new Customer
            {
                Company = company,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                Firstname = firstname,
                Surname = surname
            };

            customer.HasCreditLimit = _customerCreditServiceWrapper.HasCreditLimit(customer);

            if (customer.HasCreditLimit)
            {
                customer.CreditLimit = _customerCreditServiceWrapper.CreditLimit(customer);
            }
            
            if (customer.HasCreditLimit && customer.CreditLimit < 500)
            {
                return false;
            }

            return _customerDataAccessWrapper.AddCustomer(customer);
        }

        private bool ValidateCustomerDetails(string firstname, string surname, string email, DateTime dateOfBirth, int companyId)
        {
            if (string.IsNullOrEmpty(firstname) || string.IsNullOrEmpty(surname) || string.IsNullOrEmpty(email))
            {
                return false;
            }

            if (!email.Contains("@") && !email.Contains("."))
            {
                return false;
            }

            var now = DateTime.Now;
            int age = now.Year - dateOfBirth.Year;
            if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day)) age--;

            if (age < 21)
            {
                return false;
            }

            return true;
        }
    }
}
