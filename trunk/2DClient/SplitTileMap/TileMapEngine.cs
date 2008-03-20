using System;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SplitTileMap
{
    public class TileMapEngine
    {
        private bool _renderIndex;
        private Graphics _graphics;
        private List<int> _missing;
        private Image _output;
        private int _scale;
        private string _tilePath;
        private Color _highlightColor;

        private Control _control;

        const int cLAND_TILE = 999;
        const int cWATER_TILE = 0;

        private Dictionary<int, Image> _tileImages;
        Dictionary<int, string> _tiles = new Dictionary<int, string>();

        private IBitmap _bitmap;
        public float OffsetX;
        private float _offsetY;

        private void MapTilesToIndexes()
        {
            _tiles[cWATER_TILE] = "AllWater";
            _tiles[cLAND_TILE] = "AllLand";

            _tiles[41] = "00-04";
            _tiles[7] = "00-05";
            _tiles[43] = "00-06";
            _tiles[150] = "00-07";

            _tiles[47] = "01-00";
            _tiles[227] = "01-00";
            _tiles[244] = "01-01";
            _tiles[233] = "01-02";
            _tiles[151] = "01-03";
            _tiles[148] = "01-04";
            _tiles[224] = "01-05";
            _tiles[105] = "01-06";
            _tiles[212] = "01-07";

            _tiles[192] = "02-00";
            _tiles[96] = "02-01";
            _tiles[3] = "02-02";
            _tiles[6] = "02-03";
            _tiles[64] = "02-04";
            _tiles[2] = "02-05";

            _tiles[128] = "03-02";
            _tiles[22] = "03-03";
            _tiles[15] = "03-04";
            _tiles[23] = "03-05";

            _tiles[104] = "04-01";
            _tiles[234] = "04-01";
            _tiles[240] = "04-07";
            _tiles[232] = "04-06";

            _tiles[9] = "05-07";
            _tiles[11] = "05-03";
            _tiles[160] = "05-04";
            _tiles[31] = "05-05";
            _tiles[20] = "05-06";
            _tiles[21] = "05-06";
            _tiles[32] = "05-00";

            _tiles[1] = "06-00";
            _tiles[4] = "06-01";
            _tiles[208] = "06-02";
            _tiles[248] = "06-04";
            _tiles[5] = "06-05";

            _tiles[144] = "07-00";
            _tiles[40] = "07-01";
            _tiles[36] = "07-02";
            _tiles[129] = "07-03";
            _tiles[16] = "07-04";
            _tiles[8] = "07-05";

            _tiles[53] = "07-05";
            _tiles[13] = "07-06";
        }

        public TileMapEngine(Control control) : this()
        {
            _control = control;
        }

        public TileMapEngine()
        {
            _missing = new List<int>();
            _tileImages = new Dictionary<int, Image>();

            MapTilesToIndexes();
        }

        public int GetIndexFromMapPoint(Point point)
        {
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
            return result;
        }

        private static void ShowOutput(string outFile)
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
            _missing.Sort();
            foreach (int index in _missing)
                Debug.WriteLine(index);
        }

        private void CheckRenderIndexAsText(int y, int x, int index)
        {
            if (_renderIndex)
                _graphics.DrawString(
                    index.ToString(),
                    new Font("Courier New", 8),
                    new SolidBrush(Color.White),
                    new Point(x * Scale, y * Scale + (Scale / 2))
                );
        }

        private Image GetTileFromIndex(int index)
        {
            // Check cache
            if (_tileImages.ContainsKey(index))
                return _tileImages[index];

            if (!_tiles.ContainsKey(index))
                return null;

            // retrieve from disk
            Image image = Image.FromFile(
                String.Format("{0}\\tile{1}.png", TilePath, _tiles[index])
            );

            // cache for future use, and return result
            _tileImages[index] = image;
            return image;
        }

        private Rectangle GetTileRect(int x, int y)
        {
            int scaleX = Scale * 4 / 3;
            int scaleY = Scale;
            return new Rectangle(x * scaleX, y * scaleY, scaleX, scaleY);
        }

        private void HighlightTile(int x, int y, Color highlight)
        {
            RectangleF rect = GetTileRect(x, y);
            rect.Inflate(8, 8);
            _graphics.FillEllipse(
                new LinearGradientBrush(
                    rect,
                    Color.FromArgb(90, highlight),
                    Color.FromArgb(30, highlight),
                    0,
                    false
                ),
                rect
            );
        }

        private void CheckHighlight(int x, int y)
        {
            Color currentColor = _bitmap.GetPixel(x, y);
            //if (_highlightColor == currentColor)
            //    HighlightTile(x, y, Color.Yellow);

            //if (Bitmap.IsLand(x, y))
            //    HighlightTile(x, y, currentColor);
        }

        private void RenderTile(int x, int y, int index)
        {
            Image tile = GetTileFromIndex(index);
            if (tile == null)
            {
                if (!_missing.Contains(index))
                    _missing.Add(index);
                return;
            }

            _graphics.DrawImage(
                tile,
                GetTileRect(x, y),
                new Rectangle(0, 0, tile.Width, tile.Height),
                GraphicsUnit.Pixel
            );
            CheckHighlight(x, y);
        }

        public void GenerateMapToFile(string outFile)
        {
            Execute();
            _output.Save(outFile, ImageFormat.Png);

            FindMissingIndexes();
            ShowOutput(outFile);
        }

        public void Render(Graphics g)
        {
            _graphics = g;
            Execute();
        }

        private float CapCoord(float coord, int cap)
        {
            if (coord < 0)
                coord = cap + coord;

            if (coord > cap)
                coord = coord - cap;

            return coord;
        }

        private PointF CapPoints(PointF result)
        {
            result.X = CapCoord(result.X, Bitmap.Width);
            return result;
        }

        internal Point GetPointWithOffset(int x, int y)
        {
            PointF result = CapPoints(new PointF(Offset.X + x, Offset.Y + y));
            return new Point((int)result.X, (int)result.Y);
        }

        private void Execute()
        {
            int rowsShowing = _control.Height / Scale;
            int colsShowing = _control.Width / Scale;

            for (int y = 0; y < Bitmap.Height; y++)
            {
                if (y > rowsShowing)
                    break;

                for (int x = 0; x < Bitmap.Width; x++)
                {
                    if (x > colsShowing)
                        break;

                    Point offset = GetPointWithOffset(x, y);

                    int tileIndex = GetIndexFromMapPoint(offset);
                    RenderTile(x, y, tileIndex);
                    CheckRenderIndexAsText(y, x, tileIndex);
                }
            }
            
            OffsetX = CapCoord(OffsetX, Bitmap.Width);
        }

        public IBitmap Bitmap
        {
            get { return _bitmap; }
            set { _bitmap = value; }
        }

        public List<int> MissingIndexes
        {
            get { return _missing; }
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

        public bool RenderIndex
        {
            get { return _renderIndex; }
            set { _renderIndex = value; }
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

                OffsetX += 0.7f * sgn;
                OffsetY += 0.7f * sgn;

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

        public Color HiglightColor
        {
            get { return _highlightColor; }
            set { _highlightColor = value; }
        }
    }
}

