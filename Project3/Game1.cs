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
            Hero1.Texture2D = Content.Load<Texture2D>("Hero");
            Hero1.CountTextureSize(new Point(Hero1.Texture2D.Width*2, Hero1.Texture2D.Height*2));
            empty = Content.Load<Texture2D>("empty");
            map = Content.Load<Texture2D>("map");
            band = Content.Load<Texture2D>("band");
            Portal.Texture2D = Content.Load<Texture2D>("portal");
            Showcase.Texture2D = Content.Load<Texture2D>("shCase");
            HealPotion.ItemTexture = Content.Load<Texture2D>("HPotion");
            BigHealPotion.ItemTexture = Content.Load<Texture2D>("BigHPotion");
            StrangePotion.ItemTexture = Content.Load<Texture2D>("StrPotion");
            RandomPotion.ItemTexture = Content.Load<Texture2D>("RandPotion");
            Ammo.ItemTexture = Content.Load<Texture2D>("AMMO");
            Heart.ItemTexture = Content.Load<Texture2D>("Heart");
            PistolItem.ItemTexture = Content.Load<Texture2D>("Pistol");
            SimpleShotGunItem.ItemTexture = Content.Load<Texture2D>("SimShGun");
            CrosBowItem.ItemTexture = Content.Load<Texture2D>("CrossBow");
            SubMachineGunItem.ItemTexture = Content.Load<Texture2D>("SubMaGun");
            MachineGunPistolItem.ItemTexture = Content.Load<Texture2D>("SimMaGun");
            DeagleItem.ItemTexture = Content.Load<Texture2D>("Deagle");
            PPShItem.ItemTexture = Content.Load<Texture2D>("PPSh");
            P2070Item.ItemTexture = Content.Load<Texture2D>("P2070");
            SniperRifleItem.ItemTexture = Content.Load<Texture2D>("SniperRif");
            RifleItem.ItemTexture = Content.Load<Texture2D>("Rif");
            BenelleShotGunItem.ItemTexture = Content.Load<Texture2D>("BenShGun");
            RemingtonShotGunItem.ItemTexture = Content.Load<Texture2D>("RemShGun");
            Pistol.GunTexture = Content.Load<Texture2D>("Pistol");
            SimpleShotGun.GunTexture = Content.Load<Texture2D>("SimShGun");
            CrosBow.GunTexture = Content.Load<Texture2D>("CrossBow");
            SubMachineGun.GunTexture = Content.Load<Texture2D>("SubMaGun");
            MachineGunPistol.GunTexture = Content.Load<Texture2D>("SimMaGun");
            PPSh.GunTexture = Content.Load<Texture2D>("PPSh");
            P2070.GunTexture = Content.Load<Texture2D>("P2070");
            SniperRifle.GunTexture = Content.Load<Texture2D>("SniperRif");
            Rifle.GunTexture = Content.Load<Texture2D>("Rif");
            BenelleShotGun.GunTexture = Content.Load<Texture2D>("BenShGun");
            RemingtonShotGun.GunTexture = Content.Load<Texture2D>("RemShGun");
            Pistol.PrjTexture = Content.Load<Texture2D>("bullet");
            SimpleShotGun.PrjTexture = Content.Load<Texture2D>("bullet");
            CrosBow.PrjTexture = Content.Load<Texture2D>("arrow");
            SubMachineGun.PrjTexture = Content.Load<Texture2D>("bullet");
            MachineGunPistol.PrjTexture = Content.Load<Texture2D>("bullet");
            PPSh.PrjTexture = Content.Load<Texture2D>("bullet");
            P2070.PrjTexture = Content.Load<Texture2D>("bullet");
            SniperRifle.PrjTexture = Content.Load<Texture2D>("bullet");
            Rifle.PrjTexture = Content.Load<Texture2D>("bullet");
            BenelleShotGun.PrjTexture = Content.Load<Texture2D>("bullet");;
            Objects.Init(GraphicsDevice ,spriteBatch, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            RemingtonShotGun.PrjTexture = Content.Load<Texture2D>("bullet");
            Deagle.PrjTexture = Content.Load<Texture2D>("bullet");
            Deagle.GunTexture = Content.Load<Texture2D>("Deagle");
            Objects.ProjTexture2D = Content.Load<Texture2D>("projactale");
            Coin.Texture2D = Content.Load<Texture2D>("coin");
            Key.Texture2D = Content.Load<Texture2D>("key");
            Coin.CountTextureSize(new Point(Coin.Texture2D.Width, Coin.Texture2D.Height));
            SimpleEnem.Texture2D = Content.Load<Texture2D>("Enemy");
            SimpleEnem.CountTextureSizeAndCicles(new Point(SimpleEnem.Texture2D.Width*2, SimpleEnem.Texture2D.Height*2));
            Chest.Texture2D = Content.Load<Texture2D>("chestClosed");
            Chest.SecTexture2D = Content.Load<Texture2D>("chestOpened");
            Door.Texture2D = Content.Load<Texture2D>("door");
            Door.SecTexture2D = Content.Load<Texture2D>("doorUD");
            Door.CountTextureSize(new Point(Door.Texture2D.Width, Door.Texture2D.Height), new Point(Door.SecTexture2D.Width, Door.SecTexture2D.Height));
            Level.Marker = Content.Load<Texture2D>("marker");
            Level.Torch = Content.Load<Texture2D>("torch");
            Level.Floor = Content.Load<Texture2D>("floor");
            Level.Wall = Content.Load<Texture2D>("wall");
            Hero1.inventory.Add(new Pistol());
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
        long scrolWalue;
        protected override void Update(GameTime gameTime)
        {
            if (IsActive)
            {
                if (State == GameState.GameLevel)
                {
                    if (Hero1.GetHP() <= 0)
                    {
                        State = GameState.GameOver;
                        Level.LevelCount = 0;
                        Level.ReInitLevel(GraphicsDevice, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);                     
                    }
                    keyboardState = Keyboard.GetState();
                    mouseState = Mouse.GetState();

                    Level.Update();
                    if (keyboardState.IsKeyDown(Keys.W)) MoveSet.MoveUp();
                    if (keyboardState.IsKeyDown(Keys.S)) MoveSet.MoveDown();
                    if (keyboardState.IsKeyDown(Keys.D)) MoveSet.MoveRight();
                    if (keyboardState.IsKeyDown(Keys.A)) MoveSet.MoveLeft();
                    if (keyboardState.IsKeyDown(Keys.E)) Level.InteractWithItemsAndObjects();
                    var scrolShift = scrolWalue - mouseState.ScrollWheelValue;
                    if (scrolShift < 0 || scrolShift > 0)
                        MoveSet.ChangeWeapon(scrolShift);
                    if (Hero1.IsWeapReloaded()) 
                    {
                        if ((int)mouseState.LeftButton == 1)
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
                    scrolWalue = mouseState.ScrollWheelValue;
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
        public static bool CheckDoorOpening(int doorID)
        {
            var keyboardState = Keyboard.GetState();
            return doorID switch
            {
                0 => keyboardState.IsKeyDown(Keys.D),
                1 => keyboardState.IsKeyDown(Keys.A),
                2 => keyboardState.IsKeyDown(Keys.W),
                3 => keyboardState.IsKeyDown(Keys.S),
                _ => false,
            };
        }

        protected override void Draw(GameTime gameTime)
        {
            if (State == GameState.GameLevel)
            {
                GraphicsDevice.Clear(new Color(50,50,50,255));
                spriteBatch.Begin(SpriteSortMode.BackToFront);
                spriteBatch.DrawString(font, Hero1.GetHP().ToString(), new Vector2(55, 15), Color.White);
                spriteBatch.Draw(band, new Vector2(5, 5), null, Color.White, 0, Vector2.Zero, 0.6f, SpriteEffects.None, 0);
                spriteBatch.Draw(Coin.Texture2D, new Vector2(105, 5), null, Color.White, 0, Vector2.Zero, 1.9f, SpriteEffects.None, 0);
                spriteBatch.DrawString(font, Hero1.GetCoinsValue().ToString(), new Vector2(160, 15), Color.White);
                spriteBatch.Draw(Key.Texture2D, new Vector2(220, 18), null, Color.White, 0, Vector2.Zero, 2f, SpriteEffects.None, 0);
                spriteBatch.DrawString(font, Hero1.GetKeysValue().ToString(), new Vector2(260, 15), Color.White);
                Hero1.Draw(Mouse.GetState(), spriteBatch);
                Level.Draw(GraphicsDevice, effectPassCollection);
                spriteBatch.End();
                base.Draw(gameTime);
            }
            if (State == GameState.Map)
            {
                GraphicsDevice.Clear(Color.Black);
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
                GraphicsDevice.Clear(Color.Black);
                spriteBatch.Begin();
                MainMenu.Draw(spriteBatch);
                spriteBatch.End();
                base.Draw(gameTime);
            }
        }
    }
}
