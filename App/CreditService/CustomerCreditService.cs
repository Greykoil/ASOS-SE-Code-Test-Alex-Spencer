using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName = "App.ICustomerCreditService")]
    public interface ICustomerCreditService
    {

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ICustomerCreditService/GetCreditLimit", ReplyAction = "http://tempuri.org/ICustomerCreditService/GetCreditLimitResponse")]
        int GetCreditLimit(Customer customer);
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ICustomerCreditServiceChannel : App.ICustomerCreditService, System.ServiceModel.IClientChannel
    {
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class CustomerCreditServiceClient : System.ServiceModel.ClientBase<App.ICustomerCreditService>, App.ICustomerCreditService
    {

        public CustomerCreditServiceClient()
        {
        }

        public CustomerCreditServiceClient(string endpointConfigurationName) :
            base(endpointConfigurationName)
        {
        }

        public CustomerCreditServiceClient(string endpointConfigurationName, string remoteAddress) :
            base(endpointConfigurationName, remoteAddress)
        {
        }

        public CustomerCreditServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) :
            base(endpointConfigurationName, remoteAddress)
        {
        }

        public CustomerCreditServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :
            base(binding, remoteAddress)
        {
        }

        internal bool HasCreditLimit(Customer customer)
        {
            // This information obviously wants to be attached to the credit database in some way not in the code.
            if (customer.Company.Name == "VeryImportantClient")
            {
                return false;
            }
            return true;
        }

        public int GetCreditLimit(Customer customer)
        {
            int creditLimit = base.Channel.GetCreditLimit(customer);

            // As with above this information should be attached to the credit database not a raw string in the code
            if (customer.Company.Name == "ImportantClient")
            {
                creditLimit *= 2;
            }

            return creditLimit;
        }
    }
}
