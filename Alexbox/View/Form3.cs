﻿using Alexbox.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Alexbox.View
{
    public partial class Form3 : Form
    {
        public Panel Panel { get; set; }

        public Form3()
        {
            InitializeComponent();
            Panel = new Panel()
            {
                Dock = DockStyle.Fill,
                BackgroundImage = Image.FromFile(
                Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\2020-9-13 14-27-14.png")
        };
            Controls.Add(Panel);
        }
    }
}