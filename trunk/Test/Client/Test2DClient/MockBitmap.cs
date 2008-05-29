using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Diagnostics;

namespace SplitTileMap
{
    class MockBitmap : IBitmap
    {
        int[][] _bits;
        int _height;
        int _width;

        public MockBitmap(int[][] bits)
        {
            _bits = bits;
            _height = bits[0].Length;
            _width = bits.Length;
        }

        public Color GetPixel(int x, int y)
        {
            return _bits[y][x] > 0 ? Color.White : Color.Black;
        }

        public bool IsLand(int x, int y)
        {
            if (x <= -1 || x >= Height || y <= -1 || y >= Width)
                return false;
            else
            {
                return _bits[y][x] > 0;
            }
        }

        public int Height
        {
            get { return _height; }
        }

        public int Width
        {
            get { return _width; }
        }
    }
}
