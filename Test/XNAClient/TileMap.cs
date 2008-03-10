using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Form = System.Windows.Forms;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Text.RegularExpressions;

namespace Risk.Client.Drawing
{

    enum ZoomDirection { Up, Down };
    delegate void PositionChanged(object sender, Vector2 newPosition);

    class Position
    {
        private Vector2 _position;
        private int _maxHeight;

        public Position() { }

        public Position(float x, float y, int maxHeight)
        {
            _position = new Vector2(x, y);
            MaxHeight = maxHeight;
        }

        public int MaxHeight
        {
            get { return _maxHeight; }
            set { _maxHeight = -1 * value; }
        }

        public float X
        {
            get { return _position.X; }
            set
            {
                if (value < 0) 
                    value += 4000;
                _position.X = (value % 4000);
                if (OnPositionChanged != null)
                    OnPositionChanged(this, _position);
            }
        }

        public float Y
        {
            get { return _position.Y; }
            set {

                if (value > _maxHeight && value < 0)
                {
                    _position.Y = value;
                    if (OnPositionChanged != null)
                        OnPositionChanged(this, _position);
                }
            }
        }
	
        public static implicit operator Vector2(Position position)
        {
            return position._position;
        }

        public override string ToString()
        {
 	         return _position.ToString();
        }

        public event PositionChanged OnPositionChanged;
    }

    class TileMap : Component
    {
        const int   cDELTA      = 25;
        const int   cEDGE       = 80;
        const int   cZOOM       = 3;
        const float cZOOM_SCALE = 1.05f;

        GraphicsDevice _device;
        SpriteBatch    _spriteBatch;
        Texture2D[,]   _tiles;
        Form.Control   _owner;
        Position       _position;
        Vector2        _corner;

        int _columns;
        int _rows;
        int _width;
        int _positionSize = 0;
        int _mouseScroll;

        public TileMap(System.Windows.Forms.Control owner, GraphicsDevice device) : base(owner, device)
        {
            Enabled = false;

            _owner = owner;
            _device = device;

            _corner = new Vector2();
            _position = new Position(0, 0, _rows * _width - _owner.Height);
        }

        public override void Initialise()
        {
            _spriteBatch = new SpriteBatch(_device);
            MouseState ms = Mouse.GetState();
            _mouseScroll = ms.ScrollWheelValue;

            string fileName = @"..\..\Tiles\earthmap";
            string extension = "jpg";

            FindRange(fileName, extension);

            _width = 1000;
            _position = new Position(0, 0, _rows * _width - _owner.Height);

            _tiles = new Texture2D[_rows, _columns];
            for (int y = 0; y < _columns; y++)
                for (int x = 0; x < _rows; x++)
                {
                    string formattedName = String.Format(
                        "{0}{1}{2}.{3}",
                        fileName,
                        FormatPosition(y),
                        FormatPosition(x),
                        extension
                    );
                    _tiles[x, y] = Texture2D.FromFile(_device, formattedName);
                }

            Enabled = true;
        }

        private string FormatPosition(int y)
        {
            string x = y.ToString().PadLeft(_positionSize - y.ToString().Length + 1, '0');
            return x;
        }

        private void FindRange(string filePattern, string extension)
        {
            // find all files with the format of filename*.extension
            // foreach file, determine max row and column
            //   and also determine that each file is the same sized square
            //   assign this width to _width

            string path = System.IO.Path.GetDirectoryName(filePattern);
            string fileName = System.IO.Path.GetFileNameWithoutExtension(filePattern);
            string pattern = fileName + "*." + extension;

            string[] files = System.IO.Directory.GetFiles(path, pattern);

            _rows    = 0;
            _columns = 0;

            foreach (string file in files)
            {
                Match match =  Regex.Match(file, "[0-9]+");

                if (!match.Success)
                    throw new Exception("FileName must contain numerals");

                string position = match.Value;

                EnsureNumberFormat(position);

                _rows = Math.Max(_rows, Int32.Parse(position.Substring(_positionSize, _positionSize)));
                _columns = Math.Max(_columns, Int32.Parse(position.Substring(0, _positionSize)));
            }

            _rows++;
            _columns++;
        }

        private void EnsureNumberFormat(string position)
        {
            const string cINVALID_POSITION_SIZE = "file {0} has a different number pattern to previous file(s)";

            int currentPosition = position.Length / 2;

            if (_positionSize == 0)
                _positionSize = currentPosition;
            else
                if (_positionSize != currentPosition)
                    throw new Exception(String.Format(cINVALID_POSITION_SIZE, currentPosition));
        }

