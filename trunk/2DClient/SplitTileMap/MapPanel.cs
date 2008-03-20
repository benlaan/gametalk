using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace SplitTileMap
{
    internal class MapPanel : Panel
    {
        // Components
        private MapScroller _scroller;
        private TileMapEngine _engine;
        private Point _mouse;

        // Frame Rate Bits
        private TimeSpan _oneSecond = new TimeSpan(0, 0, 0, 1);
        private float _fps = 0;
        private int _frameCount = 0;
        private DateTime _lastFPSTime;

        private const int cEDGE = 35;
        private const int cSCALE = 30;
        private const int cOFFSET = 2;

        public MapPanel() : base()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);

            _engine = new TileMapEngine(this);
        }

        public void Start()
        {
            _scroller = new MapScroller();

            _engine.TilePath = Properties.Settings.Default.TilePath;
            _engine.Bitmap = new TileBitmap(Properties.Settings.Default.PoliticalMap);
            _engine.Initialise(cSCALE);

            _lastFPSTime = DateTime.Now;

            this.Paint += new PaintEventHandler(OnPaint);
        }

        private void CalcFrameRate()
        {
            _frameCount++;

            DateTime currentTime = DateTime.Now;
            if (currentTime - _lastFPSTime > _oneSecond)
            {
                _fps = _frameCount;
                _frameCount = 0;

                if (OnFPSUpdate != null)
                    OnFPSUpdate(this, EventArgs.Empty);

                _lastFPSTime = currentTime;
            }
        }

        void OnPaint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(new SolidBrush(Color.Black), this.Bounds);

            if (DesignMode)
                return;

            CalcFrameRate();

            _engine.Render(e.Graphics);

            Update();
            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            _mouse = new Point(e.X, e.Y);
        }

        private void CalculateOffset()
        {
            Point delta = _scroller.Offset;
            _scroller.ClearDelta();

            _engine.OffsetX += delta.X * cOFFSET;
            _engine.OffsetY += delta.Y * cOFFSET;
        }

        public void ZoomIn()
        {
            ZoomScale++;
        }

        public void ZoomOut()
        {
            ZoomScale--;
        }

        public Point GeneratorOffset
        {
            get { return _engine.Offset; }
        }

        internal new void Update()
        {
            CalculateOffset();

            if (_mouse.X < cEDGE)
                _engine.OffsetX -= cOFFSET;

            if (_mouse.X > this.Bounds.Width - cEDGE)
                _engine.OffsetX += cOFFSET;

            if (_mouse.Y < cEDGE)
                _engine.OffsetY -= cOFFSET;

            if (_mouse.Y > this.Bounds.Height - cEDGE)
                _engine.OffsetY += cOFFSET;

            Invalidate();
        }

        internal void CheckKeyScrollingDown(KeyEventArgs e)
        {
            _scroller.KeyDown(e);
        }

        internal void CheckKeyScrollingUp(KeyEventArgs e)
        {
            _scroller.KeyUp(e);
        }

        public int ZoomScale
        {
            get { return _engine.Scale; }
            set { _engine.Scale = value; }
        }

        public float FPS
        {
            get { return _fps; }
        }

        public event EventHandler OnFPSUpdate;

        public List<int> MissingIndexes
        {
            get { return _engine.MissingIndexes; }
        }


    }
}
