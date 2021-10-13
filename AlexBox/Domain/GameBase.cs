﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlexBox
{
    public enum GameState
    {
        Preparation,
        Act,
        Credits
    }

    public abstract class GameBase
    {
        public GameState State
        {
            get;
            private set;
        }

        public int Round
        {
            get;
            private set;
        }

        public event EventHandler<PlayerLoginArgs> PlayerLogin;

        protected event EventHandler<PlayerSubmitArgs> PlayerSubmit;

        protected void InvokePlayerLogin(object sender, PlayerLoginArgs args)
        {
            PlayerLogin(sender, args);
        }

        protected void InvokePlayerSubmit(object sender, PlayerSubmitArgs args)
        {
            PlayerSubmit(sender, args);
        }

        protected List<Player> players = new List<Player>();

        public int PlayersNumber => players.Count;

        public abstract int MinPlayers
        {
            get;
        }

        public abstract int MaxPlayers
        {
            get;
        }

        public bool IsEnoughPlayers => PlayersNumber >= MinPlayers;

        public void TryAddPlayer(object sender, PlayerLoginArgs args)
        {
            var player = args.Player; //(Player) sender        ?

            lock (players)
            {
                if (PlayersNumber >= MaxPlayers)
                {
                    args.Result = LoginResult.GameIsFull;
                }
                else if(players.Any(otherPlayer => otherPlayer.Name == player.Name))
                {
                    args.Result = LoginResult.SomeoneHasSameNickname;
                }    
                else
                {
                    players.Add(player);
                    PlayerLogin(this, new PlayerLoginArgs(player));
                    args.Result = LoginResult.Success;
                }
            }
        }

        public GameBase()
        {
            State = GameState.Preparation;
        }
    }
}
