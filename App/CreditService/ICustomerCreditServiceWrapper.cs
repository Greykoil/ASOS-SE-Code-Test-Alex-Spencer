using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App
{
    public interface ICustomerCreditServiceWrapper
    {
        bool HasCreditLimit(Customer customer);

        int CreditLimit(Customer customer);
    }
}
