using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astro4x
{
    public struct SpriteStruct //est. 24 bytes
    {
        public int X, Y; //8 bytes
        public float layer, alpha; //8 bytes

        //8 bytes
        public byte draw_width;
        public byte draw_height;
        public byte draw_x;
        public byte draw_y;

        //public byte padding;
        //public byte padding;
        //public byte padding;
        //public byte padding;
        
    }
}