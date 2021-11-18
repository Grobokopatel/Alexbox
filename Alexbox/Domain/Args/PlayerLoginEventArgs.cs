using System;
using System.Collections.Generic;
using System.Text;

namespace Alexbox.Domain
{
    public enum LoginResult
    {
        Success = 0,
        GameIsFull = 1,
        SomeoneHasSameNickname = 2
    }

    public class PlayerLoginEventArgs : EventArgs
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

        public PlayerLoginEventArgs(Player player)
        {
            Player = player;
        }
    }
}
