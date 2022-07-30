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

        private enum ConsoleState
        {
            open,
            closed,
            opening,
            closing,
        }

        private static SpriteSheet consoleSpriteSheet;
        private static Texture2D consoleTexture;

        private static List<ConsoleLine> consoleLines;
        private static string consoleTestLine = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz1234567890";
        private static int textSize = 25;
        private static int textPadding = 5;


        private static int animationSpeed = 20;
        private static bool startAnimating = false;
        private static ConsoleState consoleState = ConsoleState.closed;
        public static bool displayConsole { 
            get { 
                if(consoleState == ConsoleState.closed || (startAnimating == true && consoleState == ConsoleState.opening) ) //extra or rule stops console flashing incorect position
                    return false;
                else
                    return true;
            } 
            set {
                if (value == false && consoleState == ConsoleState.open)
                {
                    startAnimating = true;
                    consoleState = ConsoleState.closing;                    
                }
                if(value == true && consoleState == ConsoleState.closed)
                {
                    startAnimating = true;
                    consoleState = ConsoleState.opening;                    
                }
                
            } 
        }

        private static Rectangle PositionSize = new Rectangle(200, 0, 800, 250);        
        private static Rectangle consoleRectangle = new Rectangle(200, 0, 800, 0);
        

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
                consoleRectangle = PositionSize;
                if(consoleState == ConsoleState.opening)
                {
                    consoleRectangle.Height = 0;
                }
                startAnimating = false;
            }

            if (consoleState == ConsoleState.opening)
            {
                //grow the console till its the same size as PositionSize
                consoleRectangle.Height += animationSpeed;
                if (consoleRectangle.Height > PositionSize.Height)
                {
                    consoleState = ConsoleState.open;
                    consoleRectangle.Height = PositionSize.Height;
                }

            }
            else if(consoleState == ConsoleState.closing)
            {
                consoleRectangle.Height -= animationSpeed;
                if (consoleRectangle.Height <= 0)
                {
                    consoleState = ConsoleState.closed;
                    consoleRectangle.Height = 0;
                }                
            }
        }

        internal static void Draw(SpriteBatch spriteBatch)
        {
            if (displayConsole)
            {
                Rectangle textArea = new Rectangle(consoleRectangle.X + textPadding, consoleRectangle.Y + textPadding, (consoleRectangle.Width - (textPadding*2)), (consoleRectangle.Height - (textPadding * 2)));

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
                Fonts.DrawString(spriteBatch, "console", 22, "top left", textArea, Fonts.Alignment.TopLeft, Color.White);
                Fonts.DrawString(spriteBatch, "console", 22, "top center", textArea, Fonts.Alignment.TopCenter, Color.White);
                Fonts.DrawString(spriteBatch, "console", 22, "top right", textArea, Fonts.Alignment.TopRight, Color.White);

                Fonts.DrawString(spriteBatch, "console", 22, "center left", textArea, Fonts.Alignment.CenterLeft, Color.White);
                Fonts.DrawString(spriteBatch, "console", 22, "center center", textArea, Fonts.Alignment.CenterCenter, Color.White);
                Fonts.DrawString(spriteBatch, "console", 22, "center right", textArea, Fonts.Alignment.CenterRight, Color.White);

                Fonts.DrawString(spriteBatch, "console", 22, "bottom left", textArea, Fonts.Alignment.BottomLeft, Color.White);
                Fonts.DrawString(spriteBatch, "console", 22, "bottom center", textArea, Fonts.Alignment.BottomCenter, Color.White);
                Fonts.DrawString(spriteBatch, "console", 22, "bottom right", textArea, Fonts.Alignment.BottomRight, Color.White);

                spriteBatch.End();
            }
        }

    }
}
