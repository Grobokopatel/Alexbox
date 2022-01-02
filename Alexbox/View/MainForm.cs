using System.Drawing;
using System.Windows.Forms;
using Alexbox.Domain;

namespace Alexbox.View
{
    public partial class MainForm : Form
    {
        private Panel Panel { get; }
        private readonly CustomGame _currentGame;
        private Stage _currentStage;

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
                Panel.Controls.Remove(lobby);
                AddNextStageToControls();
            };
            Panel.Controls.Add(lobby);
        }

        private void AddNextStageToControls()
        {
            _currentStage = _currentGame._stages.Dequeue();
            Panel.Controls.Add(new StagePresenter(_currentStage));
            _currentStage.Ended += ChangeStage;
        }

        private void ChangeStage(TerminationType type)
        {
            Panel.Controls.Remove(_currentStage);
            _currentStage.Ended -= ChangeStage;
            if (_currentGame._stages.Count != 0)
            {
                AddNextStageToControls();
            }
            else Close();
        }
    }
}