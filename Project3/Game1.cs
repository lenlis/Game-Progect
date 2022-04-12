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
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        public static Texture2D hero1;
        public static Texture2D hero2;
        public double mouseX;
        public double mouseY;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            hero1 = Content.Load<Texture2D>("hero1");
            hero2 = Content.Load<Texture2D>("hero2");
            Objects.Init(spriteBatch, 100000, 100000);
            Projectile.Texture2D = Content.Load<Texture2D>("bulet");
            // TODO: use this.Content to load your game content here
        }

        MouseState mouseState, oldMouseState;

        protected override void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();
            Objects.Update();
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

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            spriteBatch.Begin();
            if(mouseState.X < Hero1.GetPos().X)
                spriteBatch.Draw(hero2, Hero1.GetPos(), Color.White);
            else
                spriteBatch.Draw(hero1, Hero1.GetPos(), Color.White);
            Objects.Draw();
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
