using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace AshTechEngine
{
    public static class ConsoleAsh
    {

        public class ConsoleLine
        {
            public string lineText { get; set; }
            public LineType lineType { get; set; }
            public DateTime dateTime { get; }

            public ConsoleLine()
            {
                dateTime = DateTime.Now;
            }
            public ConsoleLine(string lineText, LineType lineType)
            {
                this.lineText = lineText;
                this.lineType = lineType;
                dateTime = DateTime.Now;
            }

            public Color[] lineColor
            {
                get
                {
                    switch (lineType)
                    {
                        case LineType.normal:
                            return new Color[] { Color.LightGray };
                        case LineType.warning:
                            return new Color[] { Color.Orange };
                        case LineType.error:
                            return new Color[] { Color.MonoGameOrange };
                        case LineType.command:
                            return new Color[] { Color.LimeGreen };
                        default:
                            return new Color[] { Color.LightGray };
                    }
                }
            }
        }

        public class ConsoleCommand
        {
            public string command { get; set; }
            public string description { get; set; }
            public string helpText { get; set; }
            public Action<string[]> commandAction { get; set; }
            public ConsoleCommand(string command, string description, string helpText, Action<string[]> commandAction)
            {
                this.command = command;
                this.description = description;
                this.helpText = helpText;
                this.commandAction = commandAction;
            }
        }

        public enum LineType
        {
            normal,
            warning,
            error,
            command
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

        private static List<ConsoleLine> consoleLines = new List<ConsoleLine>();
        private static List<ConsoleCommand> consoleCommands = new List<ConsoleCommand>();

        private static string consoleTestLine = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz1234567890";
        private static int textPadding = 10;
        private static int lineHeight = 0;
        private static int fontSize = 16; 

        private static int animationSpeed = 18;
        private static int timeSinceCursorFlash = 0;
        private static int timeSinceCursorSpeed = 25;
        private static bool displayCursor;
        private static string cursor = "|";
        private static string commandString = "";
        private static List<string> previousCommandStrings = new List<string>();
        private static int previousCommandIndex = 0;
        private static bool startAnimating = false;
        private static ConsoleState consoleState = ConsoleState.closed;

        private static bool textInput { get { if (consoleState == ConsoleState.open) return true; else return false; } }

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

        public static Rectangle PositionSize = new Rectangle(50, 0, 900, 350);        
        private static Rectangle consoleRectangle = new Rectangle(0, 0, 0, 0);
        

        internal static void LoadContent(ContentLoader content, Game game )
        {                
            consoleTexture = content.Texture2DFromResource(AshTechResources.ResourceManager, "AshTechConsole_png");
            consoleSpriteSheet = new SpriteSheet(16,16);
            consoleSpriteSheet.SetTexture(consoleTexture);

            ConsoleLine consoleLine = new ConsoleLine() { lineType = LineType.normal, lineText = "AshTechEngine Console <(^.^)>" };
            consoleLines.Add(consoleLine);
            consoleLine = new ConsoleLine() { lineType = LineType.normal, lineText = "== enter ? for list of avalable commands ==" };
            consoleLines.Add(consoleLine);
            
            //setup listener for text input
            game.Window.TextInput += Window_TextInput;
            game.Window.KeyUp += Window_KeyUp;

            //setup default commands
            consoleCommands.Add(new ConsoleCommand("?", "This Help Text. [ ? [page number] ] to get additinal pages", "Help! I need somebody. Help! Not just anybody. Help! You know I need someone. Help!", a => {
                
                int page = 1;
                int limitPerPage = 10;
                int maxPages = (int)Math.Ceiling((double)consoleCommands.Count / limitPerPage) ;

                if (a.Length > 0)
                {
                    if( int.TryParse(a[0], out page))
                    {
                        page = Math.Clamp(page, 0, maxPages);
                    }
                    else
                    {
                        WriteLine(LineType.error, "error parsing page number argument  " + a[0]);
                    }
                }
                WriteLine(" -- Commands Page " + (page) + " / " + maxPages + " -- ");
                foreach (var command in consoleCommands.Skip((page - 1) * limitPerPage).Take(limitPerPage).ToList())
                {                    
                    WriteLine("[ " + command.command + " ]  ->  " + command.description);                   
                }
                WriteLine(" -- for additonal command help enter COMMAND ? -- ");
            }));

            consoleCommands.Add(new ConsoleCommand("clr", "Clear the console window", "Simply clears the console window of all previous lines", a => {
                consoleLines.Clear();                
            }));

        }

        private static void Window_KeyUp(object sender, InputKeyEventArgs e)
        {
            if (textInput)
            {
                var key = e.Key;
                if (key == Keys.Up)
                {
                    if (previousCommandStrings.Count > 0)
                    {
                        previousCommandIndex++;
                        previousCommandIndex = Math.Clamp(previousCommandIndex, 0, previousCommandStrings.Count-1);
                        string newCommandString = previousCommandStrings[previousCommandIndex];
                        commandString = newCommandString;
                    }
                }
                else if (key == Keys.Down)
                {
                    if (previousCommandStrings.Count > 0)
                    {
                        previousCommandIndex--;
                        previousCommandIndex = Math.Clamp(previousCommandIndex, 0, previousCommandStrings.Count-1);
                        string newCommandString = previousCommandStrings[previousCommandIndex];
                        commandString = newCommandString;
                    }
                }
            }
        }

        private static void Window_TextInput(object sender, TextInputEventArgs e)
        {
            if (textInput)
            {
                char character = e.Character;
                var key = e.Key;
                if (key == Keys.Back)
                {
                    if(commandString.Length > 0)
                        commandString = commandString.Remove(commandString.Length - 1);
                }
                else if(key == Keys.Enter)
                {
                    if (commandString.Length > 0)
                    {
                        previousCommandIndex = -1;
                        WriteLine(LineType.command, ">" + commandString);
                        ExecuteCommandString();
                        previousCommandStrings.Insert(0,commandString);
                        commandString = "";

                    }
                }
                else if(key != Keys.OemTilde)
                {
                    commandString += character;
                }
            }
        }

        public static void AddConsoleCommand(ConsoleCommand consoleCommand)
        {
            consoleCommands.Add(consoleCommand);
        }

        private static void ExecuteCommandString()
        {
            var commandArray = commandString.Trim().Split(' ');
            string command = "";
            if (commandArray.Length > 0)
            {
                command = commandArray[0];
                commandArray = commandArray.Skip(1).ToArray();
                if ( consoleCommands.Any(w=>w.command == command))
                {
                    var consoleCommand = consoleCommands.FirstOrDefault(w => w.command == command);
                    //get the arguments if there is any check if the first one is a ? if it is display the help dont execute
                    if(commandArray.Length > 0) 
                    {
                        if (commandArray[0] == "?") { WriteLine(consoleCommand.helpText); return; }                        
                    }
                    //Execute the Command
                    consoleCommand.commandAction(commandArray);
                }
                else
                {
                    WriteLine(LineType.error, "Uknown Command. Enter ? for available commands.");
                }
            }
        }

        public static void WriteLine(string str)
        {
            WriteLine(LineType.normal, str);
        }
        public static void WriteLine(LineType lineType, string str)
        {
            ConsoleLine consoleLine = new ConsoleLine() { lineType = lineType, lineText = str };
            consoleLines.Add(consoleLine);
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
                timeSinceCursorFlash++;
                if(timeSinceCursorFlash >= timeSinceCursorSpeed)
                {
                    timeSinceCursorFlash = 0;
                    displayCursor = !displayCursor;
                }

                if (lineHeight == 0)
                {
                    var measureString = Fonts.MeasureString("console", fontSize, consoleTestLine);
                    lineHeight = (int)measureString.Y ;

                }
                //Rectangle textArea = new Rectangle(consoleRectangle.X + textPadding, consoleRectangle.Y + textPadding, (consoleRectangle.Width - (textPadding*2)), (consoleRectangle.Height - (textPadding * 2)));
                int numberOfLines = MathHelper.Max(((consoleRectangle.Height - textPadding*2) / lineHeight)-1, 0);
                
                

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

                int lineCount = numberOfLines;
                for (int i = consoleLines.Count - 1; i >= 0 && i >= (consoleLines.Count - 1) - numberOfLines; i--)
                {
                    var line = consoleLines[i];
                    
                    Fonts.DrawString(spriteBatch, "console", fontSize, consoleLines[i].lineText, new Rectangle(consoleRectangle.X + textPadding, consoleRectangle.Y + (lineHeight * lineCount), consoleRectangle.Width - (textPadding*2), lineHeight) , Fonts.Alignment.CenterLeft, line.lineColor );
                    lineCount--;
                }

                Fonts.DrawString(spriteBatch, "console", fontSize, ">" + commandString + (displayCursor ? cursor : ""), new Vector2(consoleRectangle.X + textPadding, consoleRectangle.Height - (lineHeight+lineHeight/2)), Color.LimeGreen);

                spriteBatch.End();
            }
        }

    }
}
