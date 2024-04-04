﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Astro4x
{
    public enum Direction : Byte
    { None, Up, UpRight, Right, DownRight, Down, DownLeft, Left, UpLeft }

    public static class System_Land
    {
        public static int tilesPerRow = 80; 

        public static int totalTiles = tilesPerRow * 45;
        public static Tile[] tiles = new Tile[totalTiles];
        public static SpriteStruct sprite;
        
        public static int x = 0;
        public static int y = 0;


        public static void Constructor()
        {
            //setup land tile sprite
            sprite = new SpriteStruct();
            sprite.draw_width = 16;
            sprite.draw_height = 16;
            sprite.draw_x = (byte)(0 * sprite.draw_width);
            sprite.draw_y = (byte)(0 * sprite.draw_height);

            sprite.alpha = 1.0f;
            sprite.layer = Layers.Land;
        }

        public static void Reset()
        {
            for(int i = 0; i < totalTiles; i++)
            {
                tiles[i].ID = TileID.Grass;
            }
        }
        
        public static void Update()
        {

        }

        public static void Draw()
        {
            int tileCounter = 0;
            int yCounter = 1;

            for (int i = 0; i < totalTiles; i++)
            {
                //set x, y frame based on id
                sprite.draw_x = (byte)tiles[i].ID;
                sprite.draw_y = 0;
                
                //wrap array to map
                if (tileCounter >= tilesPerRow)
                {
                    tileCounter = 0;
                    yCounter++;
                }

                //calc x pos
                sprite.X = x + tileCounter * 16;

                //calc y pos
                sprite.Y = y + (yCounter * 16);
                
                tileCounter++;

                //calc sprite layer
                sprite.layer = Layers.Land;
                sprite.layer -= i * 0.000001f; //sort back to front
                
                //draw sprite at tile location
                Vector2 origin = new Vector2(8, 8);

                Rectangle DrawRec;
                DrawRec.X = sprite.draw_x * sprite.draw_width;
                DrawRec.Y = sprite.draw_y * sprite.draw_height;
                DrawRec.Width = sprite.draw_width;
                DrawRec.Height = sprite.draw_height;

                ScreenManager.SB.Draw(
                    Assets.sheet_Main,
                    new Vector2(sprite.X, sprite.Y),
                    DrawRec,
                    Color.White * sprite.alpha,
                    0.0f,
                    origin,
                    1.0f,
                    SpriteEffects.None,
                    sprite.layer);
            }
        }

        //

        public static void GenMap()
        {

            #region Create a water world with polar ice caps

            for (int i = 0; i < totalTiles; i++)
            {
                tiles[i].ID = TileID.Water_Deep;

                //create ice caps on top and bottom rows
                if(i < tilesPerRow * 4) //top
                {
                    if (i < tilesPerRow * 2)
                    {
                        tiles[i].ID = TileID.Snow;
                    }
                    else { tiles[i].ID = TileID.Water_Shallow; }
                }
                else if(i > totalTiles - (tilesPerRow * 4 + 1)) //bottom
                {
                    if (i > totalTiles - (tilesPerRow * 2 + 1))
                    {
                        tiles[i].ID = TileID.Snow;
                    }
                    else { tiles[i].ID = TileID.Water_Shallow; }
                }
            }

            #endregion


            //create a land mass and grow it
            //int seed = ScreenManager.RAND.Next(tilesPerRow * 8, totalTiles - (tilesPerRow * 8));

            /*
            //test all neighbors
            Fill3x3(533, TileID.Grass); //572 610 649 687
            Fill3x3(687, TileID.Grass);
            Fill3x3(841, TileID.Grass);
            Fill3x3(995, TileID.Grass);
            */

            //Fill3x3(533, TileID.Grass);
            //+154

            /*
            //create diagonal line across map
            {
                int counter = 381;
                for (int i = 0; i < 66; i++)
                {
                    Fill3x3(counter, TileID.Grass);
                    if (i % 2 == 0) { counter += tilesPerRow + 2; }
                    else { counter += tilesPerRow + 1; }
                }
            }
            */

            /*
            //create test lines across map
            int start = 957;
            for (int i = 0; i < 10; i++)
            {
                tiles[start].ID = TileID.Grass;
                start = GetNeighbor(start, Direction.DownRight);
            }
            */




            /*
            //build random splotches
            for(int i = 0; i < 50; i++)
            {
                int id = ScreenManager.RAND.Next(400, totalTiles - 400);
                Fill3x3(id, TileID.Grass);
            }
            */


        }

        public static void Fill3x3(int arrayIndex, TileID Type)
        {
            if (arrayIndex >= totalTiles) { return; }

            tiles[arrayIndex].ID = Type;

            for (int i = 1; i < 9; i++)
            { tiles[GetNeighbor(arrayIndex, (Direction)i)].ID = Type; }
        }

        public static int GetNeighbor(int arrayIndex, Direction D)
        {
            int returnValue = arrayIndex;
            
            if (D == Direction.Up)
            {
                returnValue = arrayIndex - tilesPerRow; 
            }
            else if (D == Direction.UpRight)
            {
                returnValue = arrayIndex - tilesPerRow * 1 + 1;
            }
            else if (D == Direction.Right)
            {
                returnValue = arrayIndex + 1; 
            }
            else if (D == Direction.DownRight)
            {
                returnValue = arrayIndex + tilesPerRow * 1 + 1;
            }
            else if (D == Direction.Down)
            {
                returnValue = arrayIndex + tilesPerRow; 
            }
            else if (D == Direction.DownLeft)
            {
                returnValue = arrayIndex + tilesPerRow * 1 - 1;
            }
            else if (D == Direction.Left)
            {
                returnValue = arrayIndex - 1;
            }
            else if (D == Direction.UpLeft)
            {
                returnValue = arrayIndex - tilesPerRow * 1 - 1;
            }

            //bound max + min return values
            if (returnValue >= totalTiles)
            { returnValue = totalTiles - 1; }

            else if (returnValue < 0)
            { returnValue = 0; }

            return returnValue;
        }






    }
}