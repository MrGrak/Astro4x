using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astro4x
{
    public class Screen_Land_Dev : Screen
    {
        //menu background window
        public int windowWidth = 100;
        public Rectangle bkgWindow;

        //buttons
        UI_Button genWorld;



        public Screen_Land_Dev()
        {
            Name = "LAND MENU";

            bkgWindow = new Rectangle(
                ScreenManager.GAME_WIDTH, 0, windowWidth, ScreenManager.GAME_HEIGHT);

            genWorld = new UI_Button(new Vector2(0, 0), "GEN WORLD", windowWidth - 20);
            genWorld.text.color = Color.White;

            
        }

        public override void Open()
        {
            displayState = DisplayState.OPENING;

            //set window offscreen for open
            bkgWindow.X = ScreenManager.GAME_WIDTH;
        }

        public override void Close()
        {
            displayState = DisplayState.CLOSING;
        }

        public override void HandleInput()
        {
            if (displayState == DisplayState.OPENED)
            {

                if (Input.IsNewKeyPress(Keys.Space))
                { Close(); }
            }
        }

        public override void Update()
        {


            #region handle display states

            if (displayState == DisplayState.OPENING)
            {
                //animate in
                bkgWindow.X -= 10;

                if (bkgWindow.X < ScreenManager.GAME_WIDTH - windowWidth)
                {
                    bkgWindow.X = ScreenManager.GAME_WIDTH - windowWidth;
                    displayState = DisplayState.OPENED;
                }
            }
            else if (displayState == DisplayState.OPENED)
            {
                //interaction
            }
            else if (displayState == DisplayState.CLOSING)
            {
                //animate out
                bkgWindow.X += 10;

                if (bkgWindow.X > ScreenManager.GAME_WIDTH)
                {
                    bkgWindow.X = ScreenManager.GAME_WIDTH;
                    displayState = DisplayState.CLOSED;
                }

            }
            else if (displayState == DisplayState.CLOSED)
            {
                //exit screen
                ScreenManager.RemoveScreen(this);
            }

            #endregion

            

            #region handle button interactions

            if (genWorld.button.Contains(Input.cursorPos_Screen))
            {
                genWorld.text.color = Color.Red;

                if (Input.IsNewLeftClick())
                { System_Land.GenMap(); }
            }
            else
            {
                genWorld.text.color = Color.White;
            }

            #endregion



            #region match ui elements to window

            genWorld.button.X = bkgWindow.X + 10;
            genWorld.text.position.X = genWorld.button.X + 10;

            genWorld.button.Y = bkgWindow.Y + 10;
            genWorld.text.position.Y = genWorld.button.Y + 0;

            #endregion




        }

        public override void Draw()
        {
            ScreenManager.SB.Begin(SpriteSortMode.BackToFront,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                null, null, null, null);

            //draw window, buttons, etc...
            ScreenManager.Draw(bkgWindow, Color.Black, 0.5f, Layers.Debug_Window);
            genWorld.Draw();

            
            ScreenManager.SB.End();
        }

        //


    }
}