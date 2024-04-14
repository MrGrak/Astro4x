using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astro4x
{
    public enum TileID : byte
    {
        Grass, Dirt_Mars, Dirt_Brown,
        Desert, Snow, Water_Shallow,
        Water_Deep, Dirt_Gray,
    }
    public struct Tile //est. 1 bytes
    {
        public TileID ID;
    }
}