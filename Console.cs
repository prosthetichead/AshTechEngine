using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FontStashSharp;

namespace AshTechEngine
{


    public static class Console
    {

        internal class ConsoleLine
        {
            public string lineText { get; set; }    
            public LineType lineType { get; set; }
        }

        public enum LineType
        {
            normal,
            warning,
            error,
        }

        private static SpriteSheet consoleSpriteSheet;
        private static Texture2D consoleTexture;

        private static bool _displayConsole = true;
        private static List<ConsoleLine> _consoleLines;
        private static bool startAnimating = true;
        private static bool animating = true;

        internal static List<ConsoleLine> consoleLines { get { return _consoleLines; } }
        public static bool displayConsole { get { return _displayConsole; } set { _displayConsole = value; startAnimating = true; } }

        public static Rectangle PositionSize = new Rectangle(200, 0, 800, 250);
        public static int animationSpeed = 1;
        private static Rectangle consoleRectangle = new Rectangle(0, 0, 800, 0);

        internal static void LoadContent(GraphicsDevice graphicsDevice)
        {                
            consoleTexture = Texture2D.FromFile(graphicsDevice, "Content/sprites/AshTechConsole.png");
            consoleSpriteSheet = new SpriteSheet(16,16);
            consoleSpriteSheet.SetTexture(consoleTexture);
            
        }

        public static void WriteLine(string str)
        {
            WriteLine(LineType.normal, str);
        }

        public static void WriteLine(LineType lineType, string str)
        {
            ConsoleLine _consoleLines = new ConsoleLine() { lineType = lineType, lineText = str };
            Debug.WriteLine(str);
        }

        internal static void Update()
        {
            if (startAnimating)
            {
                animating = true;
                consoleRectangle = PositionSize;
                if (displayConsole)
                {
                    consoleRectangle.Height = 0;
                }
                startAnimating = false;
            }
            if (animating)
            {
                if (displayConsole)
                {
                    //grow the console till its the same size as PositionSize
                    consoleRectangle.Height += animationSpeed;
                    if (consoleRectangle.Height > PositionSize.Height)
                    {
                        animating = false;
                        consoleRectangle.Height = PositionSize.Height;
                    }   
                }
                else
                {
                    consoleRectangle.Height -= animationSpeed;
                    if (consoleRectangle.Height > 0)
                    {
                        animating = false;
                        consoleRectangle.Height = PositionSize.Height;
                    }
                }
            }
        }

        internal static void Draw(SpriteBatch spriteBatch)
        {
            if (_displayConsole)
            {
                
                spriteBatch.Begin();
                //top left corner
                consoleSpriteSheet.spriteNumber = 0;
                consoleSpriteSheet.Draw(spriteBatch, new Vector2(consoleRectangle.X, consoleRectangle.Y), new Vector2(0, 0), 0, Color.White, SpriteEffects.None);

                //top
                consoleSpriteSheet.spriteNumber = 1;
                consoleSpriteSheet.Draw(spriteBatch, new Vector2(consoleRectangle.X + 16, consoleRectangle.Y), new Vector2(consoleRectangle.Width - 32, 16), new Vector2(0, 0), 0, Color.White, SpriteEffects.None);
                
                // top right corner
                consoleSpriteSheet.spriteNumber = 2;
                consoleSpriteSheet.Draw(spriteBatch, new Vector2(consoleRectangle.X + consoleRectangle.Width, consoleRectangle.Y), new Vector2(16, 0), 0, Color.White, SpriteEffects.None);

                //left 
                consoleSpriteSheet.spriteNumber = 3;
                consoleSpriteSheet.Draw(spriteBatch, new Vector2(consoleRectangle.X, consoleRectangle.Y + 16), new Vector2(16, consoleRectangle.Height - 32), new Vector2(0, 0), 0, Color.White, SpriteEffects.None);

                //Center 
                consoleSpriteSheet.spriteNumber = 4;
                consoleSpriteSheet.Draw(spriteBatch, new Vector2(consoleRectangle.X + 16, consoleRectangle.Y + 16), new Vector2(consoleRectangle.Width - 32, consoleRectangle.Height - 32), new Vector2(0, 0), 0, Color.White, SpriteEffects.None);

                //right
                consoleSpriteSheet.spriteNumber = 5;
                consoleSpriteSheet.Draw(spriteBatch, new Vector2(consoleRectangle.X + consoleRectangle.Width , consoleRectangle.Y + 16), new Vector2(16, consoleRectangle.Height - 32), new Vector2(16, 0), 0, Color.White, SpriteEffects.None);

                //bottom left corner
                consoleSpriteSheet.spriteNumber = 6;
                consoleSpriteSheet.Draw(spriteBatch, new Vector2(consoleRectangle.X, consoleRectangle.Y + consoleRectangle.Height), new Vector2(0, 16), 0, Color.White, SpriteEffects.None);

                //bottom
                consoleSpriteSheet.spriteNumber = 7;
                consoleSpriteSheet.Draw(spriteBatch, new Vector2(consoleRectangle.X + 16, consoleRectangle.Y + consoleRectangle.Height), new Vector2(consoleRectangle.Width - 32, 16), new Vector2(0, 16), 0, Color.White, SpriteEffects.None);

                //bottom right corner
                consoleSpriteSheet.spriteNumber = 8;
                consoleSpriteSheet.Draw(spriteBatch, new Vector2(consoleRectangle.X  + consoleRectangle.Width, consoleRectangle.Y + consoleRectangle.Height), new Vector2(16, 16), 0, Color.White, SpriteEffects.None);


                //text
                Fonts.DrawString(spriteBatch, "console", 22, "center center", consoleRectangle, Fonts.Alignment.CenterCenter, Color.White);
                Fonts.DrawString(spriteBatch, "console", 22, "top left", consoleRectangle, Fonts.Alignment.TopLeft, Color.White);
                Fonts.DrawString(spriteBatch, "console", 22, "top center", consoleRectangle, Fonts.Alignment.TopCenter, Color.White);
                Fonts.DrawString(spriteBatch, "console", 22, "top right", consoleRectangle, Fonts.Alignment.TopRight, Color.White);

                spriteBatch.End();
            }
        }

    }
}
