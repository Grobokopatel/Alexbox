using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AlexBox
{
    public partial class TestForm2 : Form
    {
        public TestForm2()
        {
            InitializeComponent();
        }

        public TestForm2(TestForm1 form1)
        {
            InitializeComponent();
            form1.BackColor = Color.Yellow;
        }
    }
}
