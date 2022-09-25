using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using AshTechEngine.InputManagment;
using AshTechEngine.ContentManagment;
using AshTechEngine.DebugTools;

namespace AshTechEngine.ScreenDisplay
{
    /// <summary>
    /// The screen manager is a component which manages one or more GameScreen
    /// instances. It maintains a stack of screens, calls their Update and Draw
    /// methods at the appropriate times, and automatically routes input to the
    /// topmost active screen.
    /// </summary>
    public class ScreenManager : DrawableGameComponent
    {
        List<Screen> screens = new List<Screen>();
        List<Screen> screensToUpdate = new List<Screen>();
        List<Screen> screensToDraw = new List<Screen>();

        InputManager input;

        GameSettings gameSettings;

        GraphicsDeviceManager graphics;

        IGraphicsDeviceService graphicsDeviceService;

        ContentLoader contentLoader;
        SpriteBatch spriteBatch;
        FrameRate frameRate;


        /// <summary>
        /// access to our Game instance
        /// </summary>
        new public Game Game
        {
            get { return base.Game; }
        }

        /// <summary>
        /// access to our graphics device
        /// </summary>
        new public GraphicsDevice GraphicsDevice
        {
            get { return base.GraphicsDevice; }
        }

        public GraphicsDeviceManager Graphics
        {
            get { return graphics; }
        }

        /// <summary>
        /// The Input Manager
        /// </summary>
        public InputManager Input
        {
            get { return input; }
        }

        public GameSettings GameSettings
        {
            get { return gameSettings; }
        }

        /// <summary>
        /// A content loader helper!
        /// Load Content from Raw Files or Resorces RESX files 
        /// </summary>
        public ContentLoader ContentLoader
        {
            get { return contentLoader; }
        }

        /// <summary>
        /// SpriteBatch shared by all the screens.
        /// </summary>
        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
        }


        public ScreenManager(Game game, GraphicsDeviceManager graphics) : base(game)
        {
            this.graphics = graphics; // we need the graphics device manager 

            input = new InputManager(game);

            graphicsDeviceService = (IGraphicsDeviceService)game.Services.GetService(typeof(IGraphicsDeviceService));
            if (graphicsDeviceService == null)
                throw new InvalidOperationException("No graphics device service.");

            frameRate = new FrameRate(5);

            gameSettings = new GameSettings(this);

            contentLoader = new ContentLoader(GraphicsDevice);
        }


        /// <summary>
        /// Load your graphics content.
        /// </summary>
        protected override void LoadContent()
        {
            // Load content belonging to the screen manager.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //load the default console font
            contentLoader.BitmapFontFromResource(Content.AshTechResources.ResourceManager, "console", "pixellocale_fnt", "pixellocale_png", Point.Zero);

            // Tell each of the screens to load their content.
            foreach (Screen screen in screens)
            {
                screen.LoadContent();
            }

            //setup the console
            ConsoleAsh.LoadContent(ContentLoader, Game);

            //add some commands to the console 
            ConsoleAsh.AddConsoleCommand(new ConsoleAsh.ConsoleCommand("fr", "display the current frame rate", "displays the current frame rate to the console", a => { ConsoleAsh.WriteLine(ConsoleAsh.LineType.warning, frameRate.framerate + " FPS"); }));

        }

        /// <summary>
        /// Unload your graphics content.
        /// </summary>
        protected override void UnloadContent()
        {
            // Unload content belonging to the screen manager.
            //content.Unload();

            // Tell each of the screens to unload their content, if they have any
            foreach (Screen screen in screens)
            {
                screen.UnloadContent();
            }
        }



        /// <summary>
        /// Allows each screen to run logic.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            input.Update();

            ConsoleAsh.Update();

            //check for input ` to open and close the console
            if (input.IsKeyTriggered(Microsoft.Xna.Framework.Input.Keys.OemTilde))
            {
                ConsoleAsh.displayConsole = !ConsoleAsh.displayConsole;
            }

            screensToUpdate.Clear();
            foreach (Screen screen in screens)
                screensToUpdate.Add(screen);

            bool otherScreenHasFocus = !Game.IsActive;  // first screen we just check if the game has focus on the OS level.
            bool coveredByOtherScreen = false;


            while (screensToUpdate.Count > 0)
            {
                Screen screen = screensToUpdate[screensToUpdate.Count - 1];
                screensToUpdate.RemoveAt(screensToUpdate.Count - 1);

                screen.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

                if (screen.ScreenState == ScreenState.TransitionOn || screen.ScreenState == ScreenState.Active)
                {
                    if (otherScreenHasFocus == false)
                    {
                        screen.HandleInput(gameTime, input);
                        otherScreenHasFocus = true; //now no other screen can run its input updates
                    }
                    if (screen.IsPopup == false)
                        coveredByOtherScreen = true;
                }
            }
        }

        /// <summary>
        /// Tells each screen to draw itself.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
#if DEBUG
            frameRate.Update(gameTime);
            Game.Window.Title = "Debug Mode :: " + frameRate.framerate + " fps ";
#endif



            screensToDraw.Clear();
            foreach (Screen screen in screens)
                screensToDraw.Add(screen);

            foreach (Screen screen in screensToDraw)
            {
                if (screen.ScreenState == ScreenState.Hidden)
                    continue;
                screen.Draw(gameTime);
            }

            //draw the console
            ConsoleAsh.Draw(SpriteBatch);
        }

        /// <summary>
        /// Adds a new screen to the screen manager.
        /// </summary>
        public void AddScreen(Screen screen)
        {
            screen.ScreenManager = this;

            if (graphicsDeviceService != null && graphicsDeviceService.GraphicsDevice != null)
            {
                screen.LoadContent(); //if graphicsDeviceService is running then load content!
            }

            screens.Add(screen);
        }

        /// <summary>
        /// Removes a screen from the screen manager.
        /// use screen exit rather then this as this does not do transitions
        /// </summary>
        public void RemoveScreen(Screen screen)
        {
            // If we have a graphics device, tell the screen to unload content.
            if (graphicsDeviceService != null &&
                graphicsDeviceService.GraphicsDevice != null)
            {
                screen.UnloadContent();
            }

            screens.Remove(screen);
            screensToUpdate.Remove(screen);
        }
    }
}