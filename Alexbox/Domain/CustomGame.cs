using Alexbox.View;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.Control;

namespace Alexbox.Domain
{
    public sealed class CustomGame
    {
        public List<Player> _players = new();
        public List<Viewer> _viewers = new();
        public readonly GameStatus GameStatus;
        private readonly int _minPlayers;
        public readonly int _maxPlayers;
        private readonly string _name;
        private readonly Queue<Page> _pages;

        public CustomGame(int minPlayers, int maxPlayers, string name)
        {
            GameStatus = GameStatus.WaitingForPlayers;
            _minPlayers = minPlayers;
            _maxPlayers = maxPlayers;
            _name = name;
            _pages = new Queue<Page>();
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

        public CustomGame AddGamePage(Page gamePage)
        {
            _pages.Enqueue(gamePage);
            return this;
        }

        public CustomGame AddGamePages(List<Page> gamePages)
        {
            foreach (var gamePage in gamePages)
            {
                _pages.Enqueue(gamePage);
            }
            return this;
        }

        private ControlCollection _controls;
        private Page _currentPage;

        public void Start(Panel panel)
        {
            _controls = panel.Controls;

            AddNextPageToControls();
        }

        private void ChangePage(TerminationType type)
        {
            _controls.Remove(_currentPage);
            _currentPage.Ended -= ChangePage;
            if (_pages.Count != 0)
            {
                AddNextPageToControls();
            }
        }

        private void AddNextPageToControls()
        {
            _currentPage = _pages.Dequeue();
            _controls.Add(_currentPage);
            _currentPage.Ended += ChangePage;
        }
    }
}