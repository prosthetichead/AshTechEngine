using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AshTechEngine.Input
{
    public class InputManager
    {
        private Game game;

        private KeyboardState currentKeyboardState;
        private KeyboardState previousKeyboardState;

        private GamePadState currentGamePadState;
        private GamePadState previousGamePadState;
        private const float analogLimit = 0.5f;  /// The value of an analog control that reads as a "pressed button"

        private MouseState currentMouseState;
        private MouseState previousMouseState;

        private Point currentMouseScreenPos;
        private Point previousMouseScreenPos;

        private Dictionary<string, InputAction> inputActions;

        private Keys LastPressedKey;
        private char LastPressedCharacter;

        public InputManager(Game game)
        {
            this.game = game;

            LoadInputActions();
        }

        public void Update(GameTime gameTime) 
        {
            
        }

        public void LoadInputActions()
        {
            //try load the actions from JSON.
            if (System.IO.File.Exists("controls.json"))
            {
                //file exits so read it and deserialize into the inputActions List
                string actions = System.IO.File.ReadAllText("controls.json");
                inputActions = JsonConvert.DeserializeObject<Dictionary<string, InputAction>>(actions);
            }
            else //file 
            {
                inputActions = new Dictionary<string, InputAction>();

                //if cant load the file it dosn't exist so setup a default and save it to file for next time
                InputAction inputAction_confirm = new InputAction("confirm", "Confirm", Keys.Enter);
                inputAction_confirm.AddGamePadMap(GamePadButtons.Start, GamePadButtons.ButtonA);
                inputActions.Add(inputAction_confirm.actionId, inputAction_confirm);

                InputAction inputAction_mouseBtn1 = new InputAction("mouseBtn1", "Mouse Button 1", MouseButtons.LeftButton);
                inputActions.Add(inputAction_mouseBtn1.actionId, inputAction_mouseBtn1);
                InputAction inputAction_mouseBtn2 = new InputAction("mouseBtn2", "Mouse Button 2", MouseButtons.RightButton);
                inputActions.Add(inputAction_mouseBtn2.actionId, inputAction_mouseBtn2);

                //write out the file for reading next time
                string json = JsonConvert.SerializeObject(inputActions, Formatting.Indented);
                System.IO.File.WriteAllText("controls.json", json);
            }
        }
    }
}
