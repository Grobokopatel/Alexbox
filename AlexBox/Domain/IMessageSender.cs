using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AlexBox
{
    public interface IMessageSender
    {
        public event EventHandler<MessageRecievedArgs> MessageRecieved;
        Task<byte[]> SendAsync(string address, int port, byte[] data);
        void StartRecievingMessagesAsync();
        int Port
        {
            get;
        }

        IPAddress LocalIPAddress
        {
            get;
        }
    }
}
