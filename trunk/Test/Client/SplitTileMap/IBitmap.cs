using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Diagnostics;

namespace SplitTileMap
{
    public interface IBitmap
    {
        bool IsLand(int x, int y);
        Color GetPixel(int x, int y);

        int Height { get; }
        int Width { get; }
    }
}
