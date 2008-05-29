using System;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.IO;

namespace SplitTileMap
{
    public class TileMapEngine
    {
        private const float cSCALE_X_MULTIPLIER = 4f / 3;
        private const int cLAND_TILE = 999;
        private const int cWATER_TILE = 0;
        private const float cMULTIPLIER = 0.7f;

        private Graphics _graphics;
        private Image _output;
        private int _scale;
        private string _tilePath;
        private Color _highlightColor;

        private Control _control;
        private Point _mouse;

        private Dictionary<int, int> _missing;
        private Dictionary<int, Image> _tileImages;
        private Dictionary<Point, int> _indexCache;

        private IBitmap _bitmap;
        private float _offsetX;
        private float _offsetY;

        public TileMapEngine(Control control)
            : this()
        {
            _control = control;
        }

        public TileMapEngine()
        {
            _missing = new Dictionary<int, int>();
            _tileImages = new Dictionary<int, Image>();
            _indexCache = new Dictionary<Point, int>();
        }

        public int GetIndexFromMapPoint(Point point)
        {
            if (_indexCache.ContainsKey(point))
                return _indexCache[point];

            int result = 0;
            int exp = 0;

            if (_bitmap.IsLand(point.X, point.Y))
                return cLAND_TILE;

            // for water tiles, sample all neighbours to determine tile based on
            // combinations of edge tiles (bounding tiles)
            for (int y = -1; y <= 1; y++)
                for (int x = -1; x <= 1; x++)
                {
                    // ignore the focal point (input param)
                    if (x == 0 && y == 0)
                        continue;

                    if (Bitmap.IsLand(point.X + x, point.Y + y))
                        result += (int)Math.Pow(2, exp);

                    exp++;
                }

            _indexCache[point] = result;
            return result;
        }

        private void ShowOutput(string outFile)
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = outFile;
            psi.WindowStyle = ProcessWindowStyle.Normal;
            Process.Start(psi);
        }

        public void Initialise(int scale)
        {
            _output = new Bitmap(_bitmap.Width * scale, _bitmap.Height * scale);
            _graphics = Graphics.FromImage(_output);
            Scale = scale;
        }

        private void FindMissingIndexes()
        {
            foreach (KeyValuePair<int, int> pair in _missing)
                Debug.WriteLine(String.Format("{0}: {1}", pair.Key, pair.Value));
        }

        private void CheckRenderIndexAsText(int x, int y, int index)
        {
            Rectangle r = GetTileRect(x, y);

            _graphics.DrawString(
                index.ToString(),
                new Font("Courier New", 8),
                new SolidBrush(Color.White),
                new Point(r.X, r.Y)
            );
        }

        private Image GetTileFromIndex(int index)
        {
            // Check image cache
            if (_tileImages.ContainsKey(index))
                return _tileImages[index];

            // retrieve from disk
            string fileName = String.Format(
                "{0}\\tile{1}.png",
                TilePath, index.ToString().PadLeft(3, '0')
            );

            if (!File.Exists(fileName))
                return null;

            Image image = new Bitmap(Image.FromFile(fileName));

            // cache for future use, and return result
            _tileImages[index] = image;
            return image;
        }

        private Rectangle GetTileRect(int x, int y)
        {
            int scaleX = (int)(Scale * cSCALE_X_MULTIPLIER);
            int scaleY = Scale;
            return new Rectangle(x * scaleX, y * scaleY, scaleX, scaleY);
        }

        private void HighlightTile(int x, int y, Color highlight)
        {
            RectangleF rect = GetTileRect(x, y);
            _graphics.FillRectangle(
                new SolidBrush(Color.FromArgb(50, Color.White)),
                rect
            );
        }

        private void CheckHighlight(int x, int y)
        {
            Point dataPoint = new Point(
                CalcHorizontalOffset(x + Offset.X),
                y + Offset.Y
            );

            Color currentColor = _bitmap.GetPixel(dataPoint.X, dataPoint.Y);
            Point p = FocusedTile;
            if (Bitmap.GetPixel(p.X, p.Y) == currentColor && Bitmap.IsLand(dataPoint.X, dataPoint.Y))
                HighlightTile(x, y, currentColor);
        }

        private int Add(Color current, Point p, int x, int y, int value)
        {
            Color color = Bitmap.GetPixel(p.X + x, p.Y + y);
            bool bitmapIsLand = Bitmap.IsLand(p.X + x, p.Y + y);

            return (bitmapIsLand && color != current) ? value : 0;
        }

        private int GetEdgeCount(Point point)
        {
            int result = 0;

            Color current = _bitmap.GetPixel(point.X, point.Y);
            result += Add(current, point, 0, -1, 1);
            result += Add(current, point, -1, 0, 2);
            result += Add(current, point, 1, 0, 4);
            result += Add(current, point, 0, 1, 8);

            return result;
        }

