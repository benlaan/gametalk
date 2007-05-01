using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Diagnostics;

using Laan.GameLibrary;
using Laan.GameLibrary.Entity;
using GameClasses = Laan.Risk.Game.Client;
using Laan.Library.ObjectTree;
using Laan.Library.Logging;


namespace Laan.Risk.GUI.Client
{

    public class FormDebugger : DefaultTraceListener
    {
		private frmClient _form;

		public FormDebugger(frmClient form)
		{
            _form = form;
		}
		public override void Write(string data)
		{
            _form.Add(data);
		}

        public override void WriteLine(string data)
        {
            _form.AddLine(data);
        }
    }

	public class frmClient : System.Windows.Forms.Form
	{
		#region Windows Form Designer generated code
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmClient));
			this.edDebug = new System.Windows.Forms.TextBox();
			this.pcStage = new System.Windows.Forms.TabControl();
			this.tabFind = new System.Windows.Forms.TabPage();
			this.gbPlayerDetails = new System.Windows.Forms.GroupBox();
			this.btnJoinServer = new System.Windows.Forms.Button();
			this.lbGameName = new System.Windows.Forms.Label();
			this.edName = new System.Windows.Forms.TextBox();
			this.lvAvailableGames = new System.Windows.Forms.ListView();
			this.colName = new System.Windows.Forms.ColumnHeader();
			this.colHost = new System.Windows.Forms.ColumnHeader();
			this.colPort = new System.Windows.Forms.ColumnHeader();
			this.btnFindServers = new System.Windows.Forms.Button();
			this.tabJoin = new System.Windows.Forms.TabPage();
			this.lvPlayers = new System.Windows.Forms.ListView();
			this.colColour = new System.Windows.Forms.ColumnHeader();
			this.colNation = new System.Windows.Forms.ColumnHeader();
			this.colShortName = new System.Windows.Forms.ColumnHeader();
			this.colReady = new System.Windows.Forms.ColumnHeader();
			this.btnReady = new System.Windows.Forms.Button();
			this.btnJoinGame = new System.Windows.Forms.Button();
			this.gbNationDetails = new System.Windows.Forms.GroupBox();
			this.lbShortName = new System.Windows.Forms.Label();
			this.edNationShortName = new System.Windows.Forms.TextBox();
			this.btnColour = new System.Windows.Forms.Button();
			this.lbColour = new System.Windows.Forms.Label();
			this.lbNationName = new System.Windows.Forms.Label();
			this.edNationName = new System.Windows.Forms.TextBox();
			this.tabGame = new System.Windows.Forms.TabPage();
			this.grdDisplay = new System.Windows.Forms.PropertyGrid();
			this.tvObjectTree = new System.Windows.Forms.TreeView();
			this.panel1 = new System.Windows.Forms.Panel();
			this.btnGetOlder = new System.Windows.Forms.Button();
			this.btnGetFatter = new System.Windows.Forms.Button();
			this.dlgNationColour = new System.Windows.Forms.ColorDialog();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.pcStage.SuspendLayout();
			this.tabFind.SuspendLayout();
			this.gbPlayerDetails.SuspendLayout();
			this.tabJoin.SuspendLayout();
			this.gbNationDetails.SuspendLayout();
			this.tabGame.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// edDebug
			// 
			this.edDebug.BackColor = System.Drawing.Color.Black;
			this.edDebug.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.edDebug.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.edDebug.ForeColor = System.Drawing.Color.FromArgb(((byte)(0)), ((byte)(192)), ((byte)(0)));
			this.edDebug.Location = new System.Drawing.Point(0, 374);
			this.edDebug.Multiline = true;
			this.edDebug.Name = "edDebug";
			this.edDebug.Size = new System.Drawing.Size(504, 296);
			this.edDebug.TabIndex = 28;
			this.edDebug.Text = "";
			// 
			// pcStage
			// 
			this.pcStage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
						| System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.pcStage.Controls.Add(this.tabFind);
			this.pcStage.Controls.Add(this.tabJoin);
			this.pcStage.Controls.Add(this.tabGame);
			this.pcStage.Location = new System.Drawing.Point(0, -21);
			this.pcStage.Name = "pcStage";
			this.pcStage.SelectedIndex = 0;
			this.pcStage.Size = new System.Drawing.Size(504, 397);
			this.pcStage.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
			this.pcStage.TabIndex = 29;
			// 
			// tabFind
			// 
			this.tabFind.BackColor = System.Drawing.SystemColors.ControlDark;
			this.tabFind.Controls.Add(this.gbPlayerDetails);
			this.tabFind.Controls.Add(this.lvAvailableGames);
			this.tabFind.Controls.Add(this.btnFindServers);
			this.tabFind.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
			this.tabFind.Location = new System.Drawing.Point(4, 22);
			this.tabFind.Name = "tabFind";
			this.tabFind.Size = new System.Drawing.Size(496, 371);
			this.tabFind.TabIndex = 2;
			this.tabFind.Text = "Find";
			// 
			// gbPlayerDetails
			// 
			this.gbPlayerDetails.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.gbPlayerDetails.BackColor = System.Drawing.Color.Transparent;
			this.gbPlayerDetails.Controls.Add(this.btnJoinServer);
			this.gbPlayerDetails.Controls.Add(this.lbGameName);
			this.gbPlayerDetails.Controls.Add(this.edName);
			this.gbPlayerDetails.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
			this.gbPlayerDetails.ForeColor = System.Drawing.Color.White;
			this.gbPlayerDetails.Location = new System.Drawing.Point(8, 303);
			this.gbPlayerDetails.Name = "gbPlayerDetails";
			this.gbPlayerDetails.Size = new System.Drawing.Size(480, 56);
			this.gbPlayerDetails.TabIndex = 31;
			this.gbPlayerDetails.TabStop = false;
			this.gbPlayerDetails.Tag = "";
			this.gbPlayerDetails.Text = "Player Details";
			// 
			// btnJoinServer
			// 
			this.btnJoinServer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnJoinServer.BackColor = System.Drawing.Color.Transparent;
			this.btnJoinServer.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnJoinServer.BackgroundImage")));
			this.btnJoinServer.Enabled = false;
			this.btnJoinServer.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnJoinServer.ForeColor = System.Drawing.Color.White;
			this.btnJoinServer.Location = new System.Drawing.Point(336, 22);
			this.btnJoinServer.Name = "btnJoinServer";
			this.btnJoinServer.Size = new System.Drawing.Size(120, 23);
			this.btnJoinServer.TabIndex = 25;
			this.btnJoinServer.Text = "&Join Server";
			this.btnJoinServer.Click += new System.EventHandler(this.btnJoinServer_Click);
			// 
			// lbGameName
			// 
			this.lbGameName.AutoSize = true;
			this.lbGameName.Location = new System.Drawing.Point(16, 24);
			this.lbGameName.Name = "lbGameName";
			this.lbGameName.Size = new System.Drawing.Size(37, 17);
			this.lbGameName.TabIndex = 24;
			this.lbGameName.Text = "Name";
			this.lbGameName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// edName
			// 
			this.edName.BackColor = System.Drawing.Color.Black;
			this.edName.ForeColor = System.Drawing.Color.White;
			this.edName.Location = new System.Drawing.Point(64, 22);
			this.edName.Name = "edName";
			this.edName.Size = new System.Drawing.Size(233, 21);
			this.edName.TabIndex = 23;
			this.edName.Text = "Praetorian";
			// 
			// lvAvailableGames
			// 
			this.lvAvailableGames.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
						| System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lvAvailableGames.BackColor = System.Drawing.SystemColors.Control;
			this.lvAvailableGames.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
						this.colName,
						this.colHost,
						this.colPort});
			this.lvAvailableGames.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
			this.lvAvailableGames.FullRowSelect = true;
			this.lvAvailableGames.Location = new System.Drawing.Point(8, 48);
			this.lvAvailableGames.Name = "lvAvailableGames";
			this.lvAvailableGames.Size = new System.Drawing.Size(480, 239);
			this.lvAvailableGames.TabIndex = 30;
			this.lvAvailableGames.View = System.Windows.Forms.View.Details;
			this.lvAvailableGames.SelectedIndexChanged += new System.EventHandler(this.lvAvailableGames_SelectedIndexChanged);
			// 
			// colName
			// 
			this.colName.Text = "Name";
			this.colName.Width = 250;
			// 
			// colHost
			// 
			this.colHost.Text = "Host";
			this.colHost.Width = 100;
			// 
			// colPort
			// 
			this.colPort.Text = "Port";
			// 
			// btnFindServers
			// 
			this.btnFindServers.AccessibleDescription = "";
			this.btnFindServers.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnFindServers.BackgroundImage")));
			this.btnFindServers.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnFindServers.ForeColor = System.Drawing.Color.White;
			this.btnFindServers.Location = new System.Drawing.Point(16, 15);
			this.btnFindServers.Name = "btnFindServers";
			this.btnFindServers.Size = new System.Drawing.Size(120, 23);
			this.btnFindServers.TabIndex = 12;
			this.btnFindServers.Text = "&Find Servers";
			this.btnFindServers.Click += new System.EventHandler(this.btnFind_Click);
			// 
			// tabJoin
			// 
			this.tabJoin.BackColor = System.Drawing.SystemColors.ControlDark;
			this.tabJoin.Controls.Add(this.lvPlayers);
			this.tabJoin.Controls.Add(this.btnReady);
			this.tabJoin.Controls.Add(this.btnJoinGame);
			this.tabJoin.Controls.Add(this.gbNationDetails);
			this.tabJoin.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tabJoin.ForeColor = System.Drawing.Color.White;
			this.tabJoin.Location = new System.Drawing.Point(4, 22);
			this.tabJoin.Name = "tabJoin";
			this.tabJoin.Size = new System.Drawing.Size(496, 356);
			this.tabJoin.TabIndex = 0;
			this.tabJoin.Text = "Join";
			// 
			// lvPlayers
			// 
			this.lvPlayers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
						| System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lvPlayers.BackColor = System.Drawing.SystemColors.Control;
			this.lvPlayers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
						this.colColour,
						this.colNation,
						this.colShortName,
						this.colReady});
			this.lvPlayers.FullRowSelect = true;
			this.lvPlayers.Location = new System.Drawing.Point(8, 120);
			this.lvPlayers.Name = "lvPlayers";
			this.lvPlayers.Size = new System.Drawing.Size(480, 168);
			this.lvPlayers.TabIndex = 29;
			this.lvPlayers.View = System.Windows.Forms.View.Details;
			// 
			// colColour
			// 
			this.colColour.Text = "Colour";
			this.colColour.Width = 50;
			// 
			// colNation
			// 
			this.colNation.Text = "Nation";
			this.colNation.Width = 326;
			// 
			// colShortName
			// 
			this.colShortName.Text = "Short";
			this.colShortName.Width = 50;
			// 
			// colReady
			// 
			this.colReady.Text = "Ready";
			this.colReady.Width = 50;
			// 
			// btnReady
			// 
			this.btnReady.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnReady.BackColor = System.Drawing.Color.Transparent;
			this.btnReady.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnReady.BackgroundImage")));
			this.btnReady.Enabled = false;
			this.btnReady.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnReady.ForeColor = System.Drawing.Color.White;
			this.btnReady.Location = new System.Drawing.Point(368, 299);
			this.btnReady.Name = "btnReady";
			this.btnReady.Size = new System.Drawing.Size(120, 23);
			this.btnReady.TabIndex = 28;
			this.btnReady.Text = "Ready";
			this.btnReady.Click += new System.EventHandler(this.btnReady_Click);
			// 
			// btnJoinGame
			// 
			this.btnJoinGame.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnJoinGame.BackColor = System.Drawing.Color.Transparent;
			this.btnJoinGame.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnJoinGame.BackgroundImage")));
			this.btnJoinGame.Enabled = false;
			this.btnJoinGame.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnJoinGame.ForeColor = System.Drawing.Color.White;
			this.btnJoinGame.Location = new System.Drawing.Point(365, 88);
			this.btnJoinGame.Name = "btnJoinGame";
			this.btnJoinGame.Size = new System.Drawing.Size(120, 23);
			this.btnJoinGame.TabIndex = 27;
			this.btnJoinGame.Text = "Join";
			this.btnJoinGame.Click += new System.EventHandler(this.btnJoin_Click);
			// 
			// gbNationDetails
			// 
			this.gbNationDetails.BackColor = System.Drawing.Color.Transparent;
			this.gbNationDetails.Controls.Add(this.lbShortName);
			this.gbNationDetails.Controls.Add(this.edNationShortName);
			this.gbNationDetails.Controls.Add(this.btnColour);
			this.gbNationDetails.Controls.Add(this.lbColour);
			this.gbNationDetails.Controls.Add(this.lbNationName);
			this.gbNationDetails.Controls.Add(this.edNationName);
			this.gbNationDetails.Enabled = false;
			this.gbNationDetails.Location = new System.Drawing.Point(8, 16);
			this.gbNationDetails.Name = "gbNationDetails";
			this.gbNationDetails.Size = new System.Drawing.Size(344, 99);
			this.gbNationDetails.TabIndex = 25;
			this.gbNationDetails.TabStop = false;
			this.gbNationDetails.Text = "Nation Details";
			// 
			// lbShortName
			// 
			this.lbShortName.AutoSize = true;
			this.lbShortName.Location = new System.Drawing.Point(28, 45);
			this.lbShortName.Name = "lbShortName";
			this.lbShortName.Size = new System.Drawing.Size(72, 17);
			this.lbShortName.TabIndex = 30;
			this.lbShortName.Text = "Short Name";
			this.lbShortName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// edNationShortName
			// 
			this.edNationShortName.BackColor = System.Drawing.Color.Black;
			this.edNationShortName.ForeColor = System.Drawing.Color.White;
			this.edNationShortName.Location = new System.Drawing.Point(104, 40);
			this.edNationShortName.MaxLength = 5;
			this.edNationShortName.Name = "edNationShortName";
			this.edNationShortName.Size = new System.Drawing.Size(48, 21);
			this.edNationShortName.TabIndex = 29;
			this.edNationShortName.Text = "R. Aus";
			// 
			// btnColour
			// 
			this.btnColour.BackColor = System.Drawing.SystemColors.Control;
			this.btnColour.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnColour.Location = new System.Drawing.Point(104, 65);
			this.btnColour.Name = "btnColour";
			this.btnColour.Size = new System.Drawing.Size(80, 24);
			this.btnColour.TabIndex = 28;
			this.btnColour.Click += new System.EventHandler(this.btnColour_Click);
			// 
			// lbColour
			// 
			this.lbColour.AutoSize = true;
			this.lbColour.Location = new System.Drawing.Point(56, 72);
			this.lbColour.Name = "lbColour";
			this.lbColour.Size = new System.Drawing.Size(41, 17);
			this.lbColour.TabIndex = 27;
			this.lbColour.Text = "Colour";
			this.lbColour.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lbNationName
			// 
			this.lbNationName.AutoSize = true;
			this.lbNationName.Location = new System.Drawing.Point(58, 18);
			this.lbNationName.Name = "lbNationName";
			this.lbNationName.Size = new System.Drawing.Size(37, 17);
			this.lbNationName.TabIndex = 26;
			this.lbNationName.Text = "Name";
			this.lbNationName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// edNationName
			// 
			this.edNationName.BackColor = System.Drawing.Color.Black;
			this.edNationName.ForeColor = System.Drawing.Color.White;
			this.edNationName.Location = new System.Drawing.Point(104, 16);
			this.edNationName.MaxLength = 50;
			this.edNationName.Name = "edNationName";
			this.edNationName.Size = new System.Drawing.Size(233, 21);
			this.edNationName.TabIndex = 25;
			this.edNationName.Text = "Republica Australis";
			// 
			// tabGame
			// 
			this.tabGame.Controls.Add(this.grdDisplay);
			this.tabGame.Controls.Add(this.tvObjectTree);
			this.tabGame.Controls.Add(this.panel1);
			this.tabGame.Location = new System.Drawing.Point(4, 22);
			this.tabGame.Name = "tabGame";
			this.tabGame.Size = new System.Drawing.Size(496, 356);
			this.tabGame.TabIndex = 1;
			this.tabGame.Text = "Game";
			this.tabGame.Visible = false;
			// 
			// grdDisplay
			// 
			this.grdDisplay.BackColor = System.Drawing.Color.Silver;
			this.grdDisplay.CommandsBackColor = System.Drawing.Color.Silver;
			this.grdDisplay.CommandsVisibleIfAvailable = true;
			this.grdDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grdDisplay.HelpVisible = false;
			this.grdDisplay.LargeButtons = false;
			this.grdDisplay.LineColor = System.Drawing.SystemColors.ScrollBar;
			this.grdDisplay.Location = new System.Drawing.Point(240, 40);
			this.grdDisplay.Name = "grdDisplay";
			this.grdDisplay.Size = new System.Drawing.Size(256, 316);
			this.grdDisplay.TabIndex = 28;
			this.grdDisplay.Text = "PropertyGrid";
			this.grdDisplay.ToolbarVisible = false;
			this.grdDisplay.ViewBackColor = System.Drawing.SystemColors.Window;
			this.grdDisplay.ViewForeColor = System.Drawing.SystemColors.WindowText;
			// 
			// tvObjectTree
			// 
			this.tvObjectTree.BackColor = System.Drawing.SystemColors.Window;
			this.tvObjectTree.Dock = System.Windows.Forms.DockStyle.Left;
			this.tvObjectTree.ForeColor = System.Drawing.SystemColors.WindowText;
			this.tvObjectTree.FullRowSelect = true;
			this.tvObjectTree.ImageIndex = -1;
			this.tvObjectTree.Location = new System.Drawing.Point(0, 40);
			this.tvObjectTree.Name = "tvObjectTree";
			this.tvObjectTree.SelectedImageIndex = -1;
			this.tvObjectTree.ShowPlusMinus = false;
			this.tvObjectTree.Size = new System.Drawing.Size(240, 316);
			this.tvObjectTree.TabIndex = 27;
			this.tvObjectTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvObjectTree_AfterSelect);
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
			this.panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel1.BackgroundImage")));
			this.panel1.Controls.Add(this.btnGetOlder);
			this.panel1.Controls.Add(this.btnGetFatter);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(496, 40);
			this.panel1.TabIndex = 25;
			// 
			// btnGetOlder
			// 
			this.btnGetOlder.AccessibleDescription = "";
			this.btnGetOlder.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnGetOlder.BackgroundImage")));
			this.btnGetOlder.Enabled = false;
			this.btnGetOlder.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnGetOlder.ForeColor = System.Drawing.Color.White;
			this.btnGetOlder.Location = new System.Drawing.Point(136, 8);
			this.btnGetOlder.Name = "btnGetOlder";
			this.btnGetOlder.Size = new System.Drawing.Size(120, 23);
			this.btnGetOlder.TabIndex = 11;
			this.btnGetOlder.Text = "Get Older";
			// 
			// btnGetFatter
			// 
			this.btnGetFatter.AccessibleDescription = "";
			this.btnGetFatter.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnGetFatter.BackgroundImage")));
			this.btnGetFatter.Enabled = false;
			this.btnGetFatter.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnGetFatter.ForeColor = System.Drawing.Color.White;
			this.btnGetFatter.Location = new System.Drawing.Point(8, 8);
			this.btnGetFatter.Name = "btnGetFatter";
			this.btnGetFatter.Size = new System.Drawing.Size(120, 23);
			this.btnGetFatter.TabIndex = 10;
			this.btnGetFatter.Text = "Get Fatter";
			// 
			// splitter1
			// 
			this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.splitter1.Location = new System.Drawing.Point(0, 371);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(504, 3);
			this.splitter1.TabIndex = 30;
			this.splitter1.TabStop = false;
			// 
			// frmClient
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.SystemColors.ControlDark;
			this.ClientSize = new System.Drawing.Size(504, 670);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.pcStage);
			this.Controls.Add(this.edDebug);
			this.KeyPreview = true;
			this.Location = new System.Drawing.Point(150, 0);
			this.MinimumSize = new System.Drawing.Size(512, 592);
			this.Name = "frmClient";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Client";
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmClient_KeyDown);
			this.Closing += new System.ComponentModel.CancelEventHandler(this.frmClient_Closing);
			this.Load += new System.EventHandler(this.frmClient_Load);
			this.pcStage.ResumeLayout(false);
			this.tabFind.ResumeLayout(false);
			this.gbPlayerDetails.ResumeLayout(false);
			this.tabJoin.ResumeLayout(false);
			this.gbNationDetails.ResumeLayout(false);
			this.tabGame.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);
		}
        #endregion

        [STAThread]
        static void Main() 
        {
            Application.Run(new frmClient());
		}

		private System.ComponentModel.Container components = null;

		private GameClient _client;

		private ClientDataStore _dataStore;

		private GameClasses.Game _game;

		private ObjectTreeViewer viewer;
		private System.Windows.Forms.TabControl pcStage;
		private System.Windows.Forms.TabPage tabJoin;
		private System.Windows.Forms.TabPage tabGame;
		private System.Windows.Forms.PropertyGrid grdDisplay;
		private System.Windows.Forms.TreeView tvObjectTree;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button btnGetOlder;
		private System.Windows.Forms.Button btnGetFatter;
		private System.Windows.Forms.TextBox edDebug;
		private System.Windows.Forms.ColorDialog dlgNationColour;
		private System.Windows.Forms.GroupBox gbNationDetails;
		private System.Windows.Forms.Button btnColour;
		private System.Windows.Forms.Label lbColour;
		private System.Windows.Forms.Label lbNationName;
		private System.Windows.Forms.TextBox edNationName;
		private System.Windows.Forms.GroupBox gbPlayerDetails;
		private System.Windows.Forms.Label lbGameName;
		private System.Windows.Forms.TextBox edName;
		private System.Windows.Forms.Button btnJoinServer;
		private System.Windows.Forms.Button btnJoinGame;
		private System.Windows.Forms.Label lbShortName;
		private System.Windows.Forms.TextBox edNationShortName;
		private System.Windows.Forms.Button btnReady;
		private System.Windows.Forms.ListView lvPlayers;
		private System.Windows.Forms.ColumnHeader colNation;
		private System.Windows.Forms.ColumnHeader colShortName;
		private System.Windows.Forms.ColumnHeader colColour;
		private System.Windows.Forms.ColumnHeader colReady;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.TabPage tabFind;
		private System.Windows.Forms.Button btnFindServers;
		private System.Windows.Forms.ListView lvAvailableGames;
		private System.Windows.Forms.ColumnHeader colName;
		private System.Windows.Forms.ColumnHeader colHost;
		private System.Windows.Forms.ColumnHeader colPort;


		internal void AddLine(string message)
		{
			edDebug.AppendText(message + Environment.NewLine);
		}

		internal void Add(string message)
		{
			string sLastLine = edDebug.Lines[edDebug.Lines.Length - 1];
			sLastLine += message;
			edDebug.Lines[edDebug.Lines.Length - 1] = sLastLine;
		}

		public frmClient()
		{
			InitializeComponent();

			Debug.Listeners.Add(new FormDebugger(this));

            Application.Idle += new EventHandler(Application_OnIdle);

			_client = GameClient.Instance;
			_client.OnProcessMessageEvent += new OnProcessMessageEventHandler(OnProcessMessageEvent);
			_client.OnBroadcastFoundEvent += new OnBroadcastFoundEventHandler(OnBroadcastFoundEvent);

			_dataStore = ClientDataStore.Instance;
			_dataStore.AssemblyName = "Riskier";
			_dataStore.OnNewEntityEvent += new OnNewEntityEventHandler(OnNewEntityEvent);
			_dataStore.OnRootEntityEvent += new OnRootEntityEventHandler(OnRootEntityEvent);
			_dataStore.OnModifyEntityEvent += new OnModifyEntityEventHandler(OnModifyEntityEvent);
			_availableGames = new Laan.GameLibrary.AvailableGameList();
		}

		private AvailableGameList _availableGames;
		private int      	      _playerID       = 0;
		private bool     	  	  _update         = false;

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

		/// <summary>
		/// this method is called within a thread, hence it must call OnUpdateStatus via
		/// the Invoke() method to allow the UI to be updated safely
		/// </summary>
		private void UpdateStatus()
		{
			this.Invoke(new EventHandler(OnUpdateStatus), new object[] {this, new EventArgs()});
		}

		private void OnUpdateStatus(object sender, EventArgs e)
		{
			if (_update)
			{
				// Redraw debug ObjectTreeList
				if((viewer != null) && (viewer.Object != null))
					viewer.Update();
	
				// Refresh ListViews
				UpdatePlayerList();
				UpdateGameList();
	
				// Button Enable Status
				btnFindServers.Enabled = !GameClient.Instance.Finding;
				btnReady.Enabled       = (_playerID != 0);
				btnJoinServer.Enabled  = (lvAvailableGames.SelectedIndices.Count > 0);
	
				_update = false;
				this.Invalidate();
			}
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

		private void OnBroadcastFoundEvent(object sender, string name, string host, int port)
		{
			_update = _availableGames.Add(new Laan.GameLibrary.AvailableGame(name, host, port));
		}

		private void OnRootEntityEvent(BaseEntity rootEntity)
		{
			Log.WriteLine("OnRootEntityEvent({0})", rootEntity);

			_game = (GameClasses.Game)rootEntity;
			viewer.Object = _game;
			_update = true;
		}

		private void OnNewEntityEvent(BaseEntity instance)
		{
			Log.WriteLine(String.Format("OnNewEntityEvent({0})", instance));
			_update = true;
		}

		private void OnModifyEntityEvent(BaseEntity e)
		{
			Log.WriteLine(String.Format("OnModifyEntityEvent({0})", e.ID));
			_update = true;
		}

		private void OnProcessMessageEvent(object sender, ClientMessage message)
		{
			_dataStore.ProcessMessage(message.Data);
			_update = true;
		}

		private void UpdateGameList()
		{
			int selected = -1;
			if (lvAvailableGames.SelectedIndices.Count > 0)
				selected = lvAvailableGames.SelectedIndices[0];

			lvAvailableGames.Items.Clear();
            foreach(Laan.GameLibrary.AvailableGame game in _availableGames)
            {
                lvAvailableGames.Items.Add(
                    new ListViewItem(
                        new string[]
                        {
                            game.Name,
                            game.Host,
                            game.Port.ToString()
                        }
                    )
                );
			}

			if (selected >= 0)
				lvAvailableGames.Items[selected].Selected = true;
				
            lvAvailableGames.Invalidate();
        }

		private void UpdatePlayerList()
        {
            if(_game != null && _game.Players != null)
			{
				int selected = -1;
				if (lvPlayers.SelectedIndices.Count > 0)
					selected = lvPlayers.SelectedIndices[0];

				lvPlayers.Items.Clear();

				foreach(Laan.Risk.Player.Client.Player player in _game.Players)
				{
					lvPlayers.Items.Add(new ListViewItem(
						new string[]
						{
							Color.FromArgb(player.Colour).ToKnownColor().ToString(),
							player.Nation.Name.ToString(),
                            player.Nation.ShortName.ToString(),
							(player.Ready ? "Y" : "N")
						}
					));
				}
				if (selected >= 0)
					lvPlayers.Items[selected].Selected = true;

				lvPlayers.Invalidate();
            }
		}

		private Laan.GameLibrary.AvailableGame SelectedGame
		{
			get {
				if (lvAvailableGames.SelectedIndices.Count == 1)
					return _availableGames[lvAvailableGames.SelectedIndices[0]];
				else
					return null;
			}
		}

		private void frmClient_Load(object sender, System.EventArgs e)
		{
			viewer = new ObjectTreeViewer(tvObjectTree, null);
			edName.Text = Config.UserName;
		}

		private void frmClient_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			_client.Disconnect();
			_update = true;
		}

		private void frmClient_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			_update = (e.KeyCode == Keys.F5);
		}

		private void tvObjectTree_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			NodeDefinition def = viewer.SelectedObject;

			grdDisplay.SelectedObject = def.Instance;
			if (def.Property != "")
				SelectProperty(def.Property);
			_update = true;
		}

		private void lvAvailableGames_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			_update = true;
		}

		private void btnJoinServer_Click(object sender, System.EventArgs e)
		{
			try
			{
				Laan.GameLibrary.AvailableGame _game = SelectedGame;
				if (_game != null)
				{
					_client.Connect(_game.Host, _game.Port, edName.Text);

					btnJoinGame.Enabled = true;
					gbNationDetails.Enabled = true;
					pcStage.SelectedTab = tabJoin;
					_update = true;
				}
			}
			catch (Exception ex)
			{
				Log.WriteLine(ex.ToString());
				throw;
			}
		}

		private void btnColour_Click(object sender, System.EventArgs e)
		{
			if (dlgNationColour.ShowDialog() == DialogResult.OK)
				btnColour.BackColor = dlgNationColour.Color;
		}

		private void btnJoin_Click(object sender, System.EventArgs e)
		{
			_playerID = _game.AddPlayer
			(
				edNationName.Text,
				edNationShortName.Text,
                edName.Text,
				btnColour.BackColor.ToArgb()
			);

			// cancel server searching, not required any further
			GameClient.Instance.StopRendezvous();
		}

		private void btnReady_Click(object sender, System.EventArgs e)
		{
			Player.Client.Player player = (Player.Client.Player)ClientDataStore.Instance.Find(_playerID);
			player.Ready = !player.Ready;
			_update = true;
		}

		private void btnFind_Click(object sender, System.EventArgs e)
		{
			GameClient.Instance.StartRendezvous(Laan.Risk.Constants.RendezvousText);
		}

		private void Application_OnIdle(object sender, EventArgs e)
		{
			UpdateStatus();
		}

	}

}

