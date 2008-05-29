using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Diagnostics;

namespace SplitTileMap
{
    class TileBitmap : IBitmap
    {
        private Image _bitmap;
        private Color[,] _data;

        public TileBitmap(string fileName)
        {
            _bitmap = Bitmap.FromFile(fileName);
            _data = new Color[_bitmap.Width, _bitmap.Height];

            for (int x = 0; x < _bitmap.Width; x++)
                for (int y = 0; y < _bitmap.Height; y++)
                    _data[x, y] = ((Bitmap)_bitmap).GetPixel(x, y);
        }

        public Color GetPixel(int x, int y)
        {
            if (x < 0 || x >= _bitmap.Width || y < 0 || y >= _bitmap.Height)
                return Color.Black;
            else
                return _data[x, y];
        }

        public bool IsLand(int x, int y)
        {
            if (x < 0 || x >= _bitmap.Width || y < 0 || y >= _bitmap.Height)
                return false;

            Color color = GetPixel(x, y);
            return color.R > 0 || color.B > 0 || color.G > 0;
        }

        public int Height
        {
            get { return _bitmap.Height; }
        }

        public int Width
        {
            get { return _bitmap.Width; }
        }
    }
}
