using System;

namespace App
{
    public interface ICustomerService
    {
        bool AddCustomer(string firstname, string surname, string email, DateTime dateOfBirth, int companyId);
    }
}