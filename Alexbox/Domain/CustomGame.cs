using Alexbox.View;
using System.Collections.Generic;
using System.Windows.Forms;
using static System.Windows.Forms.Control;

namespace Alexbox.Domain
{
    public sealed class CustomGame
    {
        public readonly Dictionary<long, Player> Players = new();
        public readonly Dictionary<long, Viewer> Viewers = new();
        public readonly GameStatus GameStatus;
        private readonly int MinPlayers;
        public readonly int MaxPlayers;
        private readonly string _name;
        private readonly Queue<Stage> _stages;

        public CustomGame(int minPlayers, int maxPlayers, string name)
        {
            GameStatus = GameStatus.WaitingForPlayers;
            MinPlayers = minPlayers;
            MaxPlayers = maxPlayers;
            _name = name;
            _stages = new Queue<Stage>();
        }

        public Player GetBestPlayer()
        {
            return Players.Max(item => item.Value.Score).Value;
        }

        public CustomGame AddStage(Stage stage)
        {
            _stages.Enqueue(stage);
            return this;
        }

        public CustomGame AddStages(IEnumerable<Stage> stages)
        {
            foreach (var stage in stages)
            {
                _stages.Enqueue(stage);
            }

            return this;
        }

        private ControlCollection _controls;
        private Stage _currentStage;

        public void Start(Panel panel)
        {
            _controls = panel.Controls;

            var lobby = new Form1();
            lobby.Click += (_, _) =>
            {
                _controls.Remove(lobby);
                AddNextStageToControls();
            };
            _controls.Add(lobby);
        }

        private void AddNextStageToControls()
        {
            _currentStage = _stages.Dequeue();
            _controls.Add(_currentStage);
            _currentStage.Ended += ChangeStage;
        }

        private void ChangeStage(TerminationType type)
        {
            _controls.Remove(_currentStage);
            _currentStage.Ended -= ChangeStage;
            if (_stages.Count != 0)
            {
                AddNextStageToControls();
            }
        }
    }
}