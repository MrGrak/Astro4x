using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Astro4x
{
    public class Game1 : Game
    {
        public Game1()
        {
            //set data game refs
            ScreenManager.GDM = new GraphicsDeviceManager(this);
            ScreenManager.GDM.GraphicsProfile = GraphicsProfile.HiDef;
            ScreenManager.CM = Content;
            ScreenManager.GAME = this;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.IsBorderless = false;
            IsFixedTimeStep = true;
            ScreenManager.GDM.SynchronizeWithVerticalRetrace = true;

            //seto to 1280x720, for now
            ScreenManager.GDM.PreferredBackBufferWidth = 
                ScreenManager.WINDOW_WIDTH;
            ScreenManager.GDM.PreferredBackBufferHeight =
                ScreenManager.WINDOW_HEIGHT;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }
        protected override void LoadContent()
        {
            //setup sprite batch, camera rt
            ScreenManager.SB = new SpriteBatch(GraphicsDevice);
            ScreenManager.RT2D = new RenderTarget2D(
                GraphicsDevice,
                ScreenManager.GAME_WIDTH,
                ScreenManager.GAME_HEIGHT);
            
            //assets then systems, then boot
            Assets.Constructor();
            System_Land.Constructor();
            System_Universe.Constructor();

            ScreenManager.Constructor(); //sets boot screen
        }
        
        protected override void UnloadContent() { }
        
        protected override void Update(GameTime gameTime)
        {
            ScreenManager.Update();
        }
        
        protected override void Draw(GameTime gameTime)
        {
            ScreenManager.Draw();
        }
    }
}