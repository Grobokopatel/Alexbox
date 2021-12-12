using System.Collections.Generic;
using System.Linq;

namespace Alexbox.Domain
{
    public sealed class CustomGame : Entity
    {
        public List<Player> _players = new();
        public List<Viewer> _viewers = new();
        public readonly GameStatus GameStatus;
        private string Instruction;
        private readonly int _minPlayers;
        public readonly int _maxPlayers;
        private readonly string _name;
        private readonly List<IGamePage> _pages;

        public CustomGame(int minPlayers, int maxPlayers, string name)
        {
            GameStatus = GameStatus.WaitingForPlayers;
            _minPlayers = minPlayers;
            _maxPlayers = maxPlayers;
            _name = name;
            _pages = new List<IGamePage>();
        }

        public Player GetBestPlayer()
        {
            var bestPlayer = _players.First();
            foreach (var player in _players)
            {
                if (bestPlayer.Score < player.Score)
                {
                    bestPlayer = player;
                }
            }

            return bestPlayer;
        }

        public CustomGame AddGamePage(IGamePage gamePage)
        {
            _pages.Add(gamePage);
            return this;
        }
        public CustomGame AddGamePages(List<IGamePage> gamePages)
        {
            foreach (var gamePage in gamePages)
            {
                _pages.Add(gamePage);
            }
            return this;
        }
    }
}