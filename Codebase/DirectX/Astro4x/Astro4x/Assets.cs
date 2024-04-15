using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Astro4x
{
    public static class Assets
    {
        public static Texture2D lineTexture;
        public static SpriteFont Assets_font_eng;
        public static Texture2D sheet_Main;
        public static Texture2D sheet_Land;

        public static Color Color_DeepSea_Blue = new Color(36, 60, 90);
        public static Color Color_Mars_Orange = new Color(192, 137, 18);
        public static Color Color_Desert_Yellow = new Color(207, 203, 103);
        public static Color Color_Artic_Blue = new Color(162, 205, 206);
        public static Color Color_Moon_Gray = new Color(49, 49, 49);

        public static void Constructor()
        {
            lineTexture = new Texture2D(ScreenManager.GDM.GraphicsDevice, 1, 1);
            lineTexture.SetData<Color>(new Color[] { Color.White });

            Assets_font_eng = ScreenManager.CM.Load<SpriteFont>("font_ui_eng");
            sheet_Main = ScreenManager.CM.Load<Texture2D>(@"sheet_Main");
            sheet_Land = ScreenManager.CM.Load<Texture2D>(@"sheet_Land");
        }
    }
}