using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Timers;
using System;
using System.Collections.Generic;

namespace BulletHell
{
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        public static Texture2D empty;
        BasicEffect basicEffect;
        EffectPassCollection effectPassCollection;

        public double mouseX;
        public double mouseY;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Hero1.Texture2D = Content.Load<Texture2D>("hero1");
            Hero1.CountTextureSize(new Point(Hero1.Texture2D.Width, Hero1.Texture2D.Height));
            empty = Content.Load<Texture2D>("empty");
            Objects.Init(spriteBatch, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            Projectile.Texture2D = Content.Load<Texture2D>("bullet");
            Coin.Texture2D = Content.Load<Texture2D>("projactale");
            Coin.CountTextureSize(new Point(Coin.Texture2D.Width, Coin.Texture2D.Height));
            Chest.Texture2D = Content.Load<Texture2D>("chestClosed");
            Chest.SecTexture2D = Content.Load<Texture2D>("chestOpened");
            Door.Texture2D = Content.Load<Texture2D>("door");
            Door.SecTexture2D = Content.Load<Texture2D>("doorUD");
            Door.CountTextureSize(new Point(Door.Texture2D.Width, Door.Texture2D.Height), new Point(Door.SecTexture2D.Width, Door.SecTexture2D.Height));
            Level.InitLevel(GraphicsDevice, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            basicEffect = new BasicEffect(GraphicsDevice)
            {
                World = Matrix.CreateOrthographicOffCenter(
                0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 0, 0, 1)
            };
            effectPassCollection = basicEffect.Techniques[0].Passes;
        }

        MouseState mouseState, oldMouseState;

        protected override void Update(GameTime gameTime)
        {
            if (IsActive)
            {
                var keyboardState = Keyboard.GetState();
                mouseState = Mouse.GetState();
                Objects.Update();
                Level.Update();
                if (keyboardState.IsKeyDown(Keys.W)) MoveSet.MoveUp();
                if (keyboardState.IsKeyDown(Keys.S)) MoveSet.MoveDown();
                if (keyboardState.IsKeyDown(Keys.D)) MoveSet.MoveRight();
                if (keyboardState.IsKeyDown(Keys.A)) MoveSet.MoveLeft();
                if (((int)mouseState.LeftButton == 1) && ((int)oldMouseState.LeftButton == 0)) Objects.Fire();
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                    Exit();
                oldMouseState = mouseState;
                base.Update(gameTime);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkGray);

            spriteBatch.Begin(SpriteSortMode.BackToFront);
            if (mouseState.X < Hero1.GetPos().X + 30)
                spriteBatch.Draw(Hero1.Texture2D, Hero1.GetPos(), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);
            else
                spriteBatch.Draw(Hero1.Texture2D, Hero1.GetPos(), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0); ;
            Objects.Draw();
            Level.Draw(GraphicsDevice, effectPassCollection);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
