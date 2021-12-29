using System.Drawing;
using System.Windows.Forms;

namespace Alexbox.View
{
    public partial class StartPanel : System.Windows.Forms.Form
    {
        public Panel Panel { get; }

        public StartPanel()
        {
            InitializeComponent();
            ClientSize = new Size(1400, 700);
            Panel = new Panel()
            {
                Dock = DockStyle.Fill,
            };
            Controls.Add(Panel);
        }
    }
}