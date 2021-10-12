using System;
using System.Collections.Generic;
using System.Text;

namespace AlexBox
{
    public interface IMessageSender
    {
        public event EventHandler<byte[]> MessageRecieved;
        byte[] Send(string address, int port, byte[] data);
        void StartRecievingMessages();
    }
}
