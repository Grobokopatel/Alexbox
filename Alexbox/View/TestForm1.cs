using System;
using System.Drawing;
using System.Windows.Forms;

namespace Alexbox.View
{
    public partial class TestForm1 : Form
    {
        private Point _moveStart; // точка для перемещения

        public TestForm1()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            BackColor = Color.Yellow;
            var button1 = new Button
            {
                Location = new Point
                {
                    X = Width / 3,
                    Y = Height / 3
                }
            };
            button1.Text = "Закрыть";
            button1.Click += button1_Click;
            Controls.Add(button1); // добавляем кнопку на форму
            Load += Form1_Load;
            MouseDown += Form1_MouseDown;
            MouseMove += Form1_MouseMove;
        }

        public sealed override Color BackColor
        {
            get => base.BackColor;
            set => base.BackColor = value;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var myPath = new System.Drawing.Drawing2D.GraphicsPath();
            // создаем эллипс с высотой и шириной формы
            myPath.AddEllipse(0, 0, Width, Height);
            // создаем с помощью элипса ту область формы, которую мы хотим видеть
            var myRegion = new Region(myPath);
            // устанавливаем видимую область
            Region = myRegion;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            // если нажата левая кнопка мыши
            if (e.Button == MouseButtons.Left)
            {
                _moveStart = new Point(e.X, e.Y);
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            // если нажата левая кнопка мыши
            if ((e.Button & MouseButtons.Left) == 0) return;
            // получаем новую точку положения формы
            var deltaPos = new Point(e.X - _moveStart.X, e.Y - _moveStart.Y);
            // устанавливаем положение формы
            Location = new Point(this.Location.X + deltaPos.X, Location.Y + deltaPos.Y);
        }
    }
}
