using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alexbox.Domain
{
    public enum GameState
    {
        Preparation,
        Act,
        Credits
    }

    public abstract class GameBase : Entity
    {
        protected List<Player> players = new List<Player>();

        public abstract int MinPlayers
        {
            get;
        }

        public abstract int MaxPlayers
        {
            get;
        }

        public int CurrentPlayersNumber => players.Count;

        public bool IsEnoughPlayers => CurrentPlayersNumber >= MinPlayers;

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

        public GameBase()
        {
            PlayerSubmit += HandleSubmit;
        }

        public event EventHandler<PlayerLoginEventArgs> PlayerLogin;
        public event EventHandler<PlayerSubmitEventArgs> PlayerSubmit;

        public void TryAddPlayer(object sender, PlayerLoginEventArgs args)
        {
            var player = args.Player;

            lock (players)
            {
                if (CurrentPlayersNumber >= MaxPlayers)
                {
                    args.Result = LoginResult.GameIsFull;
                }
                else if (players.Any(otherPlayer => otherPlayer.Name == player.Name))
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

        protected void InvokePlayerLogin(object sender, PlayerLoginEventArgs args)
        {
            PlayerLogin(sender, args);
        }

        protected void HandleSubmit(object sender, PlayerSubmitEventArgs args)
        {
            var name = args.PlayerName;
            var submission = args.Submit;

            var player = players.Find(player => player.Name == name);

            if (player is null)
                throw new ArgumentException("Игрока с таким именем нет в списке игроков");

            player.AddSubmission(submission, Round);
        }

        protected void InvokePlayerSubmit(object sender, PlayerSubmitEventArgs args)
        {
            PlayerSubmit(sender, args);
        }
    }
}
