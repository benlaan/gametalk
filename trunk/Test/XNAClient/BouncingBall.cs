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
    class BouncingBall : Ball
    {

        readonly int cBOUNCE = 40;

        private Vector2 _direction;

        public BouncingBall(System.Windows.Forms.Control owner, GraphicsDevice device) : base(owner, device)
        {
            Position = new Vector2(owner.Width / 2, owner.Height / 2);

            _direction = new Vector2(
                FormMain.Random.Next(cBOUNCE) - (cBOUNCE / 2), 
                FormMain.Random.Next(cBOUNCE) - (cBOUNCE / 2)
            );
        }

        public override void Update()
        {
            // TODO: Add your update code here

            Position += _direction;
            Vector2 t = Position - Offset;

            if ((t.X < 0) || (t.X > _perimeter.Width))
                _direction.X *= -1;
            if ((t.Y < 0) || (t.Y > _perimeter.Height))
                _direction.Y *= -1;
            ;

            base.Update();
        }

        protected virtual void doCollisionOccured(System.Drawing.Point mouse)
        {
            ;
        }
    }
}
