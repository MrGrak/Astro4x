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

        //4 bytes
        public short draw_width;
        public short draw_height;

        //4 bytes
        public short draw_x;
        public short draw_y;
        
    }
}