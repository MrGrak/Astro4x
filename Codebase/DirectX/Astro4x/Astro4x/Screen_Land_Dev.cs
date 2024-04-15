using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Security;
using System.Windows.Forms;

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
        UI_Button genWorld_moon;

        UI_Button savePlanet;
        UI_Button loadPlanet;

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

            genWorld_moon = new UI_Button(new Vector2(0, 0), "GEN MOON", windowWidth - 20);
            genWorld_moon.text.color = Color.White;

            savePlanet = new UI_Button(new Vector2(0, 0), "SAVE PLANET", windowWidth - 20);
            savePlanet.text.color = Color.White;

            loadPlanet = new UI_Button(new Vector2(0, 0), "LOAD PLANET", windowWidth - 20);
            loadPlanet.text.color = Color.White;


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
                if (Input.IsNewKeyPress(Microsoft.Xna.Framework.Input.Keys.Space))
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
                    if (Input.IsNewLeftClick()) { System_Land.GenMap_Mars(); }
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

                //gen moon
                if (genWorld_moon.button.Contains(Input.cursorPos_Screen))
                {
                    genWorld_moon.text.color = Color.Red;
                    if (Input.IsNewLeftClick()) { System_Land.GenMap_Moon(); }
                }
                else
                { genWorld_moon.text.color = Color.White; }


                //save world
                if (savePlanet.button.Contains(Input.cursorPos_Screen))
                {
                    savePlanet.text.color = Color.Red;
                    if (Input.IsNewLeftClick()) { System_Land.SaveThePlanet("TEST"); }
                }
                else
                { savePlanet.text.color = Color.White; }

                //load world
                if (loadPlanet.button.Contains(Input.cursorPos_Screen))
                {
                    loadPlanet.text.color = Color.Red;
                    if (Input.IsNewLeftClick())
                    {
                        //open file dialog on windows, allow reading of .astro planet file
                        byte[] fileContent;
                        var filePath = string.Empty;

                        using (OpenFileDialog openFileDialog = new OpenFileDialog())
                        {
                            openFileDialog.InitialDirectory = Path.Combine(
                                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Astro4X");

                            openFileDialog.Filter = "astro files (*.astro)|*.txt|All files (*.*)|*.*";
                            openFileDialog.FilterIndex = 2;
                            openFileDialog.RestoreDirectory = true;

                            if (openFileDialog.ShowDialog() == DialogResult.OK)
                            {
                                //Get the path of specified file
                                filePath = openFileDialog.FileName;

                                //Read the contents of the file into a stream
                                var fileStream = openFileDialog.OpenFile();

                                using (BinaryReader reader = new BinaryReader(fileStream))
                                { fileContent = reader.ReadBytes((int)fileStream.Length); }

                                System_Land.LoadThePlanet(fileContent);
                            }
                        }
                        
                    }
                }
                else
                { loadPlanet.text.color = Color.White; }



                
















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
            genWorld_tropical.text.position.X = genWorld_tropical.button.X + 5;
            genWorld_tropical.text.position.Y = genWorld_tropical.button.Y + 0;

            genWorld_rocky.button.X = genWorld_tropical.button.X;
            genWorld_rocky.button.Y = genWorld_tropical.button.Y + 12;
            genWorld_rocky.text.position.X = genWorld_rocky.button.X + 5;
            genWorld_rocky.text.position.Y = genWorld_rocky.button.Y + 0;

            genWorld_oasis.button.X = genWorld_rocky.button.X;
            genWorld_oasis.button.Y = genWorld_rocky.button.Y + 12;
            genWorld_oasis.text.position.X = genWorld_oasis.button.X + 5;
            genWorld_oasis.text.position.Y = genWorld_oasis.button.Y + 0;

            genWorld_artic.button.X = genWorld_oasis.button.X;
            genWorld_artic.button.Y = genWorld_oasis.button.Y + 12;
            genWorld_artic.text.position.X = genWorld_artic.button.X + 5;
            genWorld_artic.text.position.Y = genWorld_artic.button.Y + 0;

            genWorld_moon.button.X = genWorld_artic.button.X;
            genWorld_moon.button.Y = genWorld_artic.button.Y + 12;
            genWorld_moon.text.position.X = genWorld_moon.button.X + 5;
            genWorld_moon.text.position.Y = genWorld_moon.button.Y + 0;





            //parent ui children to gen world
            savePlanet.button.X = bkgWindow.X + 10;
            savePlanet.button.Y = bkgWindow.Y + 330;
            savePlanet.text.position.X = savePlanet.button.X + 5;
            savePlanet.text.position.Y = savePlanet.button.Y + 0;

            loadPlanet.button.X = savePlanet.button.X;
            loadPlanet.button.Y = savePlanet.button.Y + 12;
            loadPlanet.text.position.X = loadPlanet.button.X + 5;
            loadPlanet.text.position.Y = loadPlanet.button.Y + 0;


            #endregion

        }

        public override void Draw()
        {
            ScreenManager.SB.Begin(SpriteSortMode.BackToFront,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                null, null, null, null);

            //draw window, buttons, etc...
            ScreenManager.Draw(bkgWindow, Color.Black, 0.9f, Layers.Debug_Window);
            genWorld_tropical.Draw();
            genWorld_rocky.Draw();
            genWorld_oasis.Draw();
            genWorld_artic.Draw();
            genWorld_moon.Draw();

            savePlanet.Draw();
            loadPlanet.Draw();

            ScreenManager.SB.End();
        }

        //


    }
}