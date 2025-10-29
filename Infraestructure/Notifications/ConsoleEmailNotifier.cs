using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniStore.Domain.Abstractions;

namespace MiniStore.Infrastructure.Notifications
{
    // ISP: Solo implementamos lo que se necesita
    public class ConsoleEmailNotifier : IEmailNotifier
    {
        public void SendEmail(string to, string subject, string body)
            => Console.WriteLine($"[EMAIL] To:{to} Subj:{subject} Body:{body}");
    }
}
