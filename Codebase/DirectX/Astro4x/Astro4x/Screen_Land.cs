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

        public byte highliteTimer = 0;
        public byte highliteAnimIndex = 0;


        public Screen_Land()
        {
            Name = "LAND";
            
            //setup highlight tile sprite
            highliteTile = new SpriteStruct();
            highliteTile.draw_width = 16;
            highliteTile.draw_height = 16;
            highliteTile.draw_x = (byte)(0 * highliteTile.draw_width);
            highliteTile.draw_y = (byte)(2 * highliteTile.draw_height);
            highliteTile.alpha = 1.0f;
            highliteTile.layer = Layers.Land_UI;

            //copy same properties to selected tile sprite
            selectedTile = highliteTile;
            //highlight tile needs to be lighter
            highliteTile.alpha = 0.75f;
            //hide off screen for now
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

            //place camera center
            Camera2D.targetPosition = new Vector2(632, 368);
            Camera2D.currentPosition = new Vector2(632, 368);
            
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

                //generate a new world map via space
                if(Input.IsNewKeyPress(Keys.Space))
                {
                    //System_Land.GenMap();

                    //open the land devn menu
                    if (Camera2D.targetZoom <= 1.0f && ScreenManager.activeScreen == this)
                    {
                        ScreenManager.AddScreen(ScreenManager.LandMenu);
                    }
                }




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
                    
                    if (Input.IsKeyDown(Keys.D))
                    {
                        if (Camera2D.targetPosition.X < Camera2D.levelMax.X - 5)
                        { Camera2D.targetPosition.X += 5; }
                    }
                    if (Input.IsKeyDown(Keys.A))
                    {
                        if (Camera2D.targetPosition.X > Camera2D.levelMin.X + 5)
                        { Camera2D.targetPosition.X -= 5; }
                    }
                    
                    if (Input.IsKeyDown(Keys.W))
                    {
                        if (Camera2D.targetPosition.Y > Camera2D.levelMin.Y + 5)
                        { Camera2D.targetPosition.Y -= 5; }
                    }
                    if (Input.IsKeyDown(Keys.S))
                    {
                        if (Camera2D.targetPosition.Y < Camera2D.levelMax.Y - 5)
                        { Camera2D.targetPosition.Y += 5; }
                    }
                    
                    #endregion

                    #region Move camera with cursor

                    //this is shit and i hate it

                    /*
                    int boundsX = 20;
                    int boundsY = 20;
                    float speedX = 4.0f;
                    float speedY = 2.5f;

                    if (Input.cursorPos_Screen.X < boundsX)
                    {
                        if (Camera2D.targetPosition.X > 290)
                        { Camera2D.targetPosition.X -= speedX; }
                    }
                    else if (Input.cursorPos_Screen.X > 640 - boundsX)
                    {
                        if (Camera2D.targetPosition.X < 920)
                        { Camera2D.targetPosition.X += speedX; }
                    }

                    if (Input.cursorPos_Screen.Y < boundsY)
                    {
                        if (Camera2D.targetPosition.Y > 160)
                        { Camera2D.targetPosition.Y -= speedY; }
                        
                    }
                    else if (Input.cursorPos_Screen.Y > 360 - boundsY)
                    {
                        if (Camera2D.targetPosition.Y < 500)
                        { Camera2D.targetPosition.Y += speedY; }
                    }
                    */

                    #endregion

                    #region Check overlap with cursor

                    if (Camera2D.targetZoom == 1.0f)
                    {
                        
                        
                        int tileCounter = 0;
                        int yCounter = 1;

                        for (int i = 0; i < System_Land.totalTiles; i++)
                        {
                            //wrap array to map
                            if (tileCounter >= System_Land.tilesPerRow)
                            {
                                tileCounter = 0;
                                yCounter++;
                            }

                            //calc tile position for cursor comparison
                            Point tilePos = new Point(0, 0);

                            //calc x pos
                            tilePos.X = System_Land.x + tileCounter * 16;

                            //calc y pos
                            tilePos.Y = System_Land.y + (yCounter * 16);

                            //compare cursor and tile pos
                            if (Math.Abs(Input.cursorPos_World.X - tilePos.X) < 8
                                && Math.Abs(Input.cursorPos_World.Y - tilePos.Y) < 8)
                            {
                                //place highlight tile over tile
                                highliteTile.X = tilePos.X - 8;
                                highliteTile.Y = tilePos.Y - 8;

                                //notify debug screen
                                ScreenManager.Text_Debug_FollowMouse.text = "TILE ID: " + i;
                                ScreenManager.Text_Debug_FollowMouse.text += "\nROW: " + yCounter;
                                ScreenManager.Text_Debug_FollowMouse.text += "\n";

                                //check for LMB
                                if (Input.IsNewLeftClick())
                                {
                                    selectedTileID = i;
                                    selectedTile.X = tilePos.X - 8;
                                    selectedTile.Y = tilePos.Y - 8;

                                    Camera2D.targetPosition.X = selectedTile.X + 8;
                                    Camera2D.targetPosition.Y = selectedTile.Y + 8;
                                }

                                //check for RMB
                                if (Input.IsNewRightClick())
                                {
                                    //selectedTileID = i;
                                    //selectedTile.X = tilePos.X - 8;
                                    //selectedTile.Y = tilePos.Y - 8;

                                    /*
                                    //zoom to selected
                                    Camera2D.targetZoom = 2.0f;
                                    Camera2D.targetPosition.X = selectedTile.X + 8;
                                    Camera2D.targetPosition.Y = selectedTile.Y + 8;
                                    */

                                    //fill selected
                                    //System_Land.Fill3x3(i, TileID.Grass);
                                }

                                //"paint" tiles using RMB
                                if(Input.currentMouseState.RightButton == ButtonState.Pressed)
                                {
                                    //fill selected 3x3
                                    //System_Land.Fill3x3(i, TileID.Grass);

                                    //fill selected
                                    System_Land.tiles[i].ID = TileID.Grass;
                                }


                                //only check one tile, exit loop
                                i = System_Land.totalTiles;
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

                    #region Move Camera with keyboard input

                    if (Input.IsKeyDown(Keys.D))
                    {
                        Camera2D.targetPosition.X += 2;
                    }
                    if (Input.IsKeyDown(Keys.A))
                    {
                        Camera2D.targetPosition.X -= 2;
                    }

                    if (Input.IsKeyDown(Keys.W))
                    {
                        Camera2D.targetPosition.Y -= 2;
                    }
                    if (Input.IsKeyDown(Keys.S))
                    {
                        Camera2D.targetPosition.Y += 2;
                    }

                    #endregion
                }
                else
                {
                    //zoomed out, lock camera to one position
                    Camera2D.targetPosition.X = 632;
                    Camera2D.targetPosition.Y = 368;
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



            #region Aanimate highlight tile
            
            highliteTimer++;
            if(highliteTimer > 10)
            {
                highliteTimer = 0;
                //inc animation index, loop at end
                highliteAnimIndex++;
                if(highliteAnimIndex > 2)
                { highliteAnimIndex = 0; }
                //set x frame (animate)
                highliteTile.draw_x = (byte)(highliteAnimIndex * highliteTile.draw_width);
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

            //draw deep sea color as background
            ScreenManager.GDM.GraphicsDevice.Clear(new Color(36,60,90));

            System_Land.Draw();

            //dont draw selected when zoomed out
            if (Camera2D.targetZoom >= 1.0f)
            {
                ScreenManager.Draw(selectedTile);
            }
            //draw highlight tile only at interactive pov
            if(Camera2D.targetZoom == 1.0f)
            {
                ScreenManager.Draw(highliteTile);
            }

            ScreenManager.SB.End();
        }

        //


    }
}
