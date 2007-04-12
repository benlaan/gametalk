using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Diagnostics;
using Laan.GameLibrary;
using Laan.GameLibrary.Entity;
using PersonClasses = Laan.Test.Business.Person.Client;
using Laan.Library.ObjectTree;
using System.Runtime.Remoting;

namespace PersonViewerClient
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
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.panel1 = new System.Windows.Forms.Panel();
			this.btnGetOlder = new System.Windows.Forms.Button();
			this.btnGetFatter = new System.Windows.Forms.Button();
			this.btnConnectToServer = new System.Windows.Forms.Button();
			this.edDebug = new System.Windows.Forms.TextBox();
			this.grdDisplay = new System.Windows.Forms.PropertyGrid();
			this.tvObjectTree = new System.Windows.Forms.TreeView();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			//
			this.panel1.Controls.Add(this.btnGetOlder);
			this.panel1.Controls.Add(this.btnGetFatter);
			this.panel1.Controls.Add(this.btnConnectToServer);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(504, 40);
			this.panel1.TabIndex = 10;
			// 
			// btnGetOlder
			// 
			this.btnGetOlder.Location = new System.Drawing.Point(120, 8);
			this.btnGetOlder.Name = "btnGetOlder";
			this.btnGetOlder.Size = new System.Drawing.Size(104, 23);
			this.btnGetOlder.TabIndex = 11;
			this.btnGetOlder.Text = "Get Older";
			this.btnGetOlder.Click += new System.EventHandler(this.btnGetOlder_Click);
			// 
			// btnGetFatter
			// 
			this.btnGetFatter.Location = new System.Drawing.Point(8, 8);
			this.btnGetFatter.Name = "btnGetFatter";
			this.btnGetFatter.Size = new System.Drawing.Size(104, 23);
			this.btnGetFatter.TabIndex = 10;
			this.btnGetFatter.Text = "Get Fatter";
			this.btnGetFatter.Click += new System.EventHandler(this.btnGetFatter_Click);
			// 
			// btnConnectToServer
			// 
			this.btnConnectToServer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnConnectToServer.Location = new System.Drawing.Point(374, 8);
			this.btnConnectToServer.Name = "btnConnectToServer";
			this.btnConnectToServer.Size = new System.Drawing.Size(120, 23);
			this.btnConnectToServer.TabIndex = 9;
			this.btnConnectToServer.Text = "Connect To Server..";
			this.btnConnectToServer.Click += new System.EventHandler(this.btnConnectToServer_Click);
			// 
			// edDebug
			// 
			this.edDebug.BackColor = System.Drawing.Color.Black;
			this.edDebug.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.edDebug.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.edDebug.ForeColor = System.Drawing.Color.FromArgb(((byte)(0)), ((byte)(192)), ((byte)(0)));
			this.edDebug.Location = new System.Drawing.Point(0, 333);
			this.edDebug.Multiline = true;
			this.edDebug.Name = "edDebug";
			this.edDebug.Size = new System.Drawing.Size(504, 240);
			this.edDebug.TabIndex = 12;
			this.edDebug.Text = "";
			// 
			// grdDisplay
			// 
			this.grdDisplay.CommandsVisibleIfAvailable = true;
			this.grdDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grdDisplay.HelpVisible = false;
			this.grdDisplay.LargeButtons = false;
			this.grdDisplay.LineColor = System.Drawing.SystemColors.ScrollBar;
			this.grdDisplay.Location = new System.Drawing.Point(240, 40);
			this.grdDisplay.Name = "grdDisplay";
			this.grdDisplay.Size = new System.Drawing.Size(264, 293);
			this.grdDisplay.TabIndex = 24;
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
			this.tvObjectTree.Size = new System.Drawing.Size(240, 293);
			this.tvObjectTree.TabIndex = 23;
			this.tvObjectTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvObjectTree_AfterSelect);
			// 
			// frmClient
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(504, 573);
			this.Controls.Add(this.grdDisplay);
			this.Controls.Add(this.tvObjectTree);
			this.Controls.Add(this.edDebug);
			this.Controls.Add(this.panel1);
			this.Location = new System.Drawing.Point(150, 0);
			this.Name = "frmClient";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Client";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.WinForm_Closing);
			this.Load += new System.EventHandler(this.frmClient_Load);
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
		private System.Windows.Forms.Button btnConnectToServer;

		private GameClient _client;
		private System.Windows.Forms.TextBox edDebug;
		private System.Windows.Forms.Panel panel1;

		private ClientDataStore _dataStore;

		private PersonClasses.PersonList _persons;
		private System.Windows.Forms.Button btnGetFatter;
		private System.Windows.Forms.Button btnGetOlder;
		private System.Windows.Forms.PropertyGrid grdDisplay;
		private System.Windows.Forms.TreeView tvObjectTree;

		private ObjectTreeViewer viewer;


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

			_client = GameClient.Instance;
			_client.OnProcessMessageEvent += new OnProcessMessageEventHandler(OnProcessMessageEvent);

			_dataStore = ClientDataStore.Instance;
			_dataStore.AssemblyName = "Riskier";
			_dataStore.OnNewEntityEvent += new OnNewEntityEventHandler(OnNewEntityEvent);
			_dataStore.OnRootEntityEvent += new OnRootEntityEventHandler(OnRootEntityEvent);
			_dataStore.OnModifyEntityEvent += new OnModifyEntityEventHandler(OnModifyEntityEvent);
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

			this.Invalidate();
		}

		private void OnRootEntityEvent(BaseEntity rootEntity)
		{
			Debug.WriteLine(String.Format("OnRootEntityEvent({0})", rootEntity));

			_persons = (PersonClasses.PersonList)rootEntity;
			viewer.Object = _persons;

			UpdateOnMainThread();
		}

		private void OnNewEntityEvent(BaseEntity instance)
		{
			Debug.WriteLine(String.Format("OnNewEntityEvent({0})", instance));

			UpdateOnMainThread();
		}

		private void OnModifyEntityEvent(BaseEntity e)
		{
			Debug.WriteLine(String.Format("OnModifyEntityEvent({0})", e.ID));

			UpdateOnMainThread();
		}

		private void OnProcessMessageEvent(object sender, byte[] message)
		{
			_dataStore.ProcessMessage(message);

			UpdateOnMainThread();
		}

        private void btnConnectToServer_Click(object sender, System.EventArgs e)
        {
            try
            {
				_client.Connect();
			}
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        private void WinForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _client.Disconnect();
        }

		private void btnGetFatter_Click(object sender, System.EventArgs e)
		{
			SelectedPerson.GrowFatter();
		}

		private void btnGetOlder_Click(object sender, System.EventArgs e)
		{
			SelectedPerson.GetOlder();
		}
		
		private void tvObjectTree_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			NodeDefinition def = viewer.SelectedObject;

			grdDisplay.SelectedObject = def.Instance;
			if (def.Property != "")
				SelectProperty(def.Property);
		}

		public PersonClasses.Person SelectedPerson
		{
			get {
				return grdDisplay.SelectedObject as PersonClasses.Person;
			}
		}
		
		private void frmClient_Load(object sender, System.EventArgs e)
		{
			viewer = new ObjectTreeViewer(tvObjectTree, null);
		}

    }
}
