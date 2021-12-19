using System.Drawing;
using System.Windows.Forms;

namespace Alexbox.View
{
    public class TextPage : Page
    {
        public TextPage(string text)
        {
            var label = new Label
            {
                Text = text,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 16),
            };

            controlTable.RowStyles.Add(new RowStyle(SizeType.Percent, 5));
            controlTable.Controls.Add(label/*, 0, 1*/);
        }
    }
}