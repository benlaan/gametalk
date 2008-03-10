using System;
using System.Collections.Generic;
using System.Text;
using Xna = Microsoft.Xna.Framework;
using Drawing = System.Drawing;
using Microsoft.Xna.Framework;

namespace Risk.Client.Drawing
{
    class Geometry
    {

        static public bool PointInRectangle(Xna.Point p, Xna.Rectangle r)
        {
            return (
                p.X > r.Left && p.X < (r.Right)
                &&
                p.Y > r.Top && p.Y < (r.Bottom)
               );
        }
    }
}
