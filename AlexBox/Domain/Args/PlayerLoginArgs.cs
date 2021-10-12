using System;
using System.Collections.Generic;
using System.Text;

namespace AlexBox
{
    public enum LoginResult
    {
        Success = 0,
        GameIsFull = 1,
        SomeoneHasSameNickname = 2
    }

    public class PlayerLoginArgs : EventArgs
    {
        public Player Player
        {
            get;
        }
        
        public LoginResult Result 
        {
            get;
            set;
        }

        public PlayerLoginArgs(Player player)
        {
            Player = player;
        }
    }
}
