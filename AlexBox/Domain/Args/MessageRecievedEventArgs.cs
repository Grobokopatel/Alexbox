using System;
using System.Collections.Generic;
using System.Text;

namespace AlexBox.Domain
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
