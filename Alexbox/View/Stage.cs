using Alexbox.Domain;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Alexbox.View
{
    public abstract class Stage : UserControl
    {
        //public Page WaitPlayerRepliesOrTimout(60000)
        
        //Пока не работает
        public Stage WithBackground(Image image)
        {
            BackgroundImage = image;
            return this;
        }

        public Stage WithParagraph(string text)
        {
            paragraph.Text = text;
            return this;
        }

        public Action<TerminationType> Ended;
        protected readonly TableLayoutPanel ControlTable;
        private Label paragraph;

        public Stage()
        {
            /*            this.components = new System.ComponentModel.Container();
                        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            */
            ControlTable = new TableLayoutPanel
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

            ControlTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 1));
            ControlTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 80));
            ControlTable.Controls.Add(paragraph/*, 0, 0*/);
            Dock = DockStyle.Fill;
            Controls.Add(ControlTable);

            Load += (_, _) =>
            {
                var timer = new Timer();
                timer.Interval = 3000;
                timer.Start();
                timer.Tick += (_, _) =>
                { Ended(TerminationType.Timeout); timer.Stop(); };
            };
        }
    }
}