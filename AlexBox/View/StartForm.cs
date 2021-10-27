using AlexBox.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AlexBox.View
{
    public partial class StartForm : Form
    {
        public StartForm(LocalNetworkGame game)
        {
            InitializeComponent();
            Text = "AlexBox";
            var hostOrPlayerControl = new HostOrPlayerControl(game);
            Controls.Add(hostOrPlayerControl);
        }
    }
}
