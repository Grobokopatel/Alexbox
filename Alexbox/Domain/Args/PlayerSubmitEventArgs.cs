using System;
using System.Collections.Generic;
using System.Text;

namespace Alexbox.Domain
{
    public class PlayerSubmitEventArgs : EventArgs
    {
        public string PlayerName
        {
            get;
        }

        public string Submit
        {
            get;
        }

        public PlayerSubmitEventArgs(string playerName, string message)
        {
            PlayerName = playerName;
            Submit = message;
        }
    }
}
