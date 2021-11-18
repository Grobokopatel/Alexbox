using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Alexbox.Domain
{
    public interface IMessageSender
    {
        void StartRecievingMessagesAsync();

        public IFormatter Formatter
        {
            get;
        }

        Task<byte[]> SendAsync(string address, int port, byte[] data);

        public event EventHandler<MessageRecievedEventArgs> MessageRecieved;

        int Port
        {
            get;
        }
    }
}
