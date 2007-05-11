using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Content;

namespace RiskClient
{

    public partial class FormMain : Form
    {

        private ComponentCollection _components;
        GraphicsDevice device;

        public static Random Random = new Random();

        public FormMain()
        {
            InitializeComponent();
        }

        internal void Initialize()
        {
            Trace.Listeners.Add(new FormLogger(this));
        }

        internal void DrawComponents()
        {
            _components.Draw();
        }

        internal void UpdateComponents()
        {
            _components.Update();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            PresentationParameters pp = new PresentationParameters();

            pp.BackBufferCount = 1;
            pp.BackBufferFormat = SurfaceFormat.Unknown;
            pp.BackBufferHeight = pnlMap.Height;
            pp.BackBufferWidth = pnlMap.Width;
            pp.DeviceWindowHandle = pnlMap.Handle;
            pp.IsFullScreen = false;
            pp.SwapEffect = SwapEffect.Default;

            device = new GraphicsDevice(
                GraphicsAdapter.DefaultAdapter,
                DeviceType.Hardware, pnlMap.Handle,
                CreateOptions.SoftwareVertexProcessing,
                pp
            );

            this.Update();

            _components = new ComponentCollection(device);

            TileMap _map = new TileMap(pnlMap, device, 2, 3, 100);
            _components.Add(_map);

//            _components.Add(new MouseBall(this.pnlMap, _device));
            for(int i = 0; i < 1; i++)
                _components.Add(new BouncingBall(this.pnlMap, device));       

        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("Test Button Clicked");
        }

        internal void AddMessage(string message)
        {
           txtNotification.AppendText(message + Environment.NewLine);
        }

        private void pnlMap_MouseClick(object sender, MouseEventArgs e)
        {
//          System.Drawing.Point mouse = new System.Drawing.Point(e.X, e.Y);
//          foreach (IComponent component in _components)
//                if (component.Enabled)
//                    if (component.PointInBoundingBox(mouse))
//                        Debug.WriteLine(String.Format("selected {0}", component.ID));
        }
        
    }

    public class FormLogger : DefaultTraceListener
    {
        private FormMain _form;

        public FormLogger(FormMain form)
        {
            _form = form;
        }

        public override void Write(string data)
        {
            _form.AddMessage(data);
        }

        public override void WriteLine(string data)
        {
            _form.AddMessage(data);
        }
    }
}