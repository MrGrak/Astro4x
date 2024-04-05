using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astro4x
{
    public class UI_Button
    {
        public Color color_bkg;
        public Rectangle button;
        public Text text;
        
        public UI_Button(Vector2 pos, String txt, int width)
        {
            button = new Rectangle((int)pos.X, (int)pos.Y, width, 10);
            text = new Text(txt, pos, Color.White);
            
            text.layer = Layers.Debug_Text;
            color_bkg = new Color(100, 100, 100);
        }
        
        public void Draw()
        {
            ScreenManager.Draw(button, color_bkg, 1.0f, Layers.Debug_Btn);
            ScreenManager.Draw(text);
        }

    }
}
