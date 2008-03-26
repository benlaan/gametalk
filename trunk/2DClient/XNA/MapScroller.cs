using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Laan.Riskier.Client.TileMap
{
    internal class MapScroller
    {

        private Point _delta;
        private bool _isDown;
        private bool _isLeft;
        private bool _isRight;
        private bool _isUp;

        public MapScroller()
        {
            _delta = new Point();
        }

        public void ClearDelta()
        {
            _delta.X = 0;
            _delta.Y = 0;
        }

        public void Reset()
        {
            _isRight = false;
            _isLeft = false;
            _isUp = false;
            _isDown = false;
        }

        internal Point Offset
        {
            get
            {
                if (_isDown)
                    _delta.Y++;
                if (_isUp)
                    _delta.Y--;
                if (_isLeft)
                    _delta.X--;
                if (_isRight)
                    _delta.X++;

                return _delta;
            }
        }

        internal void Update(KeyboardState k)
        {
            _isDown = k.IsKeyDown(Keys.Down);
            _isUp = k.IsKeyDown(Keys.Up);
            _isLeft = k.IsKeyDown(Keys.Left);
            _isRight = k.IsKeyDown(Keys.Right);
        }
    }
}
