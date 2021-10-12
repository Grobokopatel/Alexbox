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

        public string Submission
        {
            get;
        }

        public PlayerSubmitArgs(Player player, string submission)
        {
            Player = player;
            Submission = submission;
        }
    }
}
