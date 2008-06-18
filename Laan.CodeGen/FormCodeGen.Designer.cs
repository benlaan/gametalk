namespace Laan.CodeGen
{
    partial class FormCodeGen
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.gbCodeGen = new System.Windows.Forms.GroupBox();
            this.edOuputPath = new System.Windows.Forms.TextBox();
            this.edMapFile = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnExecute = new System.Windows.Forms.Button();
            this.edOutput = new System.Windows.Forms.TextBox();
            this.dlgBrowseFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.dlgMapFile = new System.Windows.Forms.OpenFileDialog();
            this.cbRebuildEntities = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.gbCodeGen.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gbCodeGen);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(10);
            this.panel1.Size = new System.Drawing.Size(608, 126);
            this.panel1.TabIndex = 0;
            // 
            // gbCodeGen
            // 
            this.gbCodeGen.Controls.Add(this.cbRebuildEntities);
            this.gbCodeGen.Controls.Add(this.edOuputPath);
            this.gbCodeGen.Controls.Add(this.edMapFile);
            this.gbCodeGen.Controls.Add(this.label2);
            this.gbCodeGen.Controls.Add(this.label1);
            this.gbCodeGen.Controls.Add(this.btnExecute);
            this.gbCodeGen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbCodeGen.Location = new System.Drawing.Point(10, 10);
            this.gbCodeGen.Name = "gbCodeGen";
            this.gbCodeGen.Size = new System.Drawing.Size(588, 106);
            this.gbCodeGen.TabIndex = 3;
            this.gbCodeGen.TabStop = false;
            this.gbCodeGen.Text = " Parameters ";
            // 
            // edOuputPath
            // 
            this.edOuputPath.Location = new System.Drawing.Point(94, 52);
            this.edOuputPath.Name = "edOuputPath";
            this.edOuputPath.Size = new System.Drawing.Size(488, 20);
            this.edOuputPath.TabIndex = 8;
            this.edOuputPath.Text = ".\\..\\..\\..\\CodeGen\\output";
            this.edOuputPath.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.edOuputPath_MouseDoubleClick);
            // 
            // edMapFile
            // 
            this.edMapFile.Location = new System.Drawing.Point(94, 26);
            this.edMapFile.Name = "edMapFile";
            this.edMapFile.Size = new System.Drawing.Size(488, 20);
            this.edMapFile.TabIndex = 7;
            this.edMapFile.Text = ".\\..\\..\\RiskMap.xml";
            this.edMapFile.TextChanged += new System.EventHandler(this.edMapFile_TextChanged);
            this.edMapFile.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.edMapFile_MouseDoubleClick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Output Path";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(41, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Map File";
            // 
            // btnExecute
            // 
            this.btnExecute.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnExecute.Location = new System.Drawing.Point(509, 78);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(73, 23);
            this.btnExecute.TabIndex = 0;
            this.btnExecute.Text = "Execute";
            this.btnExecute.UseVisualStyleBackColor = true;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // edOutput
            // 
            this.edOutput.BackColor = System.Drawing.Color.Black;
            this.edOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.edOutput.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.edOutput.ForeColor = System.Drawing.Color.Lime;
            this.edOutput.Location = new System.Drawing.Point(0, 126);
            this.edOutput.Multiline = true;
            this.edOutput.Name = "edOutput";
            this.edOutput.Size = new System.Drawing.Size(608, 459);
            this.edOutput.TabIndex = 1;
            // 
            // dlgMapFile
            // 
            this.dlgMapFile.FileName = "openFileDialog1";
            // 
            // cbRebuildEntities
            // 
            this.cbRebuildEntities.AutoSize = true;
            this.cbRebuildEntities.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbRebuildEntities.Location = new System.Drawing.Point(9, 78);
            this.cbRebuildEntities.Name = "cbRebuildEntities";
            this.cbRebuildEntities.Size = new System.Drawing.Size(99, 17);
            this.cbRebuildEntities.TabIndex = 9;
            this.cbRebuildEntities.Text = "Rebuild Entities";
            this.cbRebuildEntities.UseVisualStyleBackColor = true;
            // 
            // FormCodeGen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(608, 585);
            this.Controls.Add(this.edOutput);
            this.Controls.Add(this.panel1);
            this.Name = "FormCodeGen";
            this.Text = "Laan GameLibrary CodeGen";
            this.panel1.ResumeLayout(false);
            this.gbCodeGen.ResumeLayout(false);
            this.gbCodeGen.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox gbCodeGen;
        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.TextBox edOutput;
        private System.Windows.Forms.FolderBrowserDialog dlgBrowseFolder;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox edOuputPath;
        private System.Windows.Forms.TextBox edMapFile;
        private System.Windows.Forms.OpenFileDialog dlgMapFile;
        private System.Windows.Forms.CheckBox cbRebuildEntities;

    }
}

