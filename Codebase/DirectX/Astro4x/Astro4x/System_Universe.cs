using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using System.IO;

namespace Astro4x
{
    //universe tiles - different from land tiles
    public enum Tile_UID : byte
    {
        Empty,
        Planet_Tropical, Planet_Rocky, Planet_Oasis, Planet_Artic, Planet_Moon,
        Star
    }
    public struct Tile_U //est. 1 bytes
    {
        public Tile_UID ID;
    }

    public static class System_Universe
    {
        public static int tilesPerRow = 80;

        public static int totalTiles = tilesPerRow * 45;
        public static Tile_U[] tiles = new Tile_U[totalTiles];
        public static SpriteStruct sprite;

        public static int x = 0;
        public static int y = 0;



        public static void Constructor()
        {
            //setup universe tile sprite
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
            for (int i = 0; i < totalTiles; i++)
            {
                tiles[i].ID = Tile_UID.Empty;
            }
        }

        public static void Update()
        {

        }

        public static void Draw()
        {
            ScreenManager.GDM.GraphicsDevice.Clear(Color.Black);
            
            int tileCounter = 0;
            int yCounter = 1;

            for (int i = 0; i < totalTiles; i++)
            {
                //set x, y frame based on id
                sprite.draw_x = (byte)tiles[i].ID;
                
                sprite.draw_y = 1;
                //swap to simple tiles if camera zoomed out
                if (Camera2D.targetZoom < 1.0f)
                { sprite.draw_y = 0; }

                //wrap array to map
                if (tileCounter >= tilesPerRow)
                {
                    tileCounter = 0;
                    yCounter++;
                }

                //calc pos
                sprite.X = x + tileCounter * 16;
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
                    Assets.sheet_Universe,
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

        //gen universe method
        public static void Generate()
        {
            for (int i = 0; i < totalTiles; i++)
            {
                if(ScreenManager.RAND.Next(0, 101) > 99)
                {
                    //randomly choose an available type
                    tiles[i].ID = (Tile_UID)ScreenManager.RAND.Next(0, 7);
                }
            }
        }

        //save uni method

        //load uni method

    }
}