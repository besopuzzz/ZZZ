using Microsoft.Xna.Framework.Input;

namespace ZZZ.Framework.Inputing
{
    public static class InputManager
    {
        public static IInputHandle CurrentInput { get; set; }

        private static KeyboardState keyboardState;
        private static KeyboardState oldKeyboardState;


        public static bool IsKeyPressed(Keys key)
        {
            return keyboardState.IsKeyDown(key) & oldKeyboardState.IsKeyUp(key);
        }

        public static bool IsKeyPressing(Keys key)
        {
            return keyboardState.IsKeyDown(key);
        }

    }
}
