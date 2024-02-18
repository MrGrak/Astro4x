using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astro4x
{
    public static class Input
    {
        public static KeyboardState currentKeyboardState = new KeyboardState();
        public static KeyboardState lastKeyboardState = new KeyboardState();

        public static MouseState currentMouseState = new MouseState();
        public static MouseState lastMouseState = new MouseState();

        //field to track scroll wheel changes
        public static float scrollWheelValue = 0.0f;
        public static byte scrollDirection = 0;

        public static Vector2 cursorPos_Screen = new Vector2(0, 0);
        public static Vector2 cursorPos_World = new Vector2(0, 0);



        static Input() { }

        public static void Update()
        {
            //store + get keyboard and mouse states for this frame
            lastKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();
            lastMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            //calc pos of mouse using scale
            cursorPos_Screen.X = (currentMouseState.X / ScreenManager.SCALE);
            cursorPos_Screen.Y = (currentMouseState.Y / ScreenManager.SCALE);

            
            cursorPos_World.X =
                cursorPos_Screen.X + 
                (int)Camera2D.currentPosition.X - (ScreenManager.GAME_WIDTH 
                / ScreenManager.SCALE);

            cursorPos_World.Y =
                cursorPos_Screen.Y + 
                (int)Camera2D.currentPosition.Y - (ScreenManager.GAME_HEIGHT 
                / ScreenManager.SCALE);
            

        }

        //

        public static bool IsNewLeftClick()
        {
            return (currentMouseState.LeftButton == ButtonState.Pressed &&
                    lastMouseState.LeftButton == ButtonState.Released);
        }

        public static bool IsNewRightClick()
        {
            return (currentMouseState.RightButton == ButtonState.Pressed &&
                    lastMouseState.RightButton == ButtonState.Released);
        }

        public static bool IsScrollWheelMoving()
        {
            if (scrollWheelValue != currentMouseState.ScrollWheelValue)
            {
                //determine which direction we scrolled
                if (currentMouseState.ScrollWheelValue > scrollWheelValue)
                { scrollDirection = 1; } else { scrollDirection = 2; }

                scrollWheelValue = currentMouseState.ScrollWheelValue;
                return true;
            }
            else { return false; }
        }

        public static bool IsNewKeyPress(Keys key)
        {
            return (currentKeyboardState.IsKeyDown(key)
                && lastKeyboardState.IsKeyUp(key));
        }

        public static bool IsKeyDown(Keys key)
        {
            return (currentKeyboardState.IsKeyDown(key));
        }

    }
}
