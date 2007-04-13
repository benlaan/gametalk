using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Diagnostics;
using Laan.GameLibrary;
using Laan.GameLibrary.Data;
using Laan.GameLibrary.Entity;
using PersonClasses = Laan.Test.Business.Person.Server;
using Laan.Library.ObjectTree;

namespace PersonViewer
{
	public class FormDebugger : DefaultTraceListener
	{
		public FormDebugger(frmServer form)
		{
			_form = form;
		}

		private frmServer _form;

		public override void Write(string data)
		{
			_form.Add(data);
		}

		public override void WriteLine(string data)
		{
			_form.AddLine(data);
		}
	}

	public class frmServer : System.Windows.Forms.Form
    {

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.panel1 = new System.Windows.Forms.Panel();
			this.lbClientCount = new System.Windows.Forms.Label();
			this.edClientCount = new System.Windows.Forms.TextBox();
			this.btnInitialise = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.edWeight = new System.Windows.Forms.TextBox();
			this.lbAge = new System.Windows.Forms.Label();
			this.lbName = new System.Windows.Forms.Label();
			this.edAge = new System.Windows.Forms.TextBox();
			this.edName = new System.Windows.Forms.TextBox();
			this.btnDelete = new System.Windows.Forms.Button();
			this.btnAdd = new System.Windows.Forms.Button();
			this.edDebug = new System.Windows.Forms.TextBox();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.grdDisplay = new System.Windows.Forms.PropertyGrid();
			this.tvObjectTree = new System.Windows.Forms.TreeView();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.lbClientCount);
			this.panel1.Controls.Add(this.edClientCount);
			this.panel1.Controls.Add(this.btnInitialise);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.edWeight);
			this.panel1.Controls.Add(this.lbAge);
			this.panel1.Controls.Add(this.lbName);
			this.panel1.Controls.Add(this.edAge);
			this.panel1.Controls.Add(this.edName);
			this.panel1.Controls.Add(this.btnDelete);
			this.panel1.Controls.Add(this.btnAdd);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(576, 64);
			this.panel1.TabIndex = 0;
			//
			// lbClientCount
			// 
			this.lbClientCount.Location = new System.Drawing.Point(216, 10);
			this.lbClientCount.Name = "lbClientCount";
			this.lbClientCount.Size = new System.Drawing.Size(40, 16);
			this.lbClientCount.TabIndex = 21;
			this.lbClientCount.Tag = "";
			this.lbClientCount.Text = "Clients";
			this.lbClientCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// edClientCount
			// 
			this.edClientCount.Location = new System.Drawing.Point(264, 8);
			this.edClientCount.Name = "edClientCount";
			this.edClientCount.ReadOnly = true;
			this.edClientCount.Size = new System.Drawing.Size(64, 20);
			this.edClientCount.TabIndex = 6;
			this.edClientCount.Text = "0";
			// 
			// btnInitialise
			// 
			this.btnInitialise.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnInitialise.Location = new System.Drawing.Point(496, 32);
			this.btnInitialise.Name = "btnInitialise";
			this.btnInitialise.TabIndex = 2;
			this.btnInitialise.Text = "Initialise";
			this.btnInitialise.Click += new System.EventHandler(this.btnInitialise_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(104, 34);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(39, 16);
			this.label1.TabIndex = 18;
			this.label1.Text = "Weight";
			// 
			// edWeight
			// 
			this.edWeight.Location = new System.Drawing.Point(152, 32);
			this.edWeight.Name = "edWeight";
			this.edWeight.Size = new System.Drawing.Size(56, 20);
			this.edWeight.TabIndex = 5;
			this.edWeight.Text = "87";
			// 
			// lbAge
			// 
			this.lbAge.AutoSize = true;
			this.lbAge.Location = new System.Drawing.Point(16, 34);
			this.lbAge.Name = "lbAge";
			this.lbAge.Size = new System.Drawing.Size(24, 16);
			this.lbAge.TabIndex = 13;
			this.lbAge.Text = "Age";
			// 
			// lbName
			// 
			this.lbName.AutoSize = true;
			this.lbName.Location = new System.Drawing.Point(8, 10);
			this.lbName.Name = "lbName";
			this.lbName.Size = new System.Drawing.Size(34, 16);
			this.lbName.TabIndex = 12;
			this.lbName.Text = "Name";
			// 
			// edAge
			// 
			this.edAge.Location = new System.Drawing.Point(48, 32);
			this.edAge.Name = "edAge";
			this.edAge.Size = new System.Drawing.Size(48, 20);
			this.edAge.TabIndex = 4;
			this.edAge.Text = "30";
			// 
			// edName
			// 
			this.edName.Location = new System.Drawing.Point(48, 8);
			this.edName.Name = "edName";
			this.edName.Size = new System.Drawing.Size(160, 20);
			this.edName.TabIndex = 3;
			this.edName.Text = "Ben Laan";
			// 
			// btnDelete
			// 
			this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnDelete.Enabled = false;
			this.btnDelete.Location = new System.Drawing.Point(496, 8);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.TabIndex = 1;
			this.btnDelete.Text = "Delete";
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// btnAdd
			// 
			this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnAdd.Enabled = false;
			this.btnAdd.Location = new System.Drawing.Point(416, 8);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.TabIndex = 0;
			this.btnAdd.Text = "Add";
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// edDebug
			// 
			this.edDebug.BackColor = System.Drawing.Color.Black;
			this.edDebug.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.edDebug.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.edDebug.ForeColor = System.Drawing.Color.FromArgb(((byte)(0)), ((byte)(192)), ((byte)(0)));
			this.edDebug.Location = new System.Drawing.Point(0, 394);
			this.edDebug.Multiline = true;
			this.edDebug.Name = "edDebug";
			this.edDebug.Size = new System.Drawing.Size(576, 384);
			this.edDebug.TabIndex = 17;
			this.edDebug.Text = "";
			// 
			// splitter1
			// 
			this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.splitter1.Location = new System.Drawing.Point(0, 391);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(576, 3);
			this.splitter1.TabIndex = 20;
			this.splitter1.TabStop = false;
			// 
			// grdDisplay
			// 
			this.grdDisplay.CommandsVisibleIfAvailable = true;
			this.grdDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grdDisplay.HelpVisible = false;
			this.grdDisplay.LargeButtons = false;
			this.grdDisplay.LineColor = System.Drawing.SystemColors.ScrollBar;
			this.grdDisplay.Location = new System.Drawing.Point(272, 64);
			this.grdDisplay.Name = "grdDisplay";
			this.grdDisplay.Size = new System.Drawing.Size(304, 327);
			this.grdDisplay.TabIndex = 22;
			this.grdDisplay.Text = "PropertyGrid";
			this.grdDisplay.ToolbarVisible = false;
			this.grdDisplay.ViewBackColor = System.Drawing.SystemColors.Window;
			this.grdDisplay.ViewForeColor = System.Drawing.SystemColors.WindowText;
			// 
			// tvObjectTree
			// 
			this.tvObjectTree.Dock = System.Windows.Forms.DockStyle.Left;
			this.tvObjectTree.ImageIndex = -1;
			this.tvObjectTree.Location = new System.Drawing.Point(0, 64);
			this.tvObjectTree.Name = "tvObjectTree";
			this.tvObjectTree.SelectedImageIndex = -1;
			this.tvObjectTree.Size = new System.Drawing.Size(272, 327);
			this.tvObjectTree.TabIndex = 21;
			this.tvObjectTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvObjectTree_AfterSelect);
			// 
			// frmServer
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(576, 778);
			this.Controls.Add(this.grdDisplay);
			this.Controls.Add(this.tvObjectTree);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.edDebug);
			this.Controls.Add(this.panel1);
			this.Location = new System.Drawing.Point(650, 0);
			this.Name = "frmServer";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Server";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.frmServer_Closing);
			this.Load += new System.EventHandler(this.frmServer_Load);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);
		}
		#endregion

		public frmServer()
		{
			InitializeComponent();
			_debugger = new FormDebugger(this);
			Debug.Listeners.Add(_debugger);
		}

		private PersonClasses.PersonList _persons;
		private GameServer _server;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Button btnInitialise;
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.TextBox edAge;
		private System.Windows.Forms.TextBox edClientCount;
		private System.Windows.Forms.TextBox edDebug;
		private System.Windows.Forms.TextBox edName;
		private System.Windows.Forms.TextBox edWeight;
		private System.Windows.Forms.PropertyGrid grdDisplay;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lbAge;
		private System.Windows.Forms.Label lbClientCount;
		private System.Windows.Forms.Label lbName;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.TreeView tvObjectTree;
		private ObjectTreeViewer viewer;
		FormDebugger _debugger;

		public void Add(string message)
		{
			string sLastLine = edDebug.Lines[edDebug.Lines.Length - 1];
			sLastLine += message;
			edDebug.Lines[edDebug.Lines.Length - 1] = sLastLine;
		}

		public void AddLine(string message)
		{
			if(edDebug != null)
				edDebug.AppendText(message + Environment.NewLine);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		private void EnableButtons()
		{
			btnAdd.Enabled = (_persons != null);
			btnDelete.Enabled = (_persons != null && !_persons.IsEmpty);
			btnInitialise.Enabled = (_persons == null);
		}

		private void OnMessageReceivedEvent(object sender, byte[] data)
		{
			using (BinaryStreamReader reader = new BinaryStreamReader(data))
			{
				int id = reader.ReadInt32();
				((Server)_persons.Find(id)).ProcessCommand(reader);
			}
			UpdateOnMainThread();
		}

		private void OnNewClientConnectionEvent(object sender, ClientList clients)
		{
			edClientCount.Text = clients.Count.ToString();
		}

		private void SelectProperty(string name)
		{
			GridItem root = grdDisplay.SelectedGridItem;
			do {
				root = root.Parent;
			}
			// stop before the top node, which is the property category node
			while (root.Parent.Parent != null);

			GridItem selected = null;
			foreach(GridItem node in root.GridItems)
				if (node.Label == name)
				{
					selected = node;
					break;
				}

			if (selected != null)
				grdDisplay.SelectedGridItem = selected;
		}

		// this method is called within a thread, hence it must call OnUpdate via
		// the Invoke() method to allow the UI to be updated safely
		private void UpdateOnMainThread()
		{
			this.Invoke(new EventHandler(OnUpdate), new object[] {this, new EventArgs()});
		}

		private void OnUpdate(object sender, EventArgs e)
		{
			if(viewer != null && viewer.Object != null)
				viewer.Update();

			EnableButtons();

			this.Invalidate();
		}

		private void btnAdd_Click(object sender, System.EventArgs e)
		{
			System.DateTime duration = System.DateTime.Now;

			PersonClasses.Person p = new PersonClasses.Person(
				edName.Text,
				Int32.Parse(edAge.Text),
				Int32.Parse(edWeight.Text)
			);
			_persons.Add(p);

			TimeSpan t = System.DateTime.Now - duration;
			Debug.WriteLine("Time Taken: " + t);

			UpdateOnMainThread();
		}

		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			if(grdDisplay.SelectedObject is BaseEntity)
				_persons.Remove((BaseEntity)grdDisplay.SelectedObject);

			UpdateOnMainThread();
		}

		private void btnInitialise_Click(object sender, System.EventArgs e)
		{
			_persons = new PersonClasses.PersonList();
			viewer.Object = _persons;
			UpdateOnMainThread();
		}

		private void frmServer_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			_server.Active = false;
			Debug.Listeners.Remove(_debugger);
		}

		private void frmServer_Load(object sender, System.EventArgs e)
		{
			_server = GameServer.Instance;
			_server.OnProcessMessageEvent += new OnProcessMessageEventHandler(OnMessageReceivedEvent);
			_server.OnNewClientConnectionEvent += new OnNewClientConnectionEventHandler(OnNewClientConnectionEvent);

			_server.Active = true;

			viewer = new ObjectTreeViewer(tvObjectTree, null);
		}

		private void tvObjectTree_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			NodeDefinition def = viewer.SelectedObject;

			grdDisplay.SelectedObject = def.Instance;
			if (def.Property != "")
				SelectProperty(def.Property);
		}

		[STAThread]
		static void Main()
		{
			Application.Run(new frmServer());
		}
    }
}