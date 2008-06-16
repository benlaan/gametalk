using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Laan.Riskier.Client.TileMap
{
    static class Constants
    {
        internal const float cSCALE_X_MULTIPLIER = 4f / 3;
        internal const float cMULTIPLIER = 0.7f;
        internal const int cLAND_TILE = 999;
        internal const int cHIGHLIGHT_TILE = 0;
        internal const int cWATER_TILE = 0;
        internal const int cEDGE = 50;
        internal const int cSCALE = 40;
        internal const int cOFFSET = 1;
    }

    internal class MapComponent : DrawableGameComponent
    {
        private Dictionary<int, int> _missing;
        private Dictionary<int, Texture2D> _tileImages;
        private Dictionary<Point, int> _indexCache;

        private int _scale;
        private Point _mouse;
        private IBitmap _bitmap;
        private float _offsetX;
        private float _offsetY;

        private MapScroller _scroller;
        private SpriteBatch _spriteBatch;
        private PrimitiveLine _line;
        private SpriteFont _courierNewEight;

        public MapComponent(Game game) : base(game)
        {
            _scroller = new MapScroller();

            _tileImages = new Dictionary<int, Texture2D>();
            _missing    = new Dictionary<int, int>();
            _indexCache = new Dictionary<Point, int>();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _courierNewEight = Game.Content.Load<SpriteFont>("CourierNew8");
            _bitmap = new TileBitmap(Game, Properties.Settings.Default.PoliticalMap);
            _scale = GraphicsDevice.Viewport.Height / _bitmap.Height;

            _line = new PrimitiveLine(GraphicsDevice);

            LoadAllTiles();
        }

        private void RetrieveFromDisk(int index)
        {
            // retrieve from disk
            string tileName = String.Format("tile{0}", index.ToString().PadLeft(3, '0'));
            try
            {
                // cache for future use, and return result
                _tileImages[index] = Game.Content.Load<Texture2D>(tileName);
            }
            catch (Exception)
            {
                Debug.WriteLine(index);
            }
        }

        private void LoadAllTiles()
        {
            for (int index = 0; index <= 255; index++)
                RetrieveFromDisk(index);

            RetrieveFromDisk(Constants.cLAND_TILE);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            MouseState ms = Mouse.GetState();
            _mouse = new Point(ms.X, ms.Y);

            CheckScrollAndZoom();
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();
            try
            {
                int maxRows = Math.Min(1 + GraphicsDevice.Viewport.Height / Scale, _bitmap.Height);
                int maxCols = Math.Min(1 + GraphicsDevice.Viewport.Width / Scale, _bitmap.Width);

                for (int y = 0; y < maxRows; y++)
                {
                    for (int x = 0; x < maxCols; x++)
                    {

                        int tileIndex = GetIndexFromMapPoint(GetPointWithOffset(x, y));

                        RenderTile(x, y, tileIndex);
                        CheckHighlight(x, y);
                        RenderBorders(x, y);
                    }
                }
                OffsetX = CalcHorizontalOffset(OffsetX);
            }
            finally
            {
                _spriteBatch.End();
            }
        }

        public int GetIndexFromMapPoint(Point point)
        {
            if (_indexCache.ContainsKey(point))
                return _indexCache[point];

            int result = 0;
            int exp = 0;

            if (_bitmap.IsLand(point.X, point.Y))
                return Constants.cLAND_TILE;

            // for water tiles, sample all neighbours to determine tile based on
            // combinations of edge tiles (bounding tiles)
            for (int y = -1; y <= 1; y++)
                for (int x = -1; x <= 1; x++)
                {
                    // ignore the focal point (input param)
                    if (x == 0 && y == 0)
                        continue;

                    if (_bitmap.IsLand(point.X + x, point.Y + y))
                        result += (int)Math.Pow(2, exp);

                    exp++;
                }

            _indexCache[point] = result;
            return result;
        }

        private void CheckScrollAndZoom()
        {
            KeyboardState keyState = Keyboard.GetState();

            _scroller.Update(keyState);

            if (keyState.IsKeyDown(Keys.Add) || keyState.IsKeyDown(Keys.PageUp))
                Scale++;

            if (keyState.IsKeyDown(Keys.Subtract) || keyState.IsKeyDown(Keys.PageDown))
                Scale--;

            UpdateOffsetWithMouseEdgeScrolling();

            // Calculate Offset from Scroller
            Point delta = _scroller.Offset;
            OffsetX += delta.X * Constants.cOFFSET;
            OffsetY += delta.Y * Constants.cOFFSET;
            _scroller.ClearDelta();
        }

        private void UpdateOffsetWithMouseEdgeScrolling()
        {
            if (_mouse.X < Constants.cEDGE)
                OffsetX -= Constants.cOFFSET;

            if (_mouse.X > GraphicsDevice.Viewport.Width - Constants.cEDGE)
                OffsetX += Constants.cOFFSET;

            if (_mouse.Y < Constants.cEDGE)
                OffsetY -= Constants.cOFFSET;

            if (_mouse.Y > GraphicsDevice.Viewport.Height - Constants.cEDGE)
                OffsetY += Constants.cOFFSET;
        }

        [Conditional("DEBUG")]
        private void FindMissingIndexes()
        {
            foreach (KeyValuePair<int, int> pair in _missing)
                Debug.WriteLine(String.Format("{0}: {1}", pair.Key, pair.Value));
        }

        private void DrawIndexAsText(int x, int y, int index)
        {
            Rectangle r = GetTileRect(x, y);

            _spriteBatch.DrawString(
                _courierNewEight,
                index.ToString(),
                new Vector2(r.X, r.Y),
                Color.White
            );
        }

        private Texture2D GetCachedTile(int index)
        {
            return _tileImages.ContainsKey(index) ? _tileImages[index] : null;
        }

        private Rectangle GetTileRect(int x, int y)
        {
            int scaleX = (int)(Scale * Constants.cSCALE_X_MULTIPLIER);
            int scaleY = Scale;
            return new Rectangle(x * scaleX, y * scaleY, scaleX, scaleY);
        }

        private void HighlightTile(int x, int y, Color highlight)
        {
            Rectangle rect = GetTileRect(x, y);
            _spriteBatch.Draw(
                GetCachedTile(Constants.cHIGHLIGHT_TILE),
                new Vector2(rect.X, rect.Y),
                new Color(255, 255, 255, 80)
            );
        }

        private void CheckHighlight(int x, int y)
        {
            Point dataPoint = GetPointWithOffset(x, y);

            Color currentColor = _bitmap.GetPixel(dataPoint.X, dataPoint.Y);
            Point p = FocusedTile;
            if (_bitmap.GetPixel(p.X, p.Y) == currentColor && _bitmap.IsLand(dataPoint.X, dataPoint.Y))
                HighlightTile(x, y, currentColor);
        }

        private int Add(Color current, Point p, int x, int y, int value)
        {
            Color color = _bitmap.GetPixel(p.X + x, p.Y + y);
            bool _bitmapIsLand = _bitmap.IsLand(p.X + x, p.Y + y);

            return (_bitmapIsLand && color != current) ? value : 0;
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

        public void CheckRenderLine(int edges, int mask, Vector2 from, Vector2 to, Color color)
        {
            if ((edges & mask) == mask)
            {
                _line.CreateLine(from, to, color);
                _line.Render(_spriteBatch);
            }
        }

        private void RenderBorders(int x, int y)
        {
            Point p = new Point(
                CalcHorizontalOffset(x + Offset.X),
                y + Offset.Y
            );

            if (!_bitmap.IsLand(p.X, p.Y))
                return;

            int e = GetEdgeCount(new Point(p.X, p.Y));
            Color color = _bitmap.GetPixel(p.X, p.Y);
            Color currentColor = new Color(color.R, color.G, color.B, 200);
            Rectangle rect = GetTileRect(x, y);
            const int cINSET = 2;

            CheckRenderLine(
                e,
                1,
                new Vector2(rect.Left - cINSET, rect.Top + cINSET),
                new Vector2(rect.Right + cINSET, rect.Top + cINSET),
                currentColor
            );
            CheckRenderLine(
                e,
                2,
                new Vector2(rect.Left + cINSET, rect.Top - cINSET),
                new Vector2(rect.Left + cINSET, rect.Bottom + cINSET),
                currentColor
            );
            CheckRenderLine(
                e,
                4,
                new Vector2(rect.Right - cINSET, rect.Top - cINSET),
                new Vector2(rect.Right - cINSET, rect.Bottom + cINSET),
                currentColor
            );
            CheckRenderLine(
                e,
                8,
                new Vector2(rect.Left - cINSET, rect.Bottom - cINSET),
                new Vector2(rect.Right - cINSET, rect.Bottom - cINSET),
                currentColor
            );
        }

        private void RenderTile(int x, int y, int index)
        {
            Texture2D tile = GetCachedTile(index);
            if (tile == null)
            {
                if (!_missing.ContainsKey(index))
                    _missing[index] = 1;
                else
                    _missing[index]++;

                DrawIndexAsText(x, y, index);
                return;
            }

            _spriteBatch.Draw(
                tile,
                GetTileRect(x, y),
                new Rectangle(0, 0, tile.Width, tile.Height),
                Color.White
            );

            //if (index == 000)
            //    CheckRenderIndexAsText(x, y, index);
        }

        internal Point GetPointWithOffset(int x, int y)
        {
            return new Point(
                CalcHorizontalOffset(x + Offset.X),
                y + Offset.Y
            );
        }

        private int CalcHorizontalOffset(float coord)
        {
            if (coord < 0)
                coord = _bitmap.Width + coord;

            if (coord >= _bitmap.Width)
                coord = coord - _bitmap.Width;

            return (int)coord;
        }

        internal void ReloadTiles()
        {
            _tileImages.Clear();
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
                if (value < 0 || (value > (_bitmap.Height - GraphicsDevice.Viewport.Height / Scale)))
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

                if (value < GraphicsDevice.Viewport.Height / _bitmap.Height)
                    return;

                int sgn = _scale < value ? 1 : -1;
                _scale = value;

                OffsetX += Constants.cMULTIPLIER * sgn;
                OffsetY += Constants.cMULTIPLIER * sgn;
            }
        }

        public Point Offset
        {
            get { return new Point((int)OffsetX, (int)OffsetY); }
        }

        public Point MousePoint
        {
            get { return _mouse; }
            set { _mouse = value; }
        }

        public Point FocusedTile
        {
            get
            {
                int scaleX = (int)(Scale * Constants.cSCALE_X_MULTIPLIER);

                return GetPointWithOffset(
                    (int)(_mouse.X / scaleX),
                    (int)(_mouse.Y / Scale)
                );
            }
        }

        public Point GeneratorOffset
        {
            get { return Offset; }
        }

        public Point MouseTile
        {
            get
            {
                int scaleX = (int)(Scale * Constants.cSCALE_X_MULTIPLIER);
                return new Point(
                    (int)(_mouse.X / scaleX),
                    (int)(_mouse.Y / Scale)
                );
            }
        }

        public Dictionary<int, int> MissingIndexes
        {
            get { return MissingIndexes; }
        }
    }
}
