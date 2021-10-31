using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AlexBox.Domain
{
    public interface IMessageSender
    {
        public event EventHandler<MessageRecievedEventArgs> MessageRecieved;
        Task<byte[]> SendAsync(string address, int port, byte[] data);
        void StartRecievingMessagesAsync();
        int Port
        {
            get;
        }
    }
}
