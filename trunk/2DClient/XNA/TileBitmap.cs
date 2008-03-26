using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Laan.Riskier.Client.TileMap
{
    class TileBitmap : IBitmap
    {
        private Texture2D _bitmap;
        private Color[,] _data;

        public TileBitmap(Game game, string fileName)
        {
            _bitmap = game.Content.Load<Texture2D>(fileName);
            _data = new Color[_bitmap.Width, _bitmap.Height];

            int[] raw = new int[_bitmap.Width * _bitmap.Height];
            _bitmap.GetData<int>(raw);

            for (int x = 0; x < _bitmap.Width; x++)
                for (int y = 0; y < _bitmap.Height; y++)
                {
                    int color = raw[y * _bitmap.Width + x];
                    byte b = (byte)color;
                    byte g = (byte)(color >> 8);
                    byte r = (byte)(color >> 16);
                    _data[x, y] = new Color(r, g, b);
                }
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
