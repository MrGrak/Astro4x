using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astro4x
{
    public enum TileID : byte
    {
        
        Grass, Plains, Dirt, Desert, Snow, Water_Shallow, Water_Deep,

    }
    public struct Tile //est. 1 bytes
    {
        public TileID ID;
    }
}