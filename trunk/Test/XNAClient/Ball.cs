using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework.Graphics;

namespace Risk.Client.Drawing
{
    class Ball : Sprite
    {

        protected System.Drawing.Rectangle _perimeter;

        public Ball(System.Windows.Forms.Control owner, GraphicsDevice device)
            : base(owner, device)
        {
            FileName = @"..\..\ball.png";
            _perimeter = new System.Drawing.Rectangle(0, 0, 0, 0);
        }

        public override void Initialise()
        {
            base.Initialise();

            _perimeter = new System.Drawing.Rectangle(
                0, 0,
                Owner.Size.Width - Image.Width,
                Owner.Size.Height - Image.Height
            );
        }
    }
}
