using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;

using log4net;
using log4net.Config;

using Laan.GameLibrary;
using Laan.GameLibrary.Data;
using Laan.GameLibrary.Entity;
using Laan.Library.ObjectTree;
using GameClasses = Laan.Risk.Game.Server;

namespace Laan.Risk.GUI.Server
{
	public class frmServer : System.Windows.Forms.Form
    {
		#region Windows Form Designer generated code

		private void InitializeComponent()
		{
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.btnStart = new System.Windows.Forms.Button();
            this.lbClientCount = new System.Windows.Forms.Label();
            this.edClientCount = new System.Windows.Forms.TextBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.edDebug = new System.Windows.Forms.TextBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel1 = new System.Windows.Forms.Panel();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.grdDisplay = new System.Windows.Forms.PropertyGrid();
            this.tvObjectTree = new System.Windows.Forms.TreeView();
            this.pnlHeader.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.btnStart);
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
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart.Enabled = false;
            this.btnStart.Location = new System.Drawing.Point(343, 8);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 22;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
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
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
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
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
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
            this.edDebug.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.edDebug.Location = new System.Drawing.Point(0, 352);
            this.edDebug.Multiline = true;
            this.edDebug.Name = "edDebug";
            this.edDebug.Size = new System.Drawing.Size(584, 310);
            this.edDebug.TabIndex = 17;
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter1.Location = new System.Drawing.Point(0, 349);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(584, 3);
            this.splitter1.TabIndex = 20;
            this.splitter1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.splitter2);
            this.panel1.Controls.Add(this.grdDisplay);
            this.panel1.Controls.Add(this.tvObjectTree);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 40);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(584, 309);
            this.panel1.TabIndex = 21;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // splitter2
            // 
            this.splitter2.Location = new System.Drawing.Point(330, 0);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(3, 309);
            this.splitter2.TabIndex = 31;
            this.splitter2.TabStop = false;
            // 
            // grdDisplay
            // 
            this.grdDisplay.BackColor = System.Drawing.Color.Silver;
            this.grdDisplay.CommandsBackColor = System.Drawing.Color.Silver;
            this.grdDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdDisplay.HelpVisible = false;
            this.grdDisplay.LineColor = System.Drawing.SystemColors.ScrollBar;
            this.grdDisplay.Location = new System.Drawing.Point(330, 0);
            this.grdDisplay.Name = "grdDisplay";
            this.grdDisplay.Size = new System.Drawing.Size(254, 309);
            this.grdDisplay.TabIndex = 30;
            this.grdDisplay.ToolbarVisible = false;
            // 
            // tvObjectTree
            // 
            this.tvObjectTree.BackColor = System.Drawing.SystemColors.Window;
            this.tvObjectTree.Dock = System.Windows.Forms.DockStyle.Left;
            this.tvObjectTree.ForeColor = System.Drawing.SystemColors.WindowText;
            this.tvObjectTree.FullRowSelect = true;
            this.tvObjectTree.Location = new System.Drawing.Point(0, 0);
            this.tvObjectTree.Name = "tvObjectTree";
            this.tvObjectTree.ShowPlusMinus = false;
            this.tvObjectTree.Size = new System.Drawing.Size(330, 309);
            this.tvObjectTree.TabIndex = 29;
            this.tvObjectTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvObjectTree_AfterSelect);
            // 
            // frmServer
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(584, 662);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.edDebug);
            this.Controls.Add(this.pnlHeader);
            this.Location = new System.Drawing.Point(650, 0);
            this.Name = "frmServer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Server";
            this.Load += new System.EventHandler(this.frmServer_Load);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.frmServer_Closing);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

        static frmServer()
        {
            XmlConfigurator.Configure();
        }

        public frmServer()
		{
			InitializeComponent();
			_debugger = new FormDebugger(this);
            Debug.Listeners.Add(_debugger);
        }

        private int _clientCount;
        protected ILog Log = log4net.LogManager.GetLogger(Assembly.GetEntryAssembly().ManifestModule.Name);

        private GameClasses.Game _game;
		private Laan.GameLibrary.GameServer _server;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button btnDelete;
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.TextBox edClientCount;
        private System.Windows.Forms.TextBox edDebug;
		private System.Windows.Forms.Label lbClientCount;
		private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Splitter splitter1;
        private ObjectTreeViewer _viewer;
        private FormDebugger _debugger;
        private Button btnStart;
        private Panel panel1;
        private PropertyGrid grdDisplay;
        private TreeView tvObjectTree;
        private Splitter splitter2;
		private System.Windows.Forms.Label label1;

        delegate void AddLineHandler(string message);

        internal void Add(string message)
        {
            this.Invoke(new AddLineHandler(OnAdd), new object[] { message });
        }

        internal void AddLine(string message)
        {
            this.Invoke(new AddLineHandler(OnAddLine), new object[] { message });
        }

        private void OnAddLine(string message)
        {
            if (edDebug != null)
                edDebug.AppendText(message + Environment.NewLine);

            this.Invalidate();
        }

        public void OnAdd(string message)
        {
            string sLastLine = edDebug.Lines[edDebug.Lines.Length - 1];
            sLastLine += message;
            edDebug.Lines[edDebug.Lines.Length - 1] = sLastLine;

            this.Invalidate();
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
            btnStart.Enabled = (_game != null) && _game.AllPlayersReady;
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

            _clientCount = clients.Count;
        }

        private void OnClientDisconnectionEvent(object sender, ClientNodeList clients)
        {
            if (clients.Count == 0)
                Finalise();

            _clientCount = clients.Count;
        }

		private void SelectProperty(string name)
		{
            GridItem root = grdDisplay.SelectedGridItem;
            do
            {
                root = root.Parent;
            }
            // stop before the top node, which is the property category node
            while (root.Parent.Parent != null);

            GridItem selected = null;
            foreach (GridItem node in root.GridItems)
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
			this.Invoke(
                new EventHandler(OnRedraw), 
                new object[] { this, new EventArgs() }
            );
		}

		private void OnRedraw(object sender, EventArgs e)
		{
            if (_viewer != null && _viewer.Object != null)
                _viewer.Update();

			EnableButtons();
            edClientCount.Text = _clientCount.ToString();

			this.Invalidate();
		}

		private void btnAdd_Click(object sender, System.EventArgs e)
		{
			System.DateTime duration = System.DateTime.Now;

			Laan.Risk.Player.Server.Player player = new Laan.Risk.Player.Server.Player();
			_game.Players.Add(player);

			TimeSpan t = System.DateTime.Now - duration;
			Log.Debug("Time Taken: " + t);

			Redraw();
		}

		private void btnDelete_Click(object sender, System.EventArgs e)
		{
            if (grdDisplay.SelectedObject is Player.Server.Player)
                _game.Players.Remove((Player.Server.Player)grdDisplay.SelectedObject);

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

		public void OnRendezvousReceivedEvent(object sender, string receivedText, ref bool isValid)
		{
			//Log.Debug("Rendezvous: " + receivedText);
			isValid = (receivedText == Laan.Risk.Constants.RendezvousText);
		}

        private void btnStart_Click(object sender, EventArgs e)
        {
            _game.Start();
            Redraw();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
	}

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
}
