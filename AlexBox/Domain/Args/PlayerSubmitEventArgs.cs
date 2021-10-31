using System;
using System.Collections.Generic;
using System.Text;

namespace AlexBox.Domain
{
    public class PlayerSubmitEventArgs : EventArgs
    {
        public Player Player
        {
            get;
        }

        public string Submit
        {
            get;
        }

        public PlayerSubmitEventArgs(Player player, string message)
        {
            Player = player;
            Submit = message;
        }
    }
}
