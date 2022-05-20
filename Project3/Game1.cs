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
        public static Texture2D map;
        public static Texture2D band;
        public static Texture2D gameOver;
        public static SpriteFont font;
        BasicEffect basicEffect;
        EffectPassCollection effectPassCollection;
        public enum GameState
        {
            MainMenu,
            GameLevel,
            Map,
            GameOver
        }
        GameState State = GameState.MainMenu;
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
            gameOver = Content.Load<Texture2D>("gameOver");
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("font");
            MainMenu.startB = Content.Load<Texture2D>("start");
            MainMenu.exitB = Content.Load<Texture2D>("exit");
            MainMenu.menuBack = Content.Load<Texture2D>("MenuBack");
            Hero1.Texture2D = Content.Load<Texture2D>("hero1");
            Hero1.CountTextureSize(new Point(Hero1.Texture2D.Width, Hero1.Texture2D.Height));
            empty = Content.Load<Texture2D>("empty");
            map = Content.Load<Texture2D>("map");
            band = Content.Load<Texture2D>("band");
            Objects.Init(spriteBatch, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            Projectile.Texture2D = Content.Load<Texture2D>("bullet");
            Projectile.ProjTexture2D = Content.Load<Texture2D>("projactale");
            Coin.Texture2D = Content.Load<Texture2D>("coin");
            Coin.CountTextureSize(new Point(Coin.Texture2D.Width, Coin.Texture2D.Height));
            SimpleEnem.Texture2D = Content.Load<Texture2D>("projactale");
            SimpleEnem.CountTextureSize(new Point(SimpleEnem.Texture2D.Width*2, SimpleEnem.Texture2D.Height*2));
            Chest.Texture2D = Content.Load<Texture2D>("chestClosed");
            Chest.SecTexture2D = Content.Load<Texture2D>("chestOpened");
            Door.Texture2D = Content.Load<Texture2D>("door");
            Door.SecTexture2D = Content.Load<Texture2D>("doorUD");
            Door.CountTextureSize(new Point(Door.Texture2D.Width, Door.Texture2D.Height), new Point(Door.SecTexture2D.Width, Door.SecTexture2D.Height));
            Level.marker = Content.Load<Texture2D>("marker");
            Level.Floor = Content.Load<Texture2D>("floor");
            Level.InitLevel(GraphicsDevice, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            var vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColor),
            4, BufferUsage.None);
            GraphicsDevice.SetVertexBuffer(vertexBuffer);
            basicEffect = new BasicEffect(GraphicsDevice)
            {
                VertexColorEnabled = true,
                World = Matrix.CreateOrthographicOffCenter(
                0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 0, 0, 1)
            };
            effectPassCollection = basicEffect.Techniques[0].Passes;
        }

        MouseState mouseState, oldMouseState;
        KeyboardState keyboardState, oldKeyboardState;
        protected override void Update(GameTime gameTime)
        {
            if (IsActive)
            {
                if (State == GameState.GameLevel)
                {
                    if (Hero1.GetHP() <= 0)
                    {
                        State = GameState.GameOver;
                        Level.ReInitLevel(GraphicsDevice, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);                     
                    }
                    keyboardState = Keyboard.GetState();
                    mouseState = Mouse.GetState();
                    Objects.Update();
                    Level.Update();
                    if (keyboardState.IsKeyDown(Keys.W)) MoveSet.MoveUp();
                    if (keyboardState.IsKeyDown(Keys.S)) MoveSet.MoveDown();
                    if (keyboardState.IsKeyDown(Keys.D)) MoveSet.MoveRight();
                    if (keyboardState.IsKeyDown(Keys.A)) MoveSet.MoveLeft();
                    if (Hero1.IsWeapReloaded()) 
                    {
                        if (((int)mouseState.LeftButton == 1) && ((int)oldMouseState.LeftButton == 0))
                        {
                            MoveSet.Soot();
                        }                           
                    }
                    else
                        Hero1.Reload();
                    if (keyboardState.IsKeyUp(Keys.Escape) && oldKeyboardState.IsKeyDown(Keys.Escape))
                        State = GameState.MainMenu;
                    if (keyboardState.IsKeyUp(Keys.M) && oldKeyboardState.IsKeyDown(Keys.M))
                        State = GameState.Map;
                    oldKeyboardState = keyboardState;
                    oldMouseState = mouseState;
                    base.Update(gameTime);
                }
                if (State == GameState.Map)
                {
                    keyboardState = Keyboard.GetState();
                    mouseState = Mouse.GetState();
                    if (keyboardState.IsKeyUp(Keys.Escape) && oldKeyboardState.IsKeyDown(Keys.Escape))
                        State = GameState.GameLevel;
                    if (keyboardState.IsKeyUp(Keys.M) && oldKeyboardState.IsKeyDown(Keys.M))
                        State = GameState.GameLevel;
                    oldKeyboardState = keyboardState;
                    oldMouseState = mouseState;
                    base.Update(gameTime);
                }
                if (State == GameState.GameOver)
                {
                    keyboardState = Keyboard.GetState();
                    mouseState = Mouse.GetState();
                    if (keyboardState.GetPressedKeyCount() > 0)
                        State = GameState.MainMenu;
                    if(((int)mouseState.LeftButton == 1) || ((int)oldMouseState.RightButton == 1) || ((int)oldMouseState.MiddleButton == 1))
                        State = GameState.MainMenu;
                    oldKeyboardState = keyboardState;
                    oldMouseState = mouseState;
                    base.Update(gameTime);
                }
                if (State == GameState.MainMenu)
                {
                    keyboardState = Keyboard.GetState();
                    mouseState = Mouse.GetState();
                    MainMenu.IsStartClick(mouseState);
                    MainMenu.IsExitClick(mouseState);
                    if (((int)mouseState.LeftButton == 1) && ((int)oldMouseState.LeftButton == 0))
                    {
                        if (MainMenu.IsStartClick(mouseState))
                            State = GameState.GameLevel;
                        if (MainMenu.IsExitClick(mouseState))
                            Exit();
                    }
                    oldKeyboardState = keyboardState;
                    oldMouseState = mouseState;   
                    base.Update(gameTime);
                }
            }
        }


        protected override void Draw(GameTime gameTime)
        {
            if (State == GameState.GameLevel)
            {
                GraphicsDevice.Clear(Color.DarkGray);
                spriteBatch.Begin(SpriteSortMode.BackToFront);
                spriteBatch.DrawString(font, Hero1.GetHP().ToString(), new Vector2(55, 15), Color.White);
                spriteBatch.Draw(band, new Vector2(5, 5), null, Color.White, 0, Vector2.Zero, 0.6f, SpriteEffects.None, 0);
                spriteBatch.Draw(Coin.Texture2D, new Vector2(105, 5), null, Color.White, 0, Vector2.Zero, 1.9f, SpriteEffects.None, 0);
                spriteBatch.DrawString(font, Hero1.GetCoinsValue().ToString(), new Vector2(160, 15), Color.White);
                if (mouseState.X < Hero1.GetPos().X + 30)
                    spriteBatch.Draw(Hero1.Texture2D, Hero1.GetPos(), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);
                else
                    spriteBatch.Draw(Hero1.Texture2D, Hero1.GetPos(), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0); 
                Objects.Draw();
                Level.Draw(GraphicsDevice, effectPassCollection);
                spriteBatch.End();
                base.Draw(gameTime);
            }
            if (State == GameState.Map)
            {
                GraphicsDevice.Clear(Color.DarkGray);
                spriteBatch.Begin();
                spriteBatch.Draw(map, new Vector2(210, -1), null, Color.White, 0, Vector2.Zero, 2f, SpriteEffects.None, 0);
                spriteBatch.End();
                Level.DrawMap(GraphicsDevice, basicEffect);

                base.Draw(gameTime);
            }
            if (State == GameState.GameOver)
            {
                GraphicsDevice.Clear(Color.Black);
                spriteBatch.Begin();
                spriteBatch.Draw(gameOver, new Vector2(0, 0), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                spriteBatch.End();
                base.Draw(gameTime);
            }
            if (State == GameState.MainMenu)
            {
                GraphicsDevice.Clear(Color.DarkGray);
                spriteBatch.Begin();
                MainMenu.Draw(spriteBatch);
                spriteBatch.End();
                base.Draw(gameTime);
            }
        }
    }
}
