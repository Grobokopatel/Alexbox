using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Alexbox.Domain
{
    public abstract class MessageSender
    {
        public abstract void StartReceivingMessagesAsync();

        public abstract Task<byte[]> SendAsync(string address, int port, byte[] data);

        public IFormatter Formatter
        {
            get;
        }

        public MessageSender(IFormatter formatter)
        {
            Formatter = formatter;
        }

        public abstract event EventHandler<MessageRecievedEventArgs> MessageRecieved;

        public abstract int Port
        {
            get;
        }
    }
}
