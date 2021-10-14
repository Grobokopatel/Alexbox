using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AlexBox.Views
{
    public partial class StartForm : Form
    {
        public StartForm()
        {
            InitializeComponent();
            Text = "AlexBox";
            var hostOrPlayerControl = new HostOrPlayerControl();
            Controls.Add(hostOrPlayerControl);
        }
    }
}