        private void RenderBorders(int x, int y)
        {
            Point p = new Point(
                CalcHorizontalOffset(x + Offset.X),
                y + Offset.Y
            );

            if (!Bitmap.IsLand(p.X, p.Y))
                return;

            int e = GetEdgeCount(new Point(p.X, p.Y));

            Color currentColor = _bitmap.GetPixel(p.X, p.Y);
            Pen pen = new Pen(new SolidBrush(Color.FromArgb(80, currentColor)), 5);
            Rectangle rect = GetTileRect(x, y);

            const int cINSET = 2;

            if ((e & 1) == 1)
                _graphics.DrawLine(pen, new Point(rect.Left - cINSET, rect.Top + cINSET), new Point(rect.Right + cINSET, rect.Top + cINSET));
            if ((e & 2) == 2)
                _graphics.DrawLine(pen, new Point(rect.Left + cINSET, rect.Top - cINSET), new Point(rect.Left + cINSET, rect.Bottom + cINSET));
            if ((e & 4) == 4)
                _graphics.DrawLine(pen, new Point(rect.Right - cINSET, rect.Top - cINSET), new Point(rect.Right - cINSET, rect.Bottom + cINSET));
            if ((e & 8) == 8)
                _graphics.DrawLine(pen, new Point(rect.Left -
                    cINSET, rect.Bottom - cINSET), new Point(rect.Right - cINSET, rect.Bottom - cINSET));

            //_graphics.DrawString(
            //    e.ToString(),
            //    new Font("Courier New", 8),
            //    new SolidBrush(Color.White),
            //    new Point(r.X, r.Y)
            //);
        }

        private void RenderTile(int x, int y, int index)
        {
            Image tile = GetTileFromIndex(index);
            if (tile == null)
            {
                if (!_missing.ContainsKey(index))
                    _missing[index] = 1;
                else
                    _missing[index]++;

                CheckRenderIndexAsText(x, y, index);
                return;
            }

            _graphics.DrawImage(
                tile,
                GetTileRect(x, y),
                new Rectangle(0, 0, tile.Width, tile.Height),
                GraphicsUnit.Pixel
            );

            //if (index == 000)
            //    CheckRenderIndexAsText(x, y, index);

            CheckHighlight(x, y);
            RenderBorders(x, y);
        }

        public void GenerateMapToFile(string outFile)
        {
            Execute();
            _output.Save(outFile, ImageFormat.Png);

            FindMissingIndexes();
            ShowOutput(outFile);
        }

        public void Render(Graphics graphics)
        {
            _graphics = graphics;
            Execute();
        }

        private int CalcHorizontalOffset(float coord)
        {
            if (coord < 0)
                coord = Bitmap.Width + coord;

            if (coord >= Bitmap.Width)
                coord = coord - Bitmap.Width;

            return (int)coord;
        }

        private void Execute()
        {
            int rowsShowing = 1 + _control.Height / Scale;
            int colsShowing = 1 + _control.Width / Scale;

            for (int y = 0; y < Math.Min(rowsShowing, Bitmap.Height); y++)
                for (int x = 0; x < Math.Min(colsShowing, Bitmap.Width); x++)
                {
                    int tileIndex = GetIndexFromMapPoint(
                        new Point(
                            CalcHorizontalOffset(x + Offset.X),
                            y + Offset.Y
                        )
                    );
                    RenderTile(x, y, tileIndex);
                }

            OffsetX = CalcHorizontalOffset(OffsetX);
        }

        public IBitmap Bitmap
        {
            get { return _bitmap; }
            set { _bitmap = value; }
        }

        public Dictionary<int, int> MissingIndexes
        {
            get { return _missing; }
        }

        public Point Mouse
        {
            get { return _mouse; }
            set { _mouse = value; }
        }

        public float OffsetX
        {
            get { return _offsetX; }
            set { _offsetX = value; }
        }

        public float OffsetY
        {
            get { return _offsetY; }
            set
            {
                if (value < 0 || (value > (Bitmap.Height - _control.Bounds.Height / Scale)))
                    return;
                _offsetY = value;
            }
        }

        public int Scale
        {
            get { return _scale; }
            set
            {
                if (_scale == value)
                    return;

                if (value < _control.Height / Bitmap.Height)
                    return;

                int sgn = _scale < value ? 1 : -1;
                _scale = value;

                OffsetX += cMULTIPLIER * sgn;
                OffsetY += cMULTIPLIER * sgn;

                _control.Invalidate();
            }
        }

        public string TilePath
        {
            get { return _tilePath; }
            set { _tilePath = value; }
        }

        public Point Offset
        {
            get { return new Point((int)OffsetX, (int)OffsetY); }
        }

        public Color HighlightColor
        {
            get { return _highlightColor; }
            set { _highlightColor = value; }
        }

        internal void ReloadTiles()
        {
            _tileImages.Clear();
        }

        public Point FocusedTile
        {
            get
            {
                int scaleX = (int)(Scale * cSCALE_X_MULTIPLIER);
                return new Point(
                    CalcHorizontalOffset((int)(_mouse.X / scaleX) + Offset.X),
                    (int)(_mouse.Y / Scale) + Offset.Y
                );
            }
        }

        public Point MouseTile
        {
            get
            {
                int scaleX = (int)(Scale * cSCALE_X_MULTIPLIER);
                return new Point(
                    (int)(_mouse.X / scaleX),
                    (int)(_mouse.Y / Scale)
                );
            }
        }

        internal Point GetPointWithOffset(int x, int y)
        {
            return new Point(
                CalcHorizontalOffset(x + Offset.X),
                y + Offset.Y
            );
        }
    }
}

