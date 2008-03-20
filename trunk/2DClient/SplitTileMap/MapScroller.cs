using System;
using System.Drawing;
using System.Windows.Forms;

namespace SplitTileMap
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

        internal void KeyDown(KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Down:
                    _isDown = true;
                    break;

                case Keys.Up:
                    _isUp = true;
                    break;

                case Keys.Left:
                    _isLeft = true;
                    break;

                case Keys.Right:
                    _isRight = true;
                    break;
            }
        }

        internal void KeyUp(KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Down:
                    _isDown = false;
                    break;

                case Keys.Up:
                    _isUp = false;
                    break;

                case Keys.Left:
                    _isLeft = false;
                    break;

                case Keys.Right:
                    _isRight = false;
                    break;
            }
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
    }
}
