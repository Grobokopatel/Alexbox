using System;
using System.Collections.Generic;
using System.Text;

namespace AlexBox
{
    public class MessageRecievedArgs : EventArgs
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

        public MessageRecievedArgs(byte[] message)
        {
            Message = message;
        }
    }
}
