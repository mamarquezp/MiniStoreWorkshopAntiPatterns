using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniStore.Domain.Abstractions
{
    // Interfaces segregadas (ISP) se omite push porque nadie lo usa
    public interface IEmailNotifier
    {
        void SendEmail(string to, string subject, string body);
    }

    public interface ISmsNotifier
    {
        void SendSms(string phone, string message);
    }
}
