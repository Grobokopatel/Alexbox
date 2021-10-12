using System;
using System.Collections.Generic;
using System.Text;

namespace AlexBox.Domain
{
    public interface IMessageSender
    {
        void Send<TMessage>(TMessage bytes, string ip);
        TMessage Recieve<TMessage>();
    }
}
