using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TileHelper
{
    public partial class Form1 : Form
    {
        Panel[] p;

        public Form1()
        {
            InitializeComponent();

            p = new Panel[8];
            p[0] = panel1;
            p[1] = panel2;
            p[2] = panel3;
            p[3] = panel4;
            p[4] = panel5;
            p[5] = panel6;
            p[6] = panel7;
            p[7] = panel8;
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (textBox1.Text == "")
                return;

            int i = Int32.Parse(textBox1.Text);

            for (int j = 0; j < 8; j++)
            {
                if ((i & (int)Math.Pow(2, j)) > 0)
                    p[j].BackColor = Color.Red;
                else
                    p[j].BackColor = Color.Transparent;
                    
            }
        }
    }
}
