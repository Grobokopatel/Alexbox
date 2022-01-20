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
        private Timer _timer;
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
            TelegramBot.AllPlayersAnswered += () =>
            {
                _timer?.Stop();
                ChangeStage();
            };
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
            _timer = new Timer();
            _timer.Interval = _currentGame.CurrentStage.TimeOutInMs;
            _timer.Start();
            _timer.Tick += (_, _) =>
            {
                _timer.Stop();
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
            TelegramBot.Client.SendTextMessageAsync(454489305, "CHANGE STAGE");
            if (_currentGame.CurrentStage is {ShowScores: true})
            {
                _currentGame.GameStatus = GameStatus.WaitingForReplies;
                _currentGame.CurrentRound += 1;
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