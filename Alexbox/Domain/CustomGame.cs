using Alexbox.View;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.Control;

namespace Alexbox.Domain
{
    public sealed class CustomGame
    {
        public Dictionary<long, Player> _players = new();
        public Dictionary<long, Viewer> _viewers = new();
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
            return _players.Max(item => item.Value.Score).Value;
        }

        public CustomGame AddGamePage(Page gamePage)
        {
            _pages.Enqueue(gamePage);
            return this;
        }

        public CustomGame AddGamePages(IEnumerable<Page> gamePages)
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

            var lobby = new Form1();
            lobby.Click += (s, a) =>
            {
                _controls.Remove(lobby);
                AddNextPageToControls();
            };
            _controls.Add(lobby);
        }

        private void AddNextPageToControls()
        {
            _currentPage = _pages.Dequeue();
            _controls.Add(_currentPage);
            _currentPage.Ended += ChangePage;
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
    }
}