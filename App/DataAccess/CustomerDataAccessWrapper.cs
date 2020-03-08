namespace App
{
    public class CustomerDataAccessWrapper : ICustomerDataAccessWrapper
    {
        public bool AddCustomer(Customer customer)
        {
            return CustomerDataAccess.AddCustomer(customer);
        }
    }
}
