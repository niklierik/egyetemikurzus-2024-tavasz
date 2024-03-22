using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matek
{
    public class PaymentProcessor
    {
        private readonly IPaymentGateway _paymentGateway;

        public PaymentProcessor(IPaymentGateway paymentGateway)
        {
            _paymentGateway = paymentGateway;
        }

        public bool ProcessPayment(double amount)
        {
            Console.WriteLine($"Processing payment of {amount}...");
            //_paymentGateway.ProcessPayment(amount);
            return _paymentGateway.ProcessPayment(amount);
        }
    }
}
