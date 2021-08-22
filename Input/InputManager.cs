using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
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

        private SortedDictionary<string, InputAction> inputActions;

        private Keys LastPressedKey;
        private char LastPressedCharacter;

        public InputManager(Game game)
        {
            this.game = game;

            LoadInputActions();
        }

        public void LoadInputActions()
        {
            //try load the actions from JSON.

        }
    }
}
