using System;
using System.Collections.Generic;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace Risk.Client.Drawing
{
    class MouseBall : Ball
    {

        public MouseBall(System.Windows.Forms.Control owner, GraphicsDevice device)
            : base(owner, device)
        {
        }

        public override void Update()
        {
            // TODO: Add your update code here

            MouseState ms = Mouse.GetState();

            Position = new Vector2(
                ms.X - (Image.Width / 2),
                ms.Y - (Image.Height / 2)
            );

            Vector2 t = Position - Offset;

            if (t.X < _perimeter.Left)
                Position.X = Offset.X;
            if (t.Y < _perimeter.Top)
                Position.Y = Offset.Y;

            if (t.X > _perimeter.Right)
                Position.X = _perimeter.Right + Offset.X;
            if (t.Y > _perimeter.Bottom)
                Position.Y = _perimeter.Bottom + Offset.Y;

            if (ms.LeftButton == ButtonState.Pressed)
                Debug.WriteLine("Pos: " + Position + " Off: " + Offset + " Delta: " + (Position - Offset));

            base.Update();
        }
    }
}
