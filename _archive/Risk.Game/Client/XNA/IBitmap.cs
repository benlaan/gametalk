using System;
using Microsoft.Xna.Framework.Graphics;

namespace Laan.Riskier.Client.TileMap
{
    public interface IBitmap
    {
        bool IsLand(int x, int y);
        Color GetPixel(int x, int y);

        int Height { get; }
        int Width { get; }
    }
}
