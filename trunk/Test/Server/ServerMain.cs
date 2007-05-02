using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

using Laan.GameLibrary;
using Laan.GameLibrary.Data;
using Laan.GameLibrary.Entity;

using Laan.Library.Logging;
using Laan.Library.ObjectTree;

using GameClasses = Laan.Risk.Game.Server;

namespace Laan.Risk.GUI.Server
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

		private void InitializeComponent()
		{
			this.pnlHeader = new System.Windows.Forms.Panel();
			this.lbClientCount = new System.Windows.Forms.Label();
			this.edClientCount = new System.Windows.Forms.TextBox();
			this.btnDelete = new System.Windows.Forms.Button();
			this.btnAdd = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.edDebug = new System.Windows.Forms.TextBox();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.grdDisplay = new System.Windows.Forms.PropertyGrid();
			this.tvObjectTree = new System.Windows.Forms.TreeView();
			this.pnlHeader.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlHeader
			// 
			this.pnlHeader.Controls.Add(this.lbClientCount);
			this.pnlHeader.Controls.Add(this.edClientCount);
			this.pnlHeader.Controls.Add(this.btnDelete);
			this.pnlHeader.Controls.Add(this.btnAdd);
			this.pnlHeader.Controls.Add(this.label1);
			this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlHeader.Location = new System.Drawing.Point(0, 0);
			this.pnlHeader.Name = "pnlHeader";
			this.pnlHeader.Size = new System.Drawing.Size(584, 40);
			this.pnlHeader.TabIndex = 0;
			// 
			// lbClientCount
			// 
			this.lbClientCount.Location = new System.Drawing.Point(8, 11);
			this.lbClientCount.Name = "lbClientCount";
			this.lbClientCount.Size = new System.Drawing.Size(40, 16);
			this.lbClientCount.TabIndex = 21;
			this.lbClientCount.Tag = "";
			this.lbClientCount.Text = "Clients";
			this.lbClientCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// edClientCount
			// 
			this.edClientCount.Location = new System.Drawing.Point(56, 9);
			this.edClientCount.Name = "edClientCount";
			this.edClientCount.ReadOnly = true;
			this.edClientCount.Size = new System.Drawing.Size(64, 20);
			this.edClientCount.TabIndex = 6;
			this.edClientCount.Text = "0";
			// 
			// btnDelete
			// 
			this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnDelete.Enabled = false;
			this.btnDelete.Location = new System.Drawing.Point(504, 8);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.TabIndex = 1;
			this.btnDelete.Text = "Delete";
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// btnAdd
			// 
			this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnAdd.Enabled = false;
			this.btnAdd.Location = new System.Drawing.Point(424, 8);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.TabIndex = 0;
			this.btnAdd.Text = "Add";
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			//
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 11);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(40, 16);
			this.label1.TabIndex = 21;
			this.label1.Tag = "";
			this.label1.Text = "Clients";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// edDebug
			// 
			this.edDebug.BackColor = System.Drawing.Color.Black;
			this.edDebug.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.edDebug.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.edDebug.ForeColor = System.Drawing.Color.FromArgb(((byte)(0)), ((byte)(192)), ((byte)(0)));
			this.edDebug.Location = new System.Drawing.Point(0, 246);
			this.edDebug.Multiline = true;
			this.edDebug.Name = "edDebug";
			this.edDebug.Size = new System.Drawing.Size(584, 416);
			this.edDebug.TabIndex = 17;
			this.edDebug.Text = "";
			// 
			// splitter1
			// 
			this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.splitter1.Location = new System.Drawing.Point(0, 243);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(584, 3);
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
			this.grdDisplay.Location = new System.Drawing.Point(272, 40);
			this.grdDisplay.Name = "grdDisplay";
			this.grdDisplay.Size = new System.Drawing.Size(312, 203);
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
			this.tvObjectTree.Location = new System.Drawing.Point(0, 40);
			this.tvObjectTree.Name = "tvObjectTree";
			this.tvObjectTree.SelectedImageIndex = -1;
			this.tvObjectTree.Size = new System.Drawing.Size(272, 203);
			this.tvObjectTree.TabIndex = 21;
			this.tvObjectTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvObjectTree_AfterSelect);
			// 
			// frmServer
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(584, 662);
			this.Controls.Add(this.grdDisplay);
			this.Controls.Add(this.tvObjectTree);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.edDebug);
			this.Controls.Add(this.pnlHeader);
			this.Location = new System.Drawing.Point(650, 0);
			this.Name = "frmServer";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Server";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.frmServer_Closing);
			this.Load += new System.EventHandler(this.frmServer_Load);
			this.pnlHeader.ResumeLayout(false);
			this.ResumeLayout(false);
		}
		#endregion

		public frmServer()
		{
			InitializeComponent();
			_debugger = new FormDebugger(this);
			Debug.Listeners.Add(_debugger);
		}

		private GameClasses.Game _game;
		private Laan.GameLibrary.GameServer _server;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button btnDelete;
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.TextBox edClientCount;
		private System.Windows.Forms.TextBox edDebug;
		private System.Windows.Forms.PropertyGrid grdDisplay;
		private System.Windows.Forms.Label lbClientCount;
		private System.Windows.Forms.Panel pnlHeader;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.TreeView tvObjectTree;
		private ObjectTreeViewer _viewer;
		FormDebugger _debugger;
		private System.Windows.Forms.Label label1;

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
			btnAdd.Enabled = (_game != null);
			btnDelete.Enabled = (_game != null && !_game.Players.IsEmpty);
		}

		private void Finalise()
		{
			_game = null;
			_viewer.Object = null;

			_server.Active = false;
			_server.Active = true;

			Redraw();
		}

		private void Initialise()
		{
			_game = new GameClasses.Game();
			_viewer.Object = _game;
			Redraw();
		}

		private void OnMessageReceivedEvent(object sender, ClientMessage message)
		{
			Redraw();
		}

		private void OnNewClientConnectionEvent(object sender, ClientNodeList clients)
		{
			if (clients.Count == 1)
				Initialise();

			edClientCount.Text = clients.Count.ToString();
		}

		private void OnClientDisconnectionEvent(object sender, ClientNodeList clients)
		{
			if (clients.Count == 0)
				Finalise();

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
		private void Redraw()
		{
			this.Invoke(new EventHandler(OnRedraw), new object[] {this, new EventArgs()});
		}

		private void OnRedraw(object sender, EventArgs e)
		{
			if(_viewer != null && _viewer.Object != null)
				_viewer.Update();

			EnableButtons();

			this.Invalidate();
		}

		private void btnAdd_Click(object sender, System.EventArgs e)
		{
			System.DateTime duration = System.DateTime.Now;

			Laan.Risk.Player.Server.Player player = new Laan.Risk.Player.Server.Player();
			_game.Players.Add(player);

			TimeSpan t = System.DateTime.Now - duration;
			Log.WriteLine("Time Taken: " + t);

			Redraw();
		}

		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			if(grdDisplay.SelectedObject is BaseEntity)
				_game.Players.Remove((BaseEntity)grdDisplay.SelectedObject);

			Redraw();
		}

		private void btnInitialise_Click(object sender, System.EventArgs e)
		{
			Initialise();
		}

		private void frmServer_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			_server.Active = false;
			_server.EndRendezvous();
			Debug.Listeners.Remove(_debugger);
		}

		private void frmServer_Load(object sender, System.EventArgs e)
		{
			_server = Laan.GameLibrary.GameServer.Instance;
			_server.Name = "Riskier";
			_server.OnRendezvousReceivedEvent += new OnRendezvousReceivedEventHandler(OnRendezvousReceivedEvent);
			_server.OnProcessMessageEvent += new OnProcessMessageEventHandler(OnMessageReceivedEvent);
			_server.OnNewClientConnectionEvent += new OnNewClientConnectionEventHandler(OnNewClientConnectionEvent);
			_server.OnClientDisconnectionEvent += new OnClientDisconnectionEventHandler(OnClientDisconnectionEvent);

			_server.Active = true;

			_viewer = new ObjectTreeViewer(tvObjectTree, null);

			_server.BeginRendezvous();
		}

		private void tvObjectTree_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			NodeDefinition def = _viewer.SelectedObject;

			grdDisplay.SelectedObject = def.Instance;
			if (def.Property != "")
				SelectProperty(def.Property);
		}

		[STAThread]
		static void Main()
		{
			Application.Run(new frmServer());
		}

		public void OnRendezvousReceivedEvent(object sender, string receivedText, ref bool isValid)
		{
			Log.WriteLine("Rendezvous: " + receivedText);
			isValid = (receivedText == Laan.Risk.Constants.RendezvousText);
		}
	}
}
