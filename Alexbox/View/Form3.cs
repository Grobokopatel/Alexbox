using System.Drawing;
using System.Windows.Forms;

namespace Alexbox.View
{
    public partial class Form3 : Form
    {
        public Panel Panel { get; set; }

        public Form3()
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
