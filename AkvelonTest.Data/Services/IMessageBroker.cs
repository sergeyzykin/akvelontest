using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkvelonTest
{
    public interface IMessageBroker
    {
        Task SendMessageAsync(string message);
    }
}