        private void CheckKeyboardScrolling()
        {
            // move view port according to keys
            KeyboardState ks = Keyboard.GetState();

            int updateBy = cDELTA;

            if (ks.IsKeyDown(Keys.LeftControl) || ks.IsKeyDown(Keys.RightControl))
                updateBy *= cZOOM;

            if (ks.IsKeyDown(Keys.Up))
                _position.Y += updateBy;
            if (ks.IsKeyDown(Keys.Down))
                _position.Y -= updateBy;
            if (ks.IsKeyDown(Keys.Left))
                _position.X += updateBy;
            if (ks.IsKeyDown(Keys.Right))
                _position.X -= updateBy;

            if (ks.IsKeyDown(Keys.PageDown))
                Zoom(ZoomDirection.Up);
            if (ks.IsKeyDown(Keys.PageUp))
                Zoom(ZoomDirection.Down);

            // Reset if Space is pressed
            if (ks.IsKeyDown(Keys.Space))
                _position = new Position(0, 0, _rows);

        }

        private void Zoom(ZoomDirection direction)
        {
            //Vector2 lastPosition = _position;
            float scale = 1f;
            switch (direction)
            {
                case ZoomDirection.Up:
                    scale = (float)cZOOM_SCALE;
                    break;
                case ZoomDirection.Down:
                    scale = (1 / cZOOM_SCALE);
                    break;
            }

            // determine new potential width
            int width = (int)(_width * scale);
            
            if (width > _owner.Height / _rows)
            {
                _width = width;
                _position.MaxHeight = _rows * _width - _owner.Height;
                //_delta.X += lastPosition.X * scale;
                //_delta.Y += lastPosition.Y * scale;
            }
        }

        private void CheckMouseScrolling()
        {
            // move view port according to mouse 'edging'
            MouseState ms = Mouse.GetState();
            Point p = new Point(ms.X - _owner.Left, ms.Y - _owner.Top);

            if (p.X > 0 && p.X < cEDGE)
                _position.X += cDELTA;
            if (p.Y > 0 && p.Y < cEDGE)
                _position.Y += cDELTA;

            if (p.X > 0 && p.X > _owner.Width - cEDGE)
                _position.X -= cDELTA;
            if (p.Y > 0 && p.Y > _owner.Height - cEDGE)
                _position.Y -= cDELTA;

            if (ms.ScrollWheelValue - _mouseScroll > 0)
                Zoom(ZoomDirection.Up);
            if (ms.ScrollWheelValue - _mouseScroll < 0)
                Zoom(ZoomDirection.Down);

            //(_owner.Parent as FormMain).Text = String.Format("ScrollWheel: {0}    _width: {1} owner.Height: {2}", ms.ScrollWheelValue, _width, _owner.Height);
        }

        public override void Update()
        {
            CheckMouseScrolling();
            CheckKeyboardScrolling();

            _corner.X = -1 * (int)_position.X / _width;
            _corner.Y = -1 * (int)_position.Y / _width;
        }

        public override void Draw()
        {
            _spriteBatch.Begin();

            int showCols = 1 + _owner.ClientRectangle.Width / _width;
            int showRows = 1 + _owner.ClientRectangle.Height / _width;

            for (int cols = -1; cols <= showCols; cols++)
                for (int rows = -1; rows <= showRows; rows++)
                {
                    int c = (cols + (int)_corner.X) % _columns;
                    if (c < 0) 
                        c += _columns;

                    int r = (rows + (int)_corner.Y) % _rows;
                    if (r < 0)
                        r += _rows;

                    Vector2 position = 
                        _position + 
                        new Vector2(
                            (float)(cols + _corner.X) * _width, 
                            (float)(rows + _corner.Y) * _width
                        );

                    _spriteBatch.Draw(
                        _tiles[r, c],
                        new Rectangle((int)position.X, (int)position.Y, _width, _width),
                        new Rectangle(0, 0, _tiles[r, c].Width, _tiles[r, c].Height),
                        Microsoft.Xna.Framework.Graphics.Color.White
                    );
                }

            _spriteBatch.End();
        }

        public Position Position
        {
            get { return _position; }
            set { _position  = value; }
        }

        public System.Drawing.Size Size
        {
            get { return new System.Drawing.Size(_columns * _width, _rows * _width); }
        }

        public event PositionChanged OnPositionChanged
        {
            add    { _position.OnPositionChanged += value; }
            remove { _position.OnPositionChanged -= value; }
        }
    }
}
