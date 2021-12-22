using Alexbox.Domain;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Alexbox.View
{
    public abstract class Page : UserControl
    {
        //public Page WaitPlayerRepliesOrTimout(60000)
        
        //Пока не работает
        public Page WithBackground(Image image)
        {
            BackgroundImage = image;
            return this;
        }

        public Page WithParagraph(string text)
        {
            paragraph.Text = text;
            return this;
        }

        public Action<TerminationType> Ended;
        protected readonly TableLayoutPanel controlTable;
        private Label paragraph;

        public Page()
        {
            /*            this.components = new System.ComponentModel.Container();
                        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            */
            controlTable = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
            };

            paragraph = new Label
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Arial", 30),
            };

            controlTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 1));
            controlTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 80));
            controlTable.Controls.Add(paragraph/*, 0, 0*/);
            Dock = DockStyle.Fill;
            Controls.Add(controlTable);

            Load += (sender, e) =>
            {
                var timer = new Timer();
                timer.Interval = 3000;
                timer.Start();
                timer.Tick += (sender, e) =>
                { Ended(TerminationType.Timeout); timer.Stop(); };
            };
        }
    }
}