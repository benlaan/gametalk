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

    public abstract partial class Sprite : Component
    {

        public    Vector2     Position;
        public    string      FileName;
        public    Texture2D   Image;

        protected SpriteBatch _spriteBatch;

        public Sprite(System.Windows.Forms.Control owner, GraphicsDevice device) : base (owner, device)
        {
            Position = new Vector2(0, 0);
        }

        public override void Initialise() 
        {
            base.Initialise();

            if (FileName != "")
                Image = Texture2D.FromFile(Device, FileName);
            else
                Image = GetImage();

            _spriteBatch = new SpriteBatch(Device);
        }

        protected virtual Texture2D GetImage()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void Update()
        {
            // TODO: Add your update code here
        }

        public override void Draw()
        {
            _spriteBatch.Begin();
            _spriteBatch.Draw(Image, Position - Offset, Color.White);
            _spriteBatch.End();
        }

        protected override Rectangle GetBoundingBox()
        {
            Vector2 relativePosition = Position - Offset;
            return new Rectangle((int)relativePosition.X, (int)relativePosition.Y, Image.Width, Image.Height);
        }
    }
}
