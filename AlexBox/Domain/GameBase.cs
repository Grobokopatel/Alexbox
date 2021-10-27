using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlexBox.Domain
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
        } = GameState.Preparation;

        public int Round
        {
            get;
            private set;
        }

        public event EventHandler<PlayerLoginEventArgs> PlayerLogin;
        protected event EventHandler<PlayerSubmitEventArgs> PlayerSubmit;

        protected void InvokePlayerLogin(object sender, PlayerLoginEventArgs args)
        {
            PlayerLogin(sender, args);
        }
        protected void HandleSubmit(object sender, PlayerSubmitEventArgs args)
        {
            if (!submits.ContainsKey(args.Player))
            {
                submits[args.Player] = args.Message;
            }
            else
            {
                submits[args.Player] += "===" + args.Message;
            }
        }

        protected void InvokePlayerSubmit(object sender, PlayerSubmitEventArgs args)
        {
            PlayerSubmit(sender, args);
        }

        protected Dictionary<Player, string> submits = new Dictionary<Player, string>();
        protected List<Player> players = new List<Player>();

        public int CurrentPlayersNumber => players.Count;

        public abstract int MinPlayers
        {
            get;
        }

        public abstract int MaxPlayers
        {
            get;
        }

        public bool IsEnoughPlayers => CurrentPlayersNumber >= MinPlayers;

        public void TryAddPlayer(object sender, PlayerLoginEventArgs args)
        {
            var player = args.Player; //(Player) sender        ?

            lock (players)
            {
                if (CurrentPlayersNumber >= MaxPlayers)
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
                    PlayerLogin(this, new PlayerLoginEventArgs(player));
                    args.Result = LoginResult.Success;
                }
            }
        }

        public GameBase()
        {
        }
    }
}
