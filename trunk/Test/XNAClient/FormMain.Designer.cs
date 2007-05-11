namespace RiskClient
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
            this.pnlDetail = new System.Windows.Forms.Panel();
            this.btnTest = new System.Windows.Forms.Button();
            this.pnlNotification = new System.Windows.Forms.Panel();
            this.txtNotification = new System.Windows.Forms.TextBox();
            this.pageMapView = new System.Windows.Forms.TabControl();
            this.tabPhysical = new System.Windows.Forms.TabPage();
            this.tabMilitary = new System.Windows.Forms.TabPage();
            this.tabEconomy = new System.Windows.Forms.TabPage();
            this.tabPolitical = new System.Windows.Forms.TabPage();
            this.pnlMap = new System.Windows.Forms.Panel();
            this.pnlDetail.SuspendLayout();
            this.pnlNotification.SuspendLayout();
            this.pageMapView.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlDetail
            // 
            this.pnlDetail.Controls.Add(this.btnTest);
            this.pnlDetail.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlDetail.Location = new System.Drawing.Point(0, 0);
            this.pnlDetail.Name = "pnlDetail";
            this.pnlDetail.Size = new System.Drawing.Size(200, 540);
            this.pnlDetail.TabIndex = 0;
            // 
            // btnTest
            // 
            this.btnTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnTest.Location = new System.Drawing.Point(12, 508);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(182, 23);
            this.btnTest.TabIndex = 0;
            this.btnTest.Text = "Test";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // pnlNotification
            // 
            this.pnlNotification.Controls.Add(this.txtNotification);
            this.pnlNotification.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlNotification.Location = new System.Drawing.Point(200, 470);
            this.pnlNotification.Name = "pnlNotification";
            this.pnlNotification.Size = new System.Drawing.Size(490, 70);
            this.pnlNotification.TabIndex = 1;
            // 
            // txtNotification
            // 
            this.txtNotification.BackColor = System.Drawing.Color.White;
            this.txtNotification.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNotification.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNotification.Location = new System.Drawing.Point(0, 0);
            this.txtNotification.Multiline = true;
            this.txtNotification.Name = "txtNotification";
            this.txtNotification.ReadOnly = true;
            this.txtNotification.Size = new System.Drawing.Size(490, 70);
            this.txtNotification.TabIndex = 0;
            // 
            // pageMapView
            // 
            this.pageMapView.Controls.Add(this.tabPhysical);
            this.pageMapView.Controls.Add(this.tabMilitary);
            this.pageMapView.Controls.Add(this.tabEconomy);
            this.pageMapView.Controls.Add(this.tabPolitical);
            this.pageMapView.Dock = System.Windows.Forms.DockStyle.Top;
            this.pageMapView.HotTrack = true;
            this.pageMapView.Location = new System.Drawing.Point(200, 0);
            this.pageMapView.Name = "pageMapView";
            this.pageMapView.Padding = new System.Drawing.Point(40, 3);
            this.pageMapView.SelectedIndex = 0;
            this.pageMapView.Size = new System.Drawing.Size(490, 24);
            this.pageMapView.TabIndex = 3;
            this.pageMapView.TabStop = false;
            // 
            // tabPhysical
            // 
            this.tabPhysical.Location = new System.Drawing.Point(4, 22);
            this.tabPhysical.Name = "tabPhysical";
            this.tabPhysical.Padding = new System.Windows.Forms.Padding(3);
            this.tabPhysical.Size = new System.Drawing.Size(482, 0);
            this.tabPhysical.TabIndex = 0;
            this.tabPhysical.Text = "Physical";
            this.tabPhysical.UseVisualStyleBackColor = true;
            // 
            // tabMilitary
            // 
            this.tabMilitary.Location = new System.Drawing.Point(4, 22);
            this.tabMilitary.Name = "tabMilitary";
            this.tabMilitary.Padding = new System.Windows.Forms.Padding(3);
            this.tabMilitary.Size = new System.Drawing.Size(482, -2);
            this.tabMilitary.TabIndex = 1;
            this.tabMilitary.Text = "Military";
            this.tabMilitary.UseVisualStyleBackColor = true;
            // 
            // tabEconomy
            // 
            this.tabEconomy.Location = new System.Drawing.Point(4, 22);
            this.tabEconomy.Name = "tabEconomy";
            this.tabEconomy.Size = new System.Drawing.Size(482, -2);
            this.tabEconomy.TabIndex = 2;
            this.tabEconomy.Text = "Economy";
            this.tabEconomy.UseVisualStyleBackColor = true;
            // 
            // tabPolitical
            // 
            this.tabPolitical.Location = new System.Drawing.Point(4, 22);
            this.tabPolitical.Name = "tabPolitical";
            this.tabPolitical.Size = new System.Drawing.Size(482, -2);
            this.tabPolitical.TabIndex = 3;
            this.tabPolitical.Text = "Political";
            this.tabPolitical.UseVisualStyleBackColor = true;
            // 
            // pnlMap
            // 
            this.pnlMap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.pnlMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMap.Location = new System.Drawing.Point(200, 24);
            this.pnlMap.Name = "pnlMap";
            this.pnlMap.Size = new System.Drawing.Size(490, 446);
            this.pnlMap.TabIndex = 4;
            this.pnlMap.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pnlMap_MouseClick);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(690, 540);
            this.Controls.Add(this.pnlMap);
            this.Controls.Add(this.pageMapView);
            this.Controls.Add(this.pnlNotification);
            this.Controls.Add(this.pnlDetail);
            this.Name = "FormMain";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.pnlDetail.ResumeLayout(false);
            this.pnlNotification.ResumeLayout(false);
            this.pnlNotification.PerformLayout();
            this.pageMapView.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlDetail;
        private System.Windows.Forms.Panel pnlNotification;
        private System.Windows.Forms.TabPage tabPhysical;
        private System.Windows.Forms.TabPage tabMilitary;
        private System.Windows.Forms.TabPage tabEconomy;
        private System.Windows.Forms.TabPage tabPolitical;
        private System.Windows.Forms.Panel pnlMap;
        private System.Windows.Forms.TextBox txtNotification;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.TabControl pageMapView;
    }
}

