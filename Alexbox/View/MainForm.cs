using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Alexbox.Application.TelegramBot;
using Alexbox.Domain;

namespace Alexbox.View
{
    public partial class MainForm : Form
    {
        private Panel Panel { get; }
        private readonly CustomGame _currentGame;

        public MainForm(CustomGame currentGame)
        {
            _currentGame = currentGame;
            InitializeComponent();
            ClientSize = new Size(1400, 700);
            Panel = new Panel
            {
                Dock = DockStyle.Fill,
            };
            Controls.Add(Panel);
        }

        public void Start()
        {
            TelegramBot.AllPlayersAnswered += ChangeStage;
            var lobby = new LobbyControl(_currentGame);
            lobby.Button.Click += (_, _) =>
            {
                if (_currentGame.Players.Count < _currentGame.MinPlayers)
                    MessageBox.Show("Не хвататает игроков для начала", "Ошибка");
                else
                {
                    ChangeStage();
                    lobby.timer.Stop();
                }
            };
            Panel.Controls.Add(lobby);
        }

        private void StartTimer()
        {
            var timer = new Timer();
            timer.Interval = _currentGame.CurrentStage.TimeOutInMs;
            timer.Start();
            timer.Tick += (_, _) =>
            {
                timer.Stop();
                ChangeStage();
            };
        }

        private void ChangeStage()
        {
            if (InvokeRequired)
            {
                Invoke((Action) ChangeStage);
                return;
            }

            if (_currentGame.Stages.Count == 0)
            {
                Close();
                return;
            }

            Panel.Controls.Clear();
            _currentGame.CurrentStage = _currentGame.Stages.Dequeue();
            var stagePresenter = new StagePresenter(_currentGame.CurrentStage, _currentGame);
            stagePresenter.AllTaskShown += ChangeStage;
            Panel.Controls.Add(stagePresenter);

            if (_currentGame.CurrentStage.SendingTasks)
            {
                _currentGame.CurrentStage.Distribution = new Distribution<long, Task>(
                    _currentGame.CurrentStage.TaskPerPlayer,
                    _currentGame.CurrentStage.GroupSize, _currentGame.Players.Select(player => player.Id).ToArray(),
                    _currentGame.Tasks);
                TelegramBot.SendTasks();
                _currentGame.GameStatus = GameStatus.WaitingForReplies;
            }

            if (_currentGame.CurrentStage.TimeOutInMs != 0)
                StartTimer();
        }
    }
}