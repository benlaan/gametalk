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
    public partial class FormTileHelper : Form
    {
        Panel[] _panels;
        Color cSELECTED_COLOUR = Color.Red;

        public FormTileHelper()
        {
            InitializeComponent();

            _panels = new Panel[8];
            _panels[0] = panel1;
            _panels[1] = panel2;
            _panels[2] = panel3;
            _panels[3] = panel4;
            _panels[4] = panel5;
            _panels[5] = panel6;
            _panels[6] = panel7;
            _panels[7] = panel8;
        }

        private void UpdateFormText()
        {
            this.Text = edValue.Text;
        }

        private void Recalculate()
        {
            int result = 0;
            for (int index = 0; index < 8; index++)
            {
                if (_panels[index].BackColor == cSELECTED_COLOUR)
                    result += (int)Math.Pow(2, index);
            }
            edValue.Text = result.ToString();

            UpdateFormText();
        }

        private void Swap(Dictionary<int, int> data)
        {
            foreach (KeyValuePair<int, int> pair in data)
            {
                Color from = _panels[pair.Key].BackColor;
                Color to   = _panels[pair.Value].BackColor;

                Color swap = from;
                _panels[pair.Key].BackColor = to;
                _panels[pair.Value].BackColor = swap;
            }
            Recalculate();
        }

        private void edValue_KeyUp(object sender, KeyEventArgs e)
        {
            if (edValue.Text == "")
                return;

            int value = Int32.Parse(edValue.Text);

            for (int index = 0; index < 8; index++)
                _panels[index].BackColor = GetColour(
                        (value & (int)Math.Pow(2, index)) > 0
                    );

            UpdateFormText();
        }

        public Color GetColour(bool value)
        {
            return value ? cSELECTED_COLOUR : Color.Transparent;
        }

        private void AnyPanel_Click(object sender, EventArgs e)
        {
            if (sender is Panel)
            {
                Panel panel = sender as Panel;
                panel.BackColor = GetColour(panel.BackColor != cSELECTED_COLOUR);

                Recalculate();
            }
        }

        private void btnMirror_Click(object sender, EventArgs e)
        {
            Dictionary<int, int> data = new Dictionary<int, int>();
            data[0] = 2;
            data[3] = 4;
            data[5] = 7;

            Swap(data);
        }

        private void btnFlip_Click(object sender, EventArgs e)
        {
            Dictionary<int, int> data = new Dictionary<int,int>();
            data[0] = 5;
            data[1] = 6;
            data[2] = 7;

            Swap(data);
        }

    }
}
