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
    public class Screen_Land : Screen
    {


        public byte heightOffset = 8;

        public SpriteStruct highliteTile;
        public SpriteStruct selectedTile;
        public int selectedTileID;





        public Screen_Land()
        {
            Name = "LAND";
            
            //setup highlight tile sprite
            highliteTile = new SpriteStruct();
            highliteTile.draw_width = 32;
            highliteTile.draw_height = 32;
            highliteTile.draw_x = (byte)(1 * highliteTile.draw_width);
            highliteTile.draw_y = (byte)(0 * highliteTile.draw_height);
            highliteTile.alpha = 1.0f;
            highliteTile.layer = Layers.Land_UI;

            //copy same properties to selected tile sprite
            selectedTile = highliteTile;
            //highlight tile needs to be lighter
            highliteTile.alpha = 0.5f;

            highliteTile.X = -100; highliteTile.Y = -100;
            selectedTile.X = -100; selectedTile.Y = -100;


        }

        public override void Open()
        {
            displayState = DisplayState.OPENING;

            //reset systems that screen uses
            System_Land.Reset();
            
            //load map into land system
            System_Land.GenMap();

            //place camera
            Camera2D.targetPosition = new Vector2(128, 128);
            Camera2D.currentPosition = new Vector2(128, 128);
            Camera2D.SetBounds();
            Camera2D.tracksLoosely = false;

            //reset any mutated state
            selectedTileID = -1;
        }

        public override void Close()
        {
            displayState = DisplayState.CLOSING;

        }

        public override void HandleInput()
        {
            if(displayState == DisplayState.OPENED)
            {

                #region Zoom in/out

                if (Input.IsScrollWheelMoving())
                {
                    if (Input.scrollDirection == 1) //zoom in
                    {
                        if (Camera2D.targetZoom < 1.0f)
                        { Camera2D.targetZoom = 1.0f; }
                        else
                        {
                            Camera2D.targetZoom += 1.00f;
                            if (Camera2D.targetZoom > 2.0f)
                            { Camera2D.targetZoom = 2.0f; }
                        }
                    }
                    else if (Input.scrollDirection == 2) //zoom out
                    {
                        if (Camera2D.targetZoom > 1.0f)
                        {
                            Camera2D.targetZoom = 1.0f;
                        }
                        else
                        {
                            Camera2D.targetZoom -= 0.5f;
                            if (Camera2D.targetZoom < 0.5f)
                            { Camera2D.targetZoom = 0.5f; }
                        }
                    }
                }

                #endregion

                //based on zoom amount, display camera differently
                if (Camera2D.targetZoom == 1.0f)
                {

                    #region Move Camera with keyboard input

                    /*
                    if (Input.IsKeyDown(Keys.D))
                    {
                        Camera2D.targetPosition.X += 10;
                    }
                    else if (Input.IsKeyDown(Keys.A))
                    {
                        Camera2D.targetPosition.X -= 10;
                    }
                    if (Input.IsKeyDown(Keys.W))
                    {
                        Camera2D.targetPosition.Y -= 10;
                    }
                    else if (Input.IsKeyDown(Keys.S))
                    {
                        Camera2D.targetPosition.Y += 10;
                    }
                    */

                    #endregion

                    #region Move camera with cursor

                    int bounds = 80;
                    int speed = 3;

                    if (Input.cursorPos_Screen.Y < bounds)
                    {
                        Camera2D.targetPosition.Y -= speed;
                    }
                    else if (Input.cursorPos_Screen.Y > 360 - bounds)
                    {
                        Camera2D.targetPosition.Y += speed;
                    }

                    if (Input.cursorPos_Screen.X < bounds)
                    {
                        Camera2D.targetPosition.X -= speed;
                    }
                    else if (Input.cursorPos_Screen.X > 640 - bounds)
                    {
                        Camera2D.targetPosition.X += speed;
                    }

                    #endregion

                    #region Check overlap with cursor

                    if (Camera2D.targetZoom == 1.0f)
                    {
                        int tileCounter = 0;
                        int yCounter = 1;
                        bool offsetX = true;

                        for (int i = 0; i < System_Land.totalTiles; i++)
                        {
                            //wrap array to map
                            if (tileCounter > System_Land.tileWidth)
                            {
                                tileCounter = 0;
                                yCounter++;

                                offsetX = true;
                                if (yCounter % 2 == 0)
                                { offsetX = false; }
                            }

                            //calc tile position for cursor comparison
                            Point tilePos = new Point(0, 0);

                            //calc x pos
                            tilePos.X = System_Land.x + tileCounter * 32;
                            if (offsetX) { tilePos.X += 16; }

                            //calc y pos
                            tilePos.Y = System_Land.y + (yCounter * 8);

                            //compare cursor and tile pos
                            if (Math.Abs(Input.cursorPos_World.X - tilePos.X) < 12
                                && Math.Abs(Input.cursorPos_World.Y - tilePos.Y) < 6)
                            {
                                //place highlight tile over tile
                                highliteTile.X = tilePos.X - 16;
                                highliteTile.Y = tilePos.Y - 16 - System_Land.tiles[i].Height;

                                //notify debug screen
                                ScreenManager.Text_Debug.text += "TILE ID: " + i;
                                ScreenManager.Text_Debug.text += "\nROW: " + yCounter;
                                
                                //check for LMB
                                if(Input.IsNewLeftClick())
                                {
                                    selectedTileID = i;
                                    selectedTile.X = tilePos.X - 16;
                                    selectedTile.Y = tilePos.Y - 16 - System_Land.tiles[i].Height;
                                }

                                //check for RMB
                                if (Input.IsNewRightClick())
                                {

                                    selectedTileID = i;
                                    selectedTile.X = tilePos.X - 16;
                                    selectedTile.Y = tilePos.Y - 16 - System_Land.tiles[i].Height;
                                    //zoom to selected
                                    Camera2D.targetZoom = 2.0f;
                                    Camera2D.targetPosition.X = selectedTile.X + 16;
                                    Camera2D.targetPosition.Y = selectedTile.Y + 16;
                                }
                            }
                            else
                            {

                                //if (System_Land.tiles[i].Height > 0)
                                //{ System_Land.tiles[i].Height--; }
                                //System_Land.tiles[i].Height = 0;
                            }

                            tileCounter++;
                        }


                    }

                    #endregion
                    

                }
                else if (Camera2D.targetZoom > 1.0f)
                {
                    //wait for player to zoom out
                }
                else
                {
                    //zoomed out, lock camera to one position
                    Camera2D.targetPosition.X = 600;
                    Camera2D.targetPosition.Y = 340;
                    
                }
            }
        }

        public override void Update()
        {

            #region handle display states

            if (displayState == DisplayState.OPENING)
            {
                //animate in
                displayState = DisplayState.OPENED;
            }
            else if (displayState == DisplayState.OPENED)
            {
                //main gameplay
            }
            else if (displayState == DisplayState.CLOSING)
            {
                //animate out
            }
            else if (displayState == DisplayState.CLOSED)
            {
                //exit to ???
            }

            #endregion



            Camera2D.Update();
        }

        public override void Draw()
        {
            ScreenManager.SB.Begin(SpriteSortMode.BackToFront,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                null, null, null, Camera2D.view);

            System_Land.Draw();

            //dont draw selected or highlight zoomed out
            if (Camera2D.targetZoom >= 1.0f)
            {
                ScreenManager.Draw(highliteTile);
                ScreenManager.Draw(selectedTile);
            }
            

            ScreenManager.SB.End();
        }

        //


    }
}
