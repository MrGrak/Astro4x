﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using System.IO;

namespace Astro4x
{
    //land tiles - different from universe tiles
    public enum Tile_LID : byte
    {
        Grass, Dirt_Mars, Dirt_Brown, Desert,
        Snow, Water_Shallow, Water_Deep, Dirt_Gray,
    }
    public struct Tile_L //land tile est. 1 bytes
    {
        public Tile_LID ID;
    }

    public enum PlanetType : Byte
    { Tropical, Rocky, Oasis, Artic, Moon }

    public static class System_Land
    {
        public static int tilesPerRow = 80; 

        public static int totalTiles = tilesPerRow * 45;
        public static Tile_L[] tiles = new Tile_L[totalTiles];
        public static SpriteStruct sprite;
        
        public static int x = 0;
        public static int y = 0;

        public static PlanetType planetType = PlanetType.Tropical;

        public static float tileAlpha = 1.0f;



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
                tiles[i].ID = Tile_LID.Water_Deep;
            }

            tileAlpha = 0.0f;
        }
        
        public static void Update()
        {

        }

        public static void Draw()
        {
            //fade tiles in from background color
            tileAlpha += 0.1f;
            if (tileAlpha > 1.0f) { tileAlpha = 1.0f; }

            //draw background color based on planet type to hide map tile edges
            if (planetType == PlanetType.Tropical)
            { ScreenManager.GDM.GraphicsDevice.Clear(Assets.Color_DeepSea_Blue * tileAlpha); }
            else if (planetType == PlanetType.Rocky)
            { ScreenManager.GDM.GraphicsDevice.Clear(Assets.Color_Mars_Orange * tileAlpha); }
            else if (planetType == PlanetType.Oasis)
            { ScreenManager.GDM.GraphicsDevice.Clear(Assets.Color_Desert_Yellow * tileAlpha); }
            else if (planetType == PlanetType.Artic)
            { ScreenManager.GDM.GraphicsDevice.Clear(Assets.Color_Artic_Blue * tileAlpha); }
            else if (planetType == PlanetType.Moon)
            { ScreenManager.GDM.GraphicsDevice.Clear(Assets.Color_Moon_Gray * tileAlpha); }

            

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
                sprite.alpha = tileAlpha;

                //draw sprite at tile location
                Vector2 origin = new Vector2(8, 8);

                Rectangle DrawRec;
                DrawRec.X = sprite.draw_x * sprite.draw_width;
                DrawRec.Y = sprite.draw_y * sprite.draw_height;
                DrawRec.Width = sprite.draw_width;
                DrawRec.Height = sprite.draw_height;

                ScreenManager.SB.Draw(
                    Assets.sheet_Land,
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

        //utility methods

        public static void Fill3x3(int arrayIndex, Tile_LID Type)
        {
            if (arrayIndex >= totalTiles) { return; }
            else if (arrayIndex < 0) { return; }

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

        public static void GenIsland(int arrayIndex, Tile_LID Type, int iterations)
        {
            for (int g = 0; g < iterations; g++)
            {
                //start with a tile
                int seedID = arrayIndex;

                for (int i = 0; i < 10; i++)
                {
                    Fill3x3(seedID, Type);


                    //make sure current X isn't too far left or right
                    if (seedID / tilesPerRow > 5 &&
                        seedID / tilesPerRow < tilesPerRow - 5)
                    {
                        //jiggle seed left and right
                        seedID += ScreenManager.RAND.Next(-3, 4);
                    }

                    


                    //make sure current row isn't first or last few
                    if (seedID > tilesPerRow * 5 &&
                        seedID < totalTiles - tilesPerRow * 5)
                    {
                        //move seed up or down
                        if (ScreenManager.RAND.Next(0, 101) > 50)
                        { seedID -= tilesPerRow * 1; }
                        else
                        { seedID += tilesPerRow * 1; }

                        //move seed up or down
                        if (ScreenManager.RAND.Next(0, 101) > 90)
                        {
                            if (ScreenManager.RAND.Next(0, 101) > 50)
                            { seedID -= tilesPerRow * 2; }
                            else
                            { seedID += tilesPerRow * 2; }
                        }
                    }



                }
            }
        }

        public static void FillinLoneTiles()
        {
            for (int i = 0; i < totalTiles; i++)
            {
                //check horizontal neighbors to see if lone tile
                Tile_LID leftN = tiles[GetNeighbor(i, Direction.Left)].ID;
                Tile_LID rightN = tiles[GetNeighbor(i, Direction.Right)].ID;
                //check vertical neighbors
                Tile_LID upN = tiles[GetNeighbor(i, Direction.Up)].ID;
                Tile_LID downN = tiles[GetNeighbor(i, Direction.Down)].ID;

                if (leftN == rightN && upN == downN && leftN == upN)
                {
                    tiles[i].ID = leftN;
                }
            }
        }

        //island generation rountines

        public static void GenMap_Tropical()
        {
            Reset();
            //clear any current tileInfo on land
            ScreenManager.Land.tileInfo.position.X = 9999;
            ScreenManager.Land.tileInfo.text = "";
            ScreenManager.Land.selectedTile.X = 9999;

            //store planet type
            planetType = PlanetType.Tropical;

            //start with two 'city' locations that we will bridge together
            int cityA = 1228;
            int cityB = 2209;
            
            //create coastlines around cities
            GenIsland(cityA, Tile_LID.Desert, 10);
            GenIsland(cityB, Tile_LID.Desert, 10);

            //create fill coastlines between cities
            GenIsland(1630, Tile_LID.Desert, 10);
            GenIsland(1807, Tile_LID.Desert, 10);

            //create islands in corners
            GenIsland(853, Tile_LID.Desert, 3);
            GenIsland(2660, Tile_LID.Desert, 3);
            
            //create grass within bounds of desert
            for (int i = 0; i < totalTiles; i++)
            {
                if (tiles[i].ID == Tile_LID.Desert)
                {
                    bool flipToForest = true;
                    for (int p = 1; p < 9; p++)
                    {
                        int neighborID = GetNeighbor(i, (Direction)p);
                        if (tiles[neighborID].ID == Tile_LID.Water_Deep)
                        { flipToForest = false; }
                    }
                    if (flipToForest) { tiles[i].ID = Tile_LID.Grass; }
                }
            }
            
            //fill in lone desert tiles
            for (int i = 0; i < totalTiles; i++)
            {
                Tile_LID leftN = tiles[GetNeighbor(i, Direction.Left)].ID;
                Tile_LID rightN = tiles[GetNeighbor(i, Direction.Right)].ID;
                Tile_LID upN = tiles[GetNeighbor(i, Direction.Up)].ID;
                Tile_LID downN = tiles[GetNeighbor(i, Direction.Down)].ID;
                //check for surrounding desert tiles
                if (leftN == Tile_LID.Desert && 
                    leftN == rightN && upN == downN && leftN == upN)
                { tiles[i].ID = leftN; }
            }
            
            for (int i = 0; i < totalTiles; i++)
            {
                //add shallows around land tiles 
                if (tiles[i].ID != Tile_LID.Water_Deep
                    && tiles[i].ID != Tile_LID.Water_Shallow)
                {
                    for (int p = 1; p < 9; p++)
                    {
                        int neighborID = GetNeighbor(i, (Direction)p);
                        if (tiles[neighborID].ID == Tile_LID.Water_Deep)
                        { tiles[neighborID].ID = Tile_LID.Water_Shallow; }
                    }
                }
                
                //define cities and connect them
                //if (i == cityA) { tiles[i].ID = TileID.Dirt; }
                //else if (i == cityB) { tiles[i].ID = TileID.Dirt; }
            }

            //clear 1st and last tile to deep water
            //tiles[0].ID = TileID.Water_Deep;
            //tiles[totalTiles-1].ID = TileID.Water_Deep;
            //we should be clearing rows and columns, probably
        }

        public static void GenMap_Mars()
        {
            Reset();
            //clear any current tileInfo on land
            ScreenManager.Land.tileInfo.position.X = 9999;
            ScreenManager.Land.tileInfo.text = "";
            ScreenManager.Land.selectedTile.X = 9999;

            //store planet type
            planetType = PlanetType.Rocky;

            //reset land to orange dirt
            for (int i = 0; i < totalTiles; i++)
            {
                tiles[i].ID = Tile_LID.Dirt_Mars;
            }

            //start with two 'city' locations
            int cityA = 1228;
            int cityB = 2209;

            //create dirt islands
            GenIsland(cityA, Tile_LID.Dirt_Brown, 10);
            GenIsland(cityB, Tile_LID.Dirt_Brown, 10);

            //create dirt islands between cities
            GenIsland(1630, Tile_LID.Dirt_Brown, 10);
            GenIsland(1807, Tile_LID.Dirt_Brown, 10);

            //create dirt islands in corners
            GenIsland(853, Tile_LID.Dirt_Brown, 3);
            GenIsland(2660, Tile_LID.Dirt_Brown, 3);


            //fill in lone tiles with Dirt_Brown
            for (int g = 0; g < 5; g++)
            {
                for (int i = 0; i < totalTiles; i++)
                {
                    Tile_LID leftN = tiles[GetNeighbor(i, Direction.Left)].ID;
                    Tile_LID rightN = tiles[GetNeighbor(i, Direction.Right)].ID;
                    Tile_LID upN = tiles[GetNeighbor(i, Direction.Up)].ID;
                    Tile_LID downN = tiles[GetNeighbor(i, Direction.Down)].ID;

                    //check horizontal
                    if (leftN == Tile_LID.Dirt_Brown && leftN == rightN)
                    { tiles[i].ID = leftN; }

                    //check vertical
                    if (upN == Tile_LID.Dirt_Brown && upN == downN)
                    { tiles[i].ID = upN; }
                }
            }
            
        }

        public static void GenMap_Oasis()
        {
            Reset();
            //clear any current tileInfo on land
            ScreenManager.Land.tileInfo.position.X = 9999;
            ScreenManager.Land.tileInfo.text = "";
            ScreenManager.Land.selectedTile.X = 9999;

            //store planet type
            planetType = PlanetType.Oasis;

            //reset land to desert
            for (int i = 0; i < totalTiles; i++)
            {
                tiles[i].ID = Tile_LID.Desert;
            }
            
            //create water in central area
            GenIsland(1228, Tile_LID.Water_Deep, 10);
            GenIsland(1799, Tile_LID.Water_Deep, 10);
            GenIsland(2209, Tile_LID.Water_Deep, 10);

            //fill in lone tiles with deep water
            for (int g = 0; g < 5; g++)
            {
                for (int i = 0; i < totalTiles; i++)
                {
                    Tile_LID leftN = tiles[GetNeighbor(i, Direction.Left)].ID;
                    Tile_LID rightN = tiles[GetNeighbor(i, Direction.Right)].ID;
                    Tile_LID upN = tiles[GetNeighbor(i, Direction.Up)].ID;
                    Tile_LID downN = tiles[GetNeighbor(i, Direction.Down)].ID;
                    
                    //check horizontal
                    if(leftN == Tile_LID.Water_Deep && leftN == rightN)
                    { tiles[i].ID = leftN; }

                    //check vertical
                    if (upN == Tile_LID.Water_Deep && upN == downN)
                    { tiles[i].ID = upN; }
                }
            }

            //add shallows on deep water if desert tile is neighbor
            for (int i = 0; i < totalTiles; i++)
            {
                if (tiles[i].ID == Tile_LID.Water_Deep)
                {
                    for (int p = 1; p < 9; p++)
                    {
                        int neighborID = GetNeighbor(i, (Direction)p);
                        if (tiles[neighborID].ID == Tile_LID.Desert)
                        { tiles[i].ID = Tile_LID.Water_Shallow; }
                    }
                }
            }

            //add grass around the desert tiles near deep water
            for (int i = 0; i < totalTiles; i++)
            {
                if (tiles[i].ID == Tile_LID.Desert)
                {
                    for (int p = 1; p < 9; p++)
                    {
                        int neighborID = GetNeighbor(i, (Direction)p);
                        if (tiles[neighborID].ID == Tile_LID.Water_Shallow)
                        { tiles[i].ID = Tile_LID.Grass; }
                    }
                }
            }

            //collect a list of current grass tiles
            List<int> grassTiles = new List<int>(500);
            for (int i = 0; i < totalTiles; i++)
            {
                if (tiles[i].ID == Tile_LID.Grass)
                { grassTiles.Add(i); }
            }
            //loop list of grass tiles, swap desert neighbors to grass
            for (int i = 0; i < grassTiles.Count; i++)
            {
                if (tiles[grassTiles[i]].ID == Tile_LID.Grass)
                {
                    for (int p = 1; p < 9; p++)
                    {
                        int neighborID = GetNeighbor(grassTiles[i], (Direction)p);
                        if (tiles[neighborID].ID == Tile_LID.Desert)
                        { tiles[neighborID].ID = Tile_LID.Grass; }
                    }
                }
            }





        }

        public static void GenMap_Artic()
        {
            Reset();
            //clear any current tileInfo on land
            ScreenManager.Land.tileInfo.position.X = 9999;
            ScreenManager.Land.tileInfo.text = "";
            ScreenManager.Land.selectedTile.X = 9999;

            //store planet type
            planetType = PlanetType.Artic;

            //reset land to snow/ice
            for (int i = 0; i < totalTiles; i++)
            {
                tiles[i].ID = Tile_LID.Snow;
            }

            //start with two 'city' locations
            int cityA = 1228;
            int cityB = 2209;

            //create dirt islands
            GenIsland(cityA, Tile_LID.Dirt_Brown, 10);
            GenIsland(cityB, Tile_LID.Dirt_Brown, 10);

            //create dirt islands between cities
            GenIsland(1630, Tile_LID.Dirt_Brown, 10);
            GenIsland(1807, Tile_LID.Dirt_Brown, 10);

            //create dirt islands in corners
            GenIsland(853, Tile_LID.Dirt_Brown, 3);
            GenIsland(2660, Tile_LID.Dirt_Brown, 3);




            //fill in lone tiles with Dirt_Brown
            for (int g = 0; g < 5; g++)
            {
                for (int i = 0; i < totalTiles; i++)
                {
                    Tile_LID leftN = tiles[GetNeighbor(i, Direction.Left)].ID;
                    Tile_LID rightN = tiles[GetNeighbor(i, Direction.Right)].ID;
                    Tile_LID upN = tiles[GetNeighbor(i, Direction.Up)].ID;
                    Tile_LID downN = tiles[GetNeighbor(i, Direction.Down)].ID;

                    //check horizontal
                    if (leftN == Tile_LID.Dirt_Brown && leftN == rightN)
                    { tiles[i].ID = leftN; }

                    //check vertical
                    if (upN == Tile_LID.Dirt_Brown && upN == downN)
                    { tiles[i].ID = upN; }
                }
            }
        }

        public static void GenMap_Moon()
        {
            Reset();
            //clear any current tileInfo on land
            ScreenManager.Land.tileInfo.position.X = 9999;
            ScreenManager.Land.tileInfo.text = "";
            ScreenManager.Land.selectedTile.X = 9999;

            //store planet type
            planetType = PlanetType.Moon;

            //reset land to dirt
            for (int i = 0; i < totalTiles; i++)
            {
                tiles[i].ID = Tile_LID.Dirt_Gray;
            }

            //start with two 'city' locations
            int cityA = 1228;
            int cityB = 2209;

            //create dirt islands
            GenIsland(cityA, Tile_LID.Dirt_Brown, 10);
            GenIsland(cityB, Tile_LID.Dirt_Brown, 10);

            //create dirt islands between cities
            GenIsland(1630, Tile_LID.Dirt_Brown, 10);
            GenIsland(1807, Tile_LID.Dirt_Brown, 10);

            //create dirt islands in corners
            GenIsland(853, Tile_LID.Dirt_Brown, 3);
            GenIsland(2660, Tile_LID.Dirt_Brown, 3);




            //fill in lone tiles with Dirt_Brown
            for (int g = 0; g < 5; g++)
            {
                for (int i = 0; i < totalTiles; i++)
                {
                    Tile_LID leftN = tiles[GetNeighbor(i, Direction.Left)].ID;
                    Tile_LID rightN = tiles[GetNeighbor(i, Direction.Right)].ID;
                    Tile_LID upN = tiles[GetNeighbor(i, Direction.Up)].ID;
                    Tile_LID downN = tiles[GetNeighbor(i, Direction.Down)].ID;

                    //check horizontal
                    if (leftN == Tile_LID.Dirt_Brown && leftN == rightN)
                    { tiles[i].ID = leftN; }

                    //check vertical
                    if (upN == Tile_LID.Dirt_Brown && upN == downN)
                    { tiles[i].ID = upN; }
                }
            }
        }

        //save tiles to file with string

        public static bool SaveThePlanet(string name)
        {
            byte[] planetData;

            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    //write an ascii header
                    writer.Write((byte)'A'); writer.Write((byte)'S');
                    writer.Write((byte)'T'); writer.Write((byte)'R');
                    writer.Write((byte)'O'); writer.Write((byte)'4');
                    writer.Write((byte)'X'); writer.Write((byte)'-');

                    writer.Write((byte)'P'); writer.Write((byte)'L');
                    writer.Write((byte)'A'); writer.Write((byte)'N');
                    writer.Write((byte)'E'); writer.Write((byte)'T');
                    writer.Write((byte)'-');

                    writer.Write((byte)'F'); writer.Write((byte)'I');
                    writer.Write((byte)'L'); writer.Write((byte)'E');
                    writer.Write((byte)'-');

                    writer.Write((byte)'V'); writer.Write((byte)'0');
                    writer.Write((byte)'.'); writer.Write((byte)'1');
                    writer.Write((byte)'-');

                    //write planet type
                    writer.Write((byte)planetType);

                    //write all tile ids to byte array
                    for (int i = 0; i < totalTiles; i++)
                    { writer.Write((byte)tiles[i].ID); }
                    
                    planetData = stream.ToArray();
                }
            }

            //C:\Users\<user>\AppData\Roaming\ - roaming is per user
            string savePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Astro4X");

            //create dev save data folder
            if (Directory.Exists(savePath) == false) { Directory.CreateDirectory(savePath); }

            //determine file name and absolute path
            string filePath = Path.Combine(savePath, name + ".astro");

            try
            {   //try writing land data to file
                using (var stream = File.Open(filePath, FileMode.Create, FileAccess.Write))
                { stream.Write(planetData, 0, planetData.Length); }
                Console.WriteLine("land file written to " + filePath);
                Console.WriteLine("total bytes written: " + planetData.Length);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Save Failed: " + e.ToString());
                return false;
            }
        }

        public static void LoadThePlanet(byte[] planetData)
        {
            try
            {
                //this may fail on older planet file formats
                using (MemoryStream stream = new MemoryStream(planetData))
                {
                    using (BinaryReader reader = new BinaryReader(stream))
                    {
                        //read ascii header
                        for (int i = 0; i < 25; i++)
                        { reader.ReadByte(); }

                        //read planet type
                        planetType = (PlanetType)reader.ReadByte();

                        //read tile data
                        for (int i = 0; i < totalTiles; i++)
                        { tiles[i].ID = (Tile_LID)reader.ReadByte(); }
                    }
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
        }

    }
}