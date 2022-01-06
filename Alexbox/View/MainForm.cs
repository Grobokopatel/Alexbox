using System.Drawing;
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
                    _currentGame.Start();
                    ChangeStage();
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
            Panel.Controls.Clear();
            if (_currentGame.Stages.Count == 0)
            {
                TelegramBot.Finish();
                Close();
                return;
            }

            _currentGame.CurrentStage = _currentGame.Stages.Dequeue();
            Panel.Controls.Add(new StagePresenter(_currentGame.CurrentStage, _currentGame));
            if (_currentGame.CurrentStage.SendingTasks)
            {
                TelegramBot.SendTasks();
                _currentGame.GameStatus = GameStatus.WaitingForReplies;
            }

            if (_currentGame.CurrentStage.TimeOutInMs != 0)
                StartTimer();
        }
    }
}