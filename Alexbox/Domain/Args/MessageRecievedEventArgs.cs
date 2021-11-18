using System;
using System.Collections.Generic;
using System.Text;

namespace Alexbox.Domain
{
    public class MessageRecievedEventArgs : EventArgs
    {
        public string Result
        {
            get;
            set;
        }

        public byte[] Message
        {
            get;
        }

        public MessageRecievedEventArgs(byte[] message)
        {
            Message = message;
        }
    }
}
