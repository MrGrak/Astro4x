using System;
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
        public static int tileWidth = 37;

        public static int totalTiles = tileWidth * 85 + 8;
        public static Tile[] tiles = new Tile[totalTiles];
        public static SpriteStruct sprite;
        
        public static int x = 0;
        public static int y = 0;


        public static void Constructor()
        {
            //setup land tile sprite
            sprite = new SpriteStruct();
            sprite.draw_width = 32;
            sprite.draw_height = 32;
            sprite.draw_x = (byte)(0 * sprite.draw_width);
            sprite.draw_y = (byte)(0 * sprite.draw_height);

            sprite.alpha = 1.0f;
            sprite.layer = Layers.Dev_sprite;
        }

        public static void Reset()
        {
            for(int i = 0; i < totalTiles; i++)
            {
                tiles[i].ID = TileID.Default;
                tiles[i].Height = 0;
                //tiles[i].Height = (byte)ScreenManager.RAND.Next(0, 7);
            }
        }
        
        public static void Update()
        {

        }

        public static void Draw()
        {
            int tileCounter = 0;
            int yCounter = 1;
            bool offsetX = true;

            for (int i = 0; i < totalTiles; i++)
            {
                //set x, y frame based on id
                sprite.draw_x = 0;
                sprite.draw_y = (byte)tiles[i].ID;
                
                //wrap array to map
                if (tileCounter > tileWidth)
                {
                    tileCounter = 0;
                    yCounter++;

                    offsetX = true;
                    if (yCounter % 2 == 0)
                    { offsetX = false; }
                }

                //calc x pos
                sprite.X = x + tileCounter * 32;
                if (offsetX) { sprite.X += 16; }

                //calc y pos
                sprite.Y = y + (yCounter * 8) - tiles[i].Height;
                
                tileCounter++;

                //calc sprite layer
                sprite.layer = Layers.Dev_sprite;
                sprite.layer -= i * 0.000001f; //sort back to front
                
                //draw sprite at tile location
                Vector2 origin = new Vector2(16, 16);

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
            //create a water world with polar ice caps
            for (int i = 0; i < totalTiles; i++)
            {
                tiles[i].ID = TileID.Water_Deep;

                //create ice caps on top and bottom rows
                if(i < tileWidth * 6 + 2) //top
                {
                    if (i < tileWidth * 4)
                    { tiles[i].ID = TileID.Snow; tiles[i].Height = (byte)(ScreenManager.RAND.Next(0, 3) * 2); }
                    else { tiles[i].ID = TileID.Water_Shallow; }
                }
                else if(i > totalTiles - (tileWidth * 6) - 2) //botom
                {
                    if (i > totalTiles - (tileWidth * 4))
                    { tiles[i].ID = TileID.Snow; tiles[i].Height = (byte)(ScreenManager.RAND.Next(0, 3) * 2); }
                    else { tiles[i].ID = TileID.Water_Shallow; }
                }
            }


            //create a land mass and grow it
            int seed = ScreenManager.RAND.Next(tileWidth * 8, totalTiles - (tileWidth * 8));


            //test all neighbors

            FloodFill(1035, 2, TileID.Grass);

            //FloodFill(2083, 2, TileID.Dirt);
        }

        public static void FloodFill(int arrayIndex, int levels, TileID Type)
        {
            tiles[arrayIndex].ID = Type;

            //level 1
            if(levels >= 1)
            {
                for (int i = 1; i < 9; i++)
                { tiles[GetNeighbor(arrayIndex, (Direction)i)].ID = Type; }
            }

            //level 2
            if (levels >= 2)
            {
                for (int g = 1; g < 9; g++)
                {
                    int neighbor = GetNeighbor(arrayIndex, (Direction)g);
                    for (int i = 1; i < 9; i++)
                    { tiles[GetNeighbor(neighbor, (Direction)i)].ID = Type; }
                }
            }
            
            


        }

        public static int GetNeighbor(int arrayIndex, Direction D)
        {
            int returnValue = arrayIndex;

            if (D == Direction.Up) //up
            {
                returnValue = arrayIndex - tileWidth * 2 - 2;
            }
            else if(D == Direction.UpRight)
            {
                returnValue = arrayIndex - tileWidth * 1 - 0;
            }
            else if (D == Direction.Right)
            {
                returnValue = arrayIndex + 1;
            }
            else if (D == Direction.DownRight)
            {
                returnValue = arrayIndex + tileWidth * 1 + 2;
            }
            else if (D == Direction.Down)
            {
                returnValue = arrayIndex + tileWidth * 2 + 2;
            }
            else if (D == Direction.DownLeft)
            {
                returnValue = arrayIndex + tileWidth * 1 + 1;
            }
            else if (D == Direction.Left)
            {
                returnValue = arrayIndex - 1;
            }
            else if (D == Direction.UpLeft)
            {
                returnValue = arrayIndex - tileWidth * 1 - 1;
            }

            //bound max + min return values
            if (returnValue > totalTiles)
            { returnValue = totalTiles; }
            else if (returnValue < 0)
            { returnValue = 0; }

            return returnValue;
        }






    }
}