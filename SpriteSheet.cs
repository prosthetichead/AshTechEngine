using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace AshTechEngine
{
    public class SpriteSheet
    {
        private Texture2D texture;
        private int singleSpriteWidth;
        private int singleSpriteHeight;
        /// <summary>
        /// which sprite on the sheet to draw defaults to 0
        /// </summary>
        public int spriteNumber = 0;

        public SpriteSheet(int singleSpriteWidth, int singleSpriteHeight)
        {
            this.singleSpriteWidth = singleSpriteWidth;
            this.singleSpriteHeight = singleSpriteHeight;
        }

        public void LoadTexture(ContentManager content, string textureName)
        {
            texture = content.Load<Texture2D>(textureName);
        }

        public void SetTexture(Texture2D texture)
        {
            this.texture = texture;
        }

        private Rectangle GetSourceRectangle(int spriteNumber)
        {
            int rectangleX = spriteNumber % (texture.Width / singleSpriteWidth);
            int rectangleY = spriteNumber / (texture.Width / singleSpriteWidth);

            return new Rectangle(rectangleX * singleSpriteWidth, rectangleY * singleSpriteHeight, singleSpriteWidth, singleSpriteWidth);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, Vector2 origin, float depth, Color color, SpriteEffects spriteEffect)
        {
            spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, singleSpriteWidth, singleSpriteHeight), GetSourceRectangle(spriteNumber), color, 0f, origin, spriteEffect, depth);
        }
    }
}
