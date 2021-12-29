using Alexbox.Domain;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Alexbox.View
{
    public abstract class Stage : UserControl
    {
        public Stage WaitForTimout(int milliseconds)
        {
            Load += (_, _) =>
            {
                var timer = new Timer();
                timer.Interval = milliseconds;
                timer.Start();
                timer.Tick += (_, _) =>
                {
                    Ended(TerminationType.Timeout);
                    timer.Stop();
                };
            };
            return this;
        }

        public Stage WithBackground(Image image)
        {
            BackgroundImage = image;
            return this;
        }

        public Stage WithParagraph(string text)
        {
            Paragraph.Text = text;
            return this;
        }

        public Action<TerminationType> Ended;
        protected readonly TableLayoutPanel ControlTable;
        protected readonly Label Paragraph;

        protected Stage()
        {
            /*            this.components = new System.ComponentModel.Container();
                        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            */
            ControlTable = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
            };

            Paragraph = new Label
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Arial", 30),
            };

            ControlTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 1));
            ControlTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 80));
            ControlTable.Controls.Add(Paragraph /*, 0, 0*/);
            Dock = DockStyle.Fill;
            Controls.Add(ControlTable);
        }
    }
}