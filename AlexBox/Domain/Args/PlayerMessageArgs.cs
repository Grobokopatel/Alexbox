using System;
using System.Collections.Generic;
using System.Text;

namespace AlexBox
{
    public class PlayerMessageArgs : EventArgs
    {
        public Player Player
        {
            get;
        }

        public string Message
        {
            get;
        }

        public PlayerMessageArgs(Player player, string message)
        {
            Player = player;
            Message = message;
        }
    }
}
