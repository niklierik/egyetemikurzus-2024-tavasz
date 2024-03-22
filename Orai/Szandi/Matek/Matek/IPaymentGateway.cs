using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matek
{
    public interface IPaymentGateway
    {
        bool ProcessPayment(double amount);
    }
}
