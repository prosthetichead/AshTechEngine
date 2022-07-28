# AshTechEngine
Simple easy to use (and copy from!) MonoGame Engine with Screen Managment, Input Managment, Sprite Sheets and More

## Get Started
* Clone the repository
* Add the project to a new MonoGame project solution as an existing project 
  * Right Click the Solution -> Add -> Existing Project   
* Add a Project Reference Dependancy to the MonoGame Project
  * Right click Dependancys -> Add Project Reference -> Tick AshTechEngine

The core of AshTechEngine is the ScreenManager class. Add a ScreenManager object to the main game class and added as a component.
Bellow is a basic example of a Game class using AshTechEngine.
```C# 
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AshTechEngine;

namespace MyGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private ScreenManager screenManager;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            screenManager = new ScreenManager(this, graphics);
            Components.Add(screenManager);
        }

        protected override void Initialize()
        {
            //Add Your Screens to the Screen Manager Here!
            screenManager.AddScreen(new Screen1());
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);            
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);
        }
    }
}
``` 

For the ScreenManager to have something to display a Screen class must be created in your game project and loaded into the screenManager Object
This is an Example of a Screen class which would be added into the ScreenManager in the games main initialize function.
Everything needed can be found in the parent ScreenManager.
```C# 
using System;
using AshTechEngine;
using Microsoft.Xna.Framework;

namespace MyGame
{
    class Screen1 : Screen
    {
        public override void LoadContent()
        {
            base.LoadContent();
            //The parent ScreenManager contains the shared content manager
            ScreenManager.Content.Load<Texture2D>("SPRITE-NAME");
        }
        
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }
        
        public override void HandleInput(GameTime gameTime, InputManager input)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }
        
        public override void Draw(GameTime gameTime)
        {
            // The parent ScreenManager contains the shared SpriteBatch
            var spriteBatch = ScreenManager.SpriteBatch;
            ScreenManager.GraphicsDevice.Clear(Color.Black);            
        }
    }
}

```
