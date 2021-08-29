using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AshTechEngine
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
                return (numerator / currentFrametimes);
            }
        }

        public FrameRate(int oldFrameWeight)
        {
            numerator = oldFrameWeight;
            weight = (double)oldFrameWeight / ((double)oldFrameWeight - 1d);
        }

        public void Update(GameTime gameTime)
        {
            var timeSinceLastFrame = gameTime.ElapsedGameTime.TotalSeconds;
            currentFrametimes = currentFrametimes / weight;
            currentFrametimes += timeSinceLastFrame;
        }
        
    }
    
}
