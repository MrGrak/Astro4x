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
    public class Screen_Universe : Screen
    {
        public byte heightOffset = 8;

        public SpriteStruct highliteTile;
        public SpriteStruct selectedTile;
        public int selectedTileID;

        public byte highliteTimer = 0;
        public byte highliteAnimIndex = 0;

        public Text tileInfo;

        //zoom state: 0=2.0, 1=1.0, 2=0.5, 3=0.25
        public byte zoomState = 1;


        public Screen_Universe()
        {
            Name = "UNIVERSE";

            //setup highlight tile sprite
            highliteTile = new SpriteStruct();
            highliteTile.draw_width = 16;
            highliteTile.draw_height = 16;
            highliteTile.draw_x = (byte)(15 * highliteTile.draw_width);
            highliteTile.draw_y = (byte)(0 * highliteTile.draw_height);
            highliteTile.alpha = 1.0f;
            highliteTile.layer = Layers.Land_UI;

            //copy same properties to selected tile sprite
            selectedTile = highliteTile;
            //highlight tile needs to be lighter
            highliteTile.alpha = 0.75f;
            //hide off screen for now
            highliteTile.X = -100; highliteTile.Y = -100;
            selectedTile.X = -100; selectedTile.Y = -100;

            tileInfo = new Text("init", new Vector2(999, 999), Color.White);
            tileInfo.layer = Layers.Debug_Text;
            
            System_Universe.Reset();
            System_Universe.Generate();
        }

        public override void Open()
        {
            displayState = DisplayState.OPENING;
            
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
            if (displayState == DisplayState.OPENED)
            {
                if (Input.IsNewKeyPress(Keys.Space))
                {
                    //open a dev menu
                    //if (Camera2D.targetZoom <= 1.0f && ScreenManager.activeScreen == this)
                    //{ ScreenManager.AddScreen(ScreenManager.LandMenu); }
                }


                #region Zoom in/out, using zoomState

                if (Input.IsScrollWheelMoving())
                {
                    if (Input.scrollDirection == 1) //zoom in
                    {
                        if (zoomState == 2)
                        {
                            zoomState = 1; //zoom in
                            Camera2D.targetZoom = 1.0f;
                        }
                    }
                    else if (Input.scrollDirection == 2) //zoom out
                    {
                        if (zoomState <= 1)
                        {
                            zoomState = 2; //zoom out
                            Camera2D.targetZoom = 0.5f;
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
                    
                    #region Check overlap with cursor

                    if (Camera2D.targetZoom == 1.0f)
                    {
                        int tileCounter = 0;
                        int yCounter = 1;

                        for (int i = 0; i < System_Universe.totalTiles; i++)
                        {
                            //wrap array to map
                            if (tileCounter >= System_Universe.tilesPerRow)
                            {
                                tileCounter = 0;
                                yCounter++;
                            }

                            //calc tile position for cursor comparison
                            Point tilePos = new Point(0, 0);

                            //calc x pos
                            tilePos.X = System_Universe.x + tileCounter * 16;

                            //calc y pos
                            tilePos.Y = System_Universe.y + (yCounter * 16);

                            //compare cursor and tile pos
                            if (Math.Abs(Input.cursorPos_World.X - tilePos.X) < 8
                                && Math.Abs(Input.cursorPos_World.Y - tilePos.Y) < 8)
                            {
                                //place highlight tile over tile
                                highliteTile.X = tilePos.X - 8;
                                highliteTile.Y = tilePos.Y - 8;
                                
                                //check for LMB
                                if (Input.IsNewLeftClick())
                                {
                                    selectedTileID = i;
                                    selectedTile.X = tilePos.X - 8;
                                    selectedTile.Y = tilePos.Y - 8;

                                    //Camera2D.targetPosition.X = selectedTile.X + 8;
                                    //Camera2D.targetPosition.Y = selectedTile.Y + 8;

                                    //update tile info text
                                    tileInfo.text = "ID: " + i + "." + System_Universe.tiles[i].ID.ToString().ToUpper();
                                    tileInfo.text += "\nROW: " + yCounter;

                                    tileInfo.position.X = tilePos.X - 8;
                                    tileInfo.position.Y = tilePos.Y + 10;
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
                                if (Input.currentMouseState.RightButton == ButtonState.Pressed)
                                {
                                    //fill selected 3x3
                                    //System_Land.Fill3x3(i, TileID.Grass);

                                    //fill selected
                                    //System_Land.tiles[i].ID = Tile_LID.Grass;
                                }


                                //only check one tile, exit loop
                                i = System_Universe.totalTiles;
                            }

                            tileCounter++;
                        }


                    }

                    #endregion

                }
                else
                {
                    //zoomed out, lock camera to one position
                    Camera2D.targetPosition.X = 632;
                    Camera2D.targetPosition.Y = 368;
                    //this is for 0.5 and 0.25 zooms
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

            #region Animate highlight tile

            highliteTimer++;
            if (highliteTimer > 10)
            {
                highliteTimer = 0;
                //inc animation index, loop at end
                highliteAnimIndex++;
                if (highliteAnimIndex > 2)
                { highliteAnimIndex = 0; }
                //set frame
                highliteTile.draw_y = (byte)(highliteAnimIndex * highliteTile.draw_height);
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

            System_Universe.Draw();

            //draw selected tile and highlight sprite
            if (ScreenManager.activeScreen == this)
            {
                //dont draw selected when zoomed out
                if (Camera2D.currentZoom >= 1.0f)
                {
                    ScreenManager.Draw(selectedTile, Assets.sheet_Land);

                    Vector2 textSize = Assets.Assets_font_eng.MeasureString(tileInfo.text);

                    ScreenManager.Draw(new Rectangle(
                        (int)tileInfo.position.X - 2, (int)tileInfo.position.Y,
                        (int)textSize.X + 3, (int)textSize.Y + 1),
                        Color.Black, 0.5f, Layers.Land_UI);

                    ScreenManager.Draw(tileInfo);
                }
                //draw highlight tile only at interactive pov
                if (Camera2D.currentZoom == 1.0f)
                {
                    ScreenManager.Draw(highliteTile, Assets.sheet_Land);
                }
            }

            ScreenManager.SB.End();

            /*
            //draw any screen space sprites
            ScreenManager.SB.Begin(SpriteSortMode.BackToFront,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                null, null, null, null);
            
            ScreenManager.SB.End();
            */
        }

        //

    }
}