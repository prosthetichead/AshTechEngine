using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AshTechEngine.DebugTools
{
    class FrameRate
    {
        double currentFrametimes;
        double weight;
        int numerator;

        public double framerate
        {
            get
            {
                return numerator / currentFrametimes;
            }
        }

        public FrameRate(int oldFrameWeight)
        {
            numerator = oldFrameWeight;
            weight = oldFrameWeight / (oldFrameWeight - 1d);
        }

        public void Update(GameTime gameTime)
        {
            var timeSinceLastFrame = gameTime.ElapsedGameTime.TotalSeconds;
            currentFrametimes = currentFrametimes / weight;
            currentFrametimes += timeSinceLastFrame;
        }

    }

}
