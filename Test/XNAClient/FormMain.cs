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

namespace Risk.Client.Drawing
{

    public partial class FormMain : Form
    {

        private ComponentCollection _components;
        GraphicsDevice device;
        Polygon2 _polygon;

        TileMap _map;

        public static Random Random = new Random();
        private System.Drawing.Point _mouse = new System.Drawing.Point();

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
            this.Text = _polygon.ToString();
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

            _map = new TileMap(pnlMap, device);
            //_components.Add(_map);


            List<Vector2> coords = new List<Vector2>()
            {
                //new Vector2(-2.5f, -7.5f),
                //new Vector2(-8, -4),
                //new Vector2(-2, -4),
                //new Vector2(-7.5f, -7.5f),
                //new Vector2(-5, -2),
                //new Vector2(-2.5f, -7.5f)

                new Vector2(500, 250),
                new Vector2(475, 425),
                new Vector2(250, 500),
                new Vector2(750, 500)
            };
            _polygon = new Polygon2(_map, coords, pnlMap, device);
            _components.Add(_polygon);
        
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("Test Button Clicked");
            //_components.Add(new MouseBall(this.pnlMap, device));
            //for (int i = 0; i < 5; i++)
            //_components.Add(new BouncingBall(this.pnlMap, device));
        }

        internal void AddMessage(string message)
        {
           txtNotification.AppendText(message + Environment.NewLine);
        }

        private void pnlMap_MouseClick(object sender, MouseEventArgs e)
        {
            _mouse = new System.Drawing.Point(e.X, e.Y);

            this.Text = _mouse.ToString();

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