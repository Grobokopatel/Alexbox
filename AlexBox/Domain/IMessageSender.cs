using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AlexBox.Domain
{
    public interface IMessageSender
    {
        void StartRecievingMessagesAsync();

        Task<byte[]> SendAsync(string address, int port, byte[] data);

        public event EventHandler<MessageRecievedEventArgs> MessageRecieved;

        int Port
        {
            get;
        }
    }
}
