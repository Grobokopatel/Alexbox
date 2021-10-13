using System;
using System.Collections.Generic;
using System.Text;

namespace AlexBox
{
    public class PlayerSubmitArgs : EventArgs
    {
        public Player Player
        {
            get;
        }

        public string Message
        {
            get;
        }

        public PlayerSubmitArgs(Player player, string message)
        {
            Player = player;
            Message = message;
        }
    }
}
