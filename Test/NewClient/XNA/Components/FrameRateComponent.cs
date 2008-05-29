using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace Laan.Riskier.Client.TileMap
{
    public class FrameRateComponent : GameComponent
    {
        private TimeSpan _oneSecond = new TimeSpan(0, 0, 0, 1);
        private float _fps = 0;
        private int _frameCount = 0;
        private DateTime _lastFPSTime;

        public FrameRateComponent(Game game) : base(game) { }

        public override void Initialize()
        {
            base.Initialize();
            _lastFPSTime = DateTime.Now;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _frameCount++;

            DateTime currentTime = DateTime.Now;
            if (currentTime - _lastFPSTime > _oneSecond)
            {
                _fps = _frameCount;
                _frameCount = 0;

                if (OnFPSUpdate != null)
                    OnFPSUpdate(this, EventArgs.Empty);

                _lastFPSTime = currentTime;
            }
        }

        public float FPS
        {
            get { return _fps; }
        }

        public event EventHandler OnFPSUpdate;
    }
}
