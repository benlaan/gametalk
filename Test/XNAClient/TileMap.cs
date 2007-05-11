using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Form = System.Windows.Forms;
using Microsoft.Xna.Framework.Input;

namespace RiskClient
{
    class TileMap : Component
    {

        const int cDELTA = 10;
        const int cEDGE  = 40;

        GraphicsDevice _device;
        SpriteBatch _spriteBatch;

        Texture2D[,] _textures;

        Form.Control _owner;

        bool _enabled;
        int _columns;
        int _rows;
        int _width;

        public TileMap(System.Windows.Forms.Control owner, GraphicsDevice device, int rows, int columns, int width)
        {
            Enabled = false;

            _owner = owner;
            _columns = columns;
            _rows = rows;
            _width = width;
            _device = device;
        }

        public override void Initialise()
        {
            _spriteBatch = new SpriteBatch(_device);

            _textures = new Texture2D[_rows, _columns];
            for (int y = 0; y < _columns; y++)
                for (int x = 0; x < _rows; x++)
                    _textures[x, y] = Texture2D.FromFile(_device, String.Format(@"..\..\tile{0}{1}.png", x, y));

            Enabled = true;
        }

        private static void CheckKeyboardScrolling(ref Vector2 delta)
        {
            // move view port according to keys
            KeyboardState ks = Keyboard.GetState();

            if (ks.IsKeyDown(Keys.Up))
                delta.Y -= cDELTA;
            if (ks.IsKeyDown(Keys.Down))
                delta.Y += cDELTA;
            if (ks.IsKeyDown(Keys.Left))
                delta.X -= cDELTA;
            if (ks.IsKeyDown(Keys.Right))
                delta.X += cDELTA;
        }

        private void CheckMouseScrolling(ref Vector2 delta)
        {
            // move view port according to mouse 'edging'
            MouseState ms = Mouse.GetState();
            Point p = new Point(ms.X - _owner.Left, ms.Y - _owner.Top);

            if (p.X < cEDGE)
                delta.X -= cDELTA;
            if (p.Y < cEDGE)
                delta.Y -= cDELTA;

            if (p.X > _owner.Width - cEDGE)
                delta.X += cDELTA;
            if (p.Y > _owner.Height - cEDGE)
                delta.Y += cDELTA;
        }

        public override void Update()
        {
            Vector2 delta = new Vector2();

            CheckMouseScrolling(ref delta);
            CheckKeyboardScrolling(ref delta);

            (_owner.Parent as FormMain).Text = delta.ToString();
        }

        public override void Draw()
        {
            //float scaleC = (float)_owner.Size.Width / _width / _columns;
            //float scaleR = (float)_owner.Size.Height / _width / _rows;

            _spriteBatch.Begin();
            for(int y = 0; y < _columns; y++)
                for (int x = 0; x < _rows; x++)
                {
                    //Rectangle r = 
                    //    new Rectangle(
                    //        (int)(y * _width * scaleC), (int)(x * _width * scaleR),
                    //        (int)(_width * scaleC), (int)(_width * scaleR)
                    //    );
                    _spriteBatch.Draw(
                        _textures[x, y],
                        //r,
                        new Vector2((float)y * _width, (float)x * _width), 
                        Microsoft.Xna.Framework.Graphics.Color.White
                    );
                }
            _spriteBatch.End();
        }

        #region IComponent Members


        public bool Enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                _enabled = value;
            }
        }

        #endregion
    }
}
