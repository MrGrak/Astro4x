using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Astro4x
{
    public enum DisplayState { OPENING, OPENED, CLOSING, CLOSED }

    public abstract class Screen
    {
        public String Name = "Base Screen";
        public DisplayState displayState;

        public Screen() { }
        public virtual void Open() { }
        public virtual void Close() { }
        public virtual void HandleInput() { }
        public virtual void Update() { }
        public virtual void Draw() { }
    }

    public static class ScreenManager
    {
        //game references
        public static GraphicsDeviceManager GDM;
        public static ContentManager CM;
        public static SpriteBatch SB;
        public static RenderTarget2D RT2D;
        public static Game1 GAME;
        public static Random RAND = new Random();

        public static int GAME_WIDTH = 640;
        public static int GAME_HEIGHT = 360;
        public static int WINDOW_WIDTH = 1280;
        public static int WINDOW_HEIGHT = 720;

        public static int SCALE = 2;

        //manager references
        public static List<Screen> screens = new List<Screen>();
        public static Screen activeScreen;
        
        public static Text Text_Debug_LeftTop;
        //public static Text Text_Debug_FollowMouse;
        public static Stopwatch timer = new Stopwatch();
        public static long ticks = 0;

        //screen references
        public static Screen_Land Land;
        public static Screen_Land_Dev LandMenu;
        //...


        public static void Constructor()
        {
            Text_Debug_LeftTop = new Text("init", new Vector2(8, 5), Color.White);
            Text_Debug_LeftTop.layer = Layers.Debug_Text;
            
            //construct all screen instances
            Land = new Screen_Land();
            LandMenu = new Screen_Land_Dev();




            //boot to this screen first
            AddScreen(Land);
        }

        public static void AddScreen(Screen screen)
        {
            screens.Add(screen);
            screen.Open();
        }

        public static void RemoveScreen(Screen screen)
        {
            screens.Remove(screen);
        }

        public static void ExitAllAndLoad(Screen screenToLoad)
        {
            while (screens.Count > 0)
            { screens.Remove(screens[0]); }
            
            AddScreen(screenToLoad);
            activeScreen = screenToLoad;

            screenToLoad.Open();
        }

        public static void Update()
        {
            timer.Restart();

            Input.Update();

            if (screens.Count > 0) //update screen stack
            {   //the only 'active screen' is the last one (top one)
                activeScreen = screens[screens.Count - 1];
                //update active screen
                activeScreen.HandleInput();
                activeScreen.Update();
            }
        }
        
        public static void Draw()
        {
            //draw to render target
            GDM.GraphicsDevice.SetRenderTarget(RT2D);
            GDM.GraphicsDevice.Clear(Color.Black);
            
            //each screen handles opening and closing spriteBatch
            //allows screens to use camera matrices to draw world views
            foreach (Screen screen in screens) { screen.Draw(); }
            
            //draw debug
            SB.Begin(SpriteSortMode.Deferred,
                BlendState.AlphaBlend, SamplerState.PointClamp);

            //collect and draw a fps
            timer.Stop();

            //for some reason ms isn't working?
            //Text_Debug.text = timer.ElapsedMilliseconds.ToString("00.00000");
            Text_Debug_LeftTop.text = (timer.ElapsedTicks * 0.0001f).ToString("0.0000") + " MS";
            Text_Debug_LeftTop.text += "\n" + activeScreen.Name;
            Text_Debug_LeftTop.text += " : " + activeScreen.displayState;
            Text_Debug_LeftTop.text += "\nTILES: " + System_Land.totalTiles;
            //Text_Debug.text += "\nSCROLL WHL: " + Input.scrollWheelValue;
            
            
            //follow mouse around
            //Text_Debug_FollowMouse.position.X = Input.cursorPos_Screen.X + 8;
            //Text_Debug_FollowMouse.position.Y = Input.cursorPos_Screen.Y;

            //draw debg info only at 1.0 zoom
            if (Camera2D.targetZoom == 1.0f)
            {
                Draw(Text_Debug_LeftTop);
                
            }
            

            SB.End();
            
            //set target to window
            GDM.GraphicsDevice.SetRenderTarget(null);

            //draw base rt to window
            SB.Begin(SpriteSortMode.Deferred,
                BlendState.Opaque, SamplerState.PointClamp);

            SB.Draw(RT2D, new Rectangle(0, 0, 
                WINDOW_WIDTH, WINDOW_HEIGHT),
                Color.White);

            SB.End();
        }

        //

        public static void Draw(Text Text)
        {
            SB.DrawString(Text.font, Text.text, Text.position,
                Text.color * Text.alpha, Text.rotation, Vector2.Zero,
                Text.scale, SpriteEffects.None, Text.layer);
        }

        public static void Draw(SpriteStruct sprite, Texture2D Tex)
        {
            Rectangle DrawRec;
            DrawRec.X = sprite.draw_x;
            DrawRec.Y = sprite.draw_y;
            DrawRec.Width = sprite.draw_width;
            DrawRec.Height = sprite.draw_height;

            SB.Draw(
                Tex,
                new Vector2(sprite.X, sprite.Y), 
                DrawRec,
                Color.White * sprite.alpha,
                0.0f,
                new Vector2(0, 0),
                1.0f,
                SpriteEffects.None,
                sprite.layer);
        }

        public static void Draw(Rectangle rec, Color color, float alpha, float layer)
        {
            SB.Draw(
                Assets.lineTexture,
                new Vector2(rec.X, rec.Y),
                new Rectangle(0, 0, rec.Width, rec.Height),
                color * alpha,
                0.0f,
                new Vector2(0, 0),
                1.0f,
                SpriteEffects.None,
                layer);
        }

    }
}