using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AlexBox
{
    public partial class TestForm1 : Form
    {
        public TestForm1()
        {
            InitializeComponent();
            Text = "Hello World!";
            BackColor = Color.Aquamarine;
            Width = 250;
            Height = 250;
            StartPosition = FormStartPosition.CenterScreen;

            var button = new Button()
            {
                Text = "Нажми на меня",
                AutoSize = true,
            };

            Controls.Add(button);

            button.Click += (sender, args) =>
            {
                var testForm1 = new TestForm1();
                testForm1.Show();

                var testForm2 = new TestForm2(this);
                testForm2.Show();
            };
        }
    }
}
