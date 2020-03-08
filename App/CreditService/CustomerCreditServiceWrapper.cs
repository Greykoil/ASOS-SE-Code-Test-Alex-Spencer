using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App
{
    class CustomerCreditServiceWrapper : ICustomerCreditServiceWrapper
    {
        public int CreditLimit(Customer customer)
        {
            using (var customerCreditService = new CustomerCreditServiceClient())
            {
                return customerCreditService.GetCreditLimit(customer);
            }
        }

        public bool HasCreditLimit(Customer customer)
        {
            using (var customerCreditService = new CustomerCreditServiceClient())
            {
                return customerCreditService.HasCreditLimit(customer);
            }
        }
    }
}
