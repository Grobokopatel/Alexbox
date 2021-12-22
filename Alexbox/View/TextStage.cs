using System.Drawing;
using System.Windows.Forms;

namespace Alexbox.View
{
    public class TextStage : Stage
    {
        public TextStage(string text)
        {
            var label = new Label
            {
                Text = text,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 16),
            };

            ControlTable.RowStyles.Add(new RowStyle(SizeType.Percent, 5));
            ControlTable.Controls.Add(label/*, 0, 1*/);
        }
    }
}