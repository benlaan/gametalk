using System;
using System.Diagnostics;
using System.Windows.Forms;

using Laan.Utilities;
using Laan.Library.Debugging;

namespace Laan.CodeGen
{
    public partial class FormCodeGen : Form, IDebuggable
    {
        public FormCodeGen()
        {
            InitializeComponent();
            _debugger = new FormDebugger(this);
            Debug.Listeners.Add(_debugger);
        }

        #region FormDebugger Implementation
		
        private FormDebugger _debugger;

        delegate void AddLineHandler(string message);

        public void Write(string message)
        {
            this.Invoke(new AddLineHandler(OnAdd), new object[] { message });
        }

        public void WriteLine(string message)
        {
            this.Invoke(new AddLineHandler(OnAddLine), new object[] { message });
        }

        private void OnAddLine(string message)
        {
            if (edOutput != null)
                edOutput.AppendText(message + Environment.NewLine);

            this.Invalidate();
        }

        private void OnAdd(string message)
        {
            string sLastLine = edOutput.Lines[edOutput.Lines.Length - 1];
            sLastLine += message;
            edOutput.Lines[edOutput.Lines.Length - 1] = sLastLine;

            this.Invalidate();
        }

	    #endregion 

        private void btnExecute_Click(object sender, EventArgs e)
        {
            if (edMapFile.Text != "" && edOuputPath.Text != null)
                using (new Hourglass(this))
                {
                    CodeGenerator.Run(edMapFile.Text, edOuputPath.Text);
                }
        }

        private void edOutputPath_Click(object sender, EventArgs e)
        {
            dlgBrowseFolder.ShowDialog();
        }

        private void edOuputPath_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (dlgBrowseFolder.ShowDialog() == DialogResult.OK)
                edOuputPath.Text = dlgBrowseFolder.SelectedPath;
        }

        private void edMapFile_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (dlgMapFile.ShowDialog() == DialogResult.OK)
                edMapFile.Text = dlgMapFile.FileName;
        }

        private void edMapFile_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
