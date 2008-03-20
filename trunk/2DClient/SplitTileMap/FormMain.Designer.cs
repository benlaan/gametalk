using System.Windows.Forms;
namespace SplitTileMap
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelControl2 = new System.Windows.Forms.Panel();
            this.pnlMap = new SplitTileMap.MapPanel();
            this.SuspendLayout();
            // 
            // panelControl2
            // 
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl2.Location = new System.Drawing.Point(0, 574);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(663, 65);
            this.panelControl2.TabIndex = 1;
            // 
            // pnlMap
            // 
            this.pnlMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMap.Location = new System.Drawing.Point(0, 0);
            this.pnlMap.Name = "pnlMap";
            this.pnlMap.Size = new System.Drawing.Size(663, 574);
            this.pnlMap.TabIndex = 2;
            this.pnlMap.ZoomScale = 0;
            this.pnlMap.OnFPSUpdate += new System.EventHandler(this.pnlMap_OnFPSUpdate);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(663, 639);
            this.Controls.Add(this.pnlMap);
            this.Controls.Add(this.panelControl2);
            this.DoubleBuffered = true;
            this.KeyPreview = true;
            this.Name = "FormMain";
            this.Text = "FormMain";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormMain_FormClosed);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FormKeyPressUp);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormKeyPressDown);
            this.ResumeLayout(false);

        }

        #endregion

        private Panel panelControl2;
        private MapPanel pnlMap;
    }
}