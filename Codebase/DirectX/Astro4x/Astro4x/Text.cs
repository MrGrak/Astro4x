using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astro4x
{
    public class Text
    {
        public SpriteFont font;
        public String text;            
        public Vector2 position;        
        public Color color;             
        public float alpha = 1.0f;      
        public float rotation = 0.0f;
        public float scale = 1.0f;
        public float layer = 0.001f;  

        public Text(String Text,
            Vector2 Position, Color Color)
        {
            position = Position;
            text = Text;
            color = Color;
            font = Assets.Assets_font_eng;
        }

    }
}
