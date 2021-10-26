using System;
using System.Collections.Generic;
using System.Text;

namespace AlexBox
{
    public class PlayerSubmitEventArgs : EventArgs
    {
        public Player Player
        {
            get;
        }

        public string Message
        {
            get;
        }

        public PlayerSubmitEventArgs(Player player, string message)
        {
            Player = player;
            Message = message;
        }
    }
}
