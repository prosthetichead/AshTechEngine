using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace AshTechEngine.Input
{
    public enum GamePadButtons
    {
        // Center
        Home,
        Start,
        Back,
        // D-Pad
        Up,
        Left,
        Down,
        Right,
        // Face buttons
        ButtonY,
        ButtonX,
        ButtonA,
        ButtonB,
        // Shoulder buttons
        LeftShoulder,
        RightShoulder,
        // Triggers
        LeftTrigger,
        RightTrigger,
        // Left Stick
        LeftStick,
        LeftStickUp,
        LeftStickLeft,
        LeftStickDown,
        LeftStickRight,
        // Right Stick
        RightStick,
        RightStickUp,
        RightStickLeft,
        RightStickDown,
        RightStickRight
    }
    public enum MouseButtons
    {
        LeftButton,
        RightButton,
        MiddleButton,
        ScrollWheelUp,
        ScrollWheelDown,
    }

    public class InputAction
    {
        public string actionId;
        public string actionName;

        public List<GamePadButtons> gamePadButtons = new List<GamePadButtons>();
        public List<Keys> keyboardKeys = new List<Keys>();
        public List<MouseButtons> mouseButtons = new List<MouseButtons>();

        [Newtonsoft.Json.JsonConstructor]
        public InputAction(string actionId, string actionName)
        {
            this.actionId = actionId;
            this.actionName = actionName;
        }
        public InputAction(string actionId, string actionName, params GamePadButtons[] gamePadButtons)
        {
            this.actionId = actionId;
            this.actionName = actionName;
            this.gamePadButtons.AddRange(gamePadButtons);
        }
        public InputAction(string actionId, string actionName, params Keys[] keyboardKeys)
        {
            this.actionId = actionId;
            this.actionName = actionName;
            this.keyboardKeys.AddRange(keyboardKeys);
        }
        public InputAction(string actionId, string actionName, params MouseButtons[] mouseButtons)
        {
            this.actionId = actionId;
            this.actionName = actionName;
            this.mouseButtons.AddRange(mouseButtons);
        }
        public void AddGamePadMap(params GamePadButtons[] gamePadButtons)
        {
            this.gamePadButtons.AddRange(gamePadButtons);
        }

        public void AddKeyboardKeyMap(params Keys[] keyboardKeys)
        {
            this.keyboardKeys.AddRange(keyboardKeys);
        }

        public void AddMouseMap(params MouseButtons[] mouseButtons)
        {
            this.mouseButtons.AddRange(mouseButtons);
        }
    }
}
