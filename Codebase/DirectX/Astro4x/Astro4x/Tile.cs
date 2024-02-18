using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astro4x
{
    public enum TileID : byte
    {
        Default,
        Grass, Forest, Plains, Snow, Dirt, Desert,
        Water_Shallow, Water_Deep,
    }
    public struct Tile //est. 1 bytes
    {
        public TileID ID;
        public byte Height;
        public byte Offset;
    }
}