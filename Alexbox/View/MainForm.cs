using System.Drawing;
using System.Windows.Forms;
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
            var lobby = new LobbyControl();
            lobby.Button.Click += (_, _) =>
            {
                _currentGame.StopProgram += Close;
                _currentGame.Start();
                ChangeStage(TerminationType.Timeout);
            };
            Panel.Controls.Add(lobby);
        }


        private void ChangeStage(TerminationType type)
        {
            if (_currentGame.Stages.Count == 0) Close();
            Panel.Controls.Clear();
            Panel.Controls.Add(new StagePresenter(_currentGame.CurrentStage));
            _currentGame.StageEnded += ChangeStage;
        }
    }
}