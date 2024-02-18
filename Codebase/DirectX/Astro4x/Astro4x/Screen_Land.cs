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

        
        public Screen_Land()
        {
            Name = "LAND";
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
        }

        public override void Close()
        {
            displayState = DisplayState.CLOSING;

        }

        public override void HandleInput()
        {

            



            //zoom in/out
            if (Input.IsScrollWheelMoving())
            {
                if(Input.scrollDirection == 1) //zoom in
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
                    if(Camera2D.targetZoom > 1.0f)
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



            //based on zoom amount, display camera differently
            if(Camera2D.targetZoom == 1.0f)
            {

                #region Move Camera with keyboard input

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
                        Vector2 tilePos = new Vector2(0, 0);

                        //calc x pos
                        tilePos.X = System_Land.x + tileCounter * 32;
                        if (offsetX) { tilePos.X += 16; }

                        //calc y pos
                        tilePos.Y = System_Land.y + (yCounter * 8);

                        //compare cursor and tile pos
                        if (Math.Abs(Input.cursorPos_World.X - tilePos.X) < 8
                            && Math.Abs(Input.cursorPos_World.Y - tilePos.Y) < 8)
                        {
                            //collision

                            System_Land.tiles[i].Offset = 4;

                            //notify debug screen
                            ScreenManager.Text_Debug.text += "TILE ID: " + i;
                            ScreenManager.Text_Debug.text += "\nROW: " + yCounter;
                        }
                        else
                        {
                            System_Land.tiles[i].Offset = 0;

                            //if (System_Land.tiles[i].Height > 0)
                            //{ System_Land.tiles[i].Height--; }
                            //System_Land.tiles[i].Height = 0;
                        }

                        tileCounter++;
                    }


                }

                #endregion

            }
            else if(Camera2D.targetZoom > 1.0f)
            {
                //nothing rn
            }
            else
            {
                //zoomed out, lock camera to one position
                Camera2D.targetPosition.X = 600;
                Camera2D.targetPosition.Y = 340;

                Camera2D.tracksLoosely = false;
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

            ScreenManager.SB.End();
        }

        //


    }
}
