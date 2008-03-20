using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace SplitTileMap
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void FormKeyPressDown(object sender, KeyEventArgs e)
        {
            pnlMap.CheckKeyScrollingDown(e);

            switch (e.KeyData)
            {
                case Keys.Add:
                    pnlMap.ZoomIn();
                    break;

                case Keys.Subtract:
                    pnlMap.ZoomOut();
                    break;
            }
        }

        private void FormKeyPressUp(object sender, KeyEventArgs e)
        {
            pnlMap.CheckKeyScrollingUp(e);
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            pnlMap.Start();
        }

        private void pnlMap_OnFPSUpdate(object sender, EventArgs e)
        {
            this.Text = "FPS: " + pnlMap.FPS;
        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            pnlMap.MissingIndexes.Sort();
            foreach (int index in pnlMap.MissingIndexes)
            {
                Debug.WriteLine(index);
            }
        }
    }
}