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
        UI_Button genWorld_tropical;
        UI_Button genWorld_rocky;
        UI_Button genWorld_oasis;
        UI_Button genWorld_artic;

        UI_Button saveWorld;


        public Screen_Land_Dev()
        {
            Name = "LAND MENU";

            bkgWindow = new Rectangle(
                ScreenManager.GAME_WIDTH, 0, windowWidth, ScreenManager.GAME_HEIGHT);

            genWorld_tropical = new UI_Button(new Vector2(0, 0), "GEN TROPICAL", windowWidth - 20);
            genWorld_tropical.text.color = Color.White;

            genWorld_rocky = new UI_Button(new Vector2(0, 0), "GEN ROCKY", windowWidth - 20);
            genWorld_rocky.text.color = Color.White;

            genWorld_oasis = new UI_Button(new Vector2(0, 0), "GEN OASIS", windowWidth - 20);
            genWorld_oasis.text.color = Color.White;

            genWorld_artic = new UI_Button(new Vector2(0, 0), "GEN ARTIC", windowWidth - 20);
            genWorld_artic.text.color = Color.White;
            
            saveWorld = new UI_Button(new Vector2(0, 0), "SAVE WORLD", windowWidth - 20);
            saveWorld.text.color = Color.White;
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


                #region handle button interactions

                //gen tropical
                if (genWorld_tropical.button.Contains(Input.cursorPos_Screen))
                {
                    genWorld_tropical.text.color = Color.Red;
                    if (Input.IsNewLeftClick()) { System_Land.GenMap_Tropical(); }
                }
                else
                { genWorld_tropical.text.color = Color.White; }

                //gen rocky
                if (genWorld_rocky.button.Contains(Input.cursorPos_Screen))
                {
                    genWorld_rocky.text.color = Color.Red;
                    if (Input.IsNewLeftClick()) { System_Land.GenMap_Rocky(); }
                }
                else
                { genWorld_rocky.text.color = Color.White; }

                //gen oasis
                if (genWorld_oasis.button.Contains(Input.cursorPos_Screen))
                {
                    genWorld_oasis.text.color = Color.Red;
                    if (Input.IsNewLeftClick()) { System_Land.GenMap_Oasis(); }
                }
                else
                { genWorld_oasis.text.color = Color.White; }

                //gen artic
                if (genWorld_artic.button.Contains(Input.cursorPos_Screen))
                {
                    genWorld_artic.text.color = Color.Red;
                    if (Input.IsNewLeftClick()) { System_Land.GenMap_Artic(); }
                }
                else
                { genWorld_artic.text.color = Color.White; }




                //save world
                if (saveWorld.button.Contains(Input.cursorPos_Screen))
                {
                    saveWorld.text.color = Color.Red;
                    if (Input.IsNewLeftClick()) { System_Land.SaveThePlanet("TEST"); }
                }
                else
                { saveWorld.text.color = Color.White; }

                #endregion

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
            

            #region match ui elements to window

            //parent gen world buttons to top of window
            genWorld_tropical.button.X = bkgWindow.X + 10;
            genWorld_tropical.button.Y = bkgWindow.Y + 10;
            genWorld_tropical.text.position.X = genWorld_tropical.button.X + 10;
            genWorld_tropical.text.position.Y = genWorld_tropical.button.Y + 0;

            genWorld_rocky.button.X = genWorld_tropical.button.X;
            genWorld_rocky.button.Y = genWorld_tropical.button.Y + 12;
            genWorld_rocky.text.position.X = genWorld_rocky.button.X + 10;
            genWorld_rocky.text.position.Y = genWorld_rocky.button.Y + 0;

            genWorld_oasis.button.X = genWorld_rocky.button.X;
            genWorld_oasis.button.Y = genWorld_rocky.button.Y + 12;
            genWorld_oasis.text.position.X = genWorld_oasis.button.X + 10;
            genWorld_oasis.text.position.Y = genWorld_oasis.button.Y + 0;

            genWorld_artic.button.X = genWorld_oasis.button.X;
            genWorld_artic.button.Y = genWorld_oasis.button.Y + 12;
            genWorld_artic.text.position.X = genWorld_artic.button.X + 10;
            genWorld_artic.text.position.Y = genWorld_artic.button.Y + 0;


            //parent ui children to gen world
            saveWorld.button.X = genWorld_artic.button.X;
            saveWorld.button.Y = genWorld_artic.button.Y + 12;
            saveWorld.text.position.X = saveWorld.button.X + 10;
            saveWorld.text.position.Y = saveWorld.button.Y + 0;




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
            genWorld_tropical.Draw();
            genWorld_rocky.Draw();
            genWorld_oasis.Draw();
            genWorld_artic.Draw();

            saveWorld.Draw();

            ScreenManager.SB.End();
        }

        //


    }
}