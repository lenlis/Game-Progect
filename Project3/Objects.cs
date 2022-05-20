using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Timers;
using System;
using System.Collections.Generic;

namespace BulletHell
{
    public class Objects
    {
        static public int windowWidth;
        static public int windowHeight;   
        static public SpriteBatch spriteBatch;
        public static Vector2 Position = new Vector2();
        public static readonly List<Projectile> projectiles = new List<Projectile>();
        static readonly List<Projectile> enProjectiles = new List<Projectile>();
        static readonly List<Coin> coins = new List<Coin>();
        public static bool HeroDirIsRirht;

        public static void DelAllProj()
        {
            for (int i = 0; i < projectiles.Count; i++)
            { 
                    projectiles.RemoveAt(i);
                    i--;
            }
            for (int i = 0; i < enProjectiles.Count; i++)
            {
                enProjectiles.RemoveAt(i);
                i--;
            }

        }
        public static void Init(SpriteBatch spriteBatch, int Width, int Height)
        {
            windowWidth = Width;
            windowHeight = Height;
            Objects.spriteBatch = spriteBatch;
        }

        public static void AddCoins(Vector2 Pos)
        {
            coins.Add(new Coin());
            coins[^1].UpdatePos(Pos);
        }
        public static void Fire()
        {
            projectiles.Add(new Projectile());
            projectiles[^1].InitTexture();
            if (Mouse.GetState().X > Hero1.GetPos().X + 30)
            {
                projectiles[^1].position = new Vector2(Hero1.GetPos().X + 35, Hero1.GetPos().Y + 30);
                projectiles[^1].startPos = new Vector2(Hero1.GetPos().X + 35, Hero1.GetPos().Y + 30);
                HeroDirIsRirht = true;
            }
            else
            {
                projectiles[^1].position = new Vector2(Hero1.GetPos().X + 15, Hero1.GetPos().Y + 30);
                projectiles[^1].startPos = new Vector2(Hero1.GetPos().X + 15, Hero1.GetPos().Y + 30);
                HeroDirIsRirht = false;
            }
            
            projectiles[^1].cursorePos = new Point(Mouse.GetState().X, Mouse.GetState().Y);
        }

        public static void EnFire(Vector2 enPos)
        {
            enProjectiles.Add(new Projectile());
            enProjectiles[^1].enemyPrj = true;
            enProjectiles[^1].InitTexture();
            if (Hero1.GetPos().X > enPos.X + 30)
            {
                enProjectiles[^1].position = new Vector2(enPos.X + 30, enPos.Y + 60);
                enProjectiles[^1].startPos = new Vector2(enPos.X + 30, enPos.Y + 60);
            }
            else
            {
                enProjectiles[^1].position = new Vector2(enPos.X + 30, enPos.Y + 60);
                enProjectiles[^1].startPos = new Vector2(enPos.X + 30, enPos.Y + 60);
            }

            enProjectiles[^1].cursorePos = new Point((int)Hero1.GetPos().X, (int)Hero1.GetPos().Y);
        }

        public static void Update()
        {
            for(int i = 0; i < projectiles.Count; i++)
            {
                projectiles[i].Update();
                if (projectiles[i].Hiden || projectiles[i].forceTic > 5)
                {
                    projectiles.RemoveAt(i);
                    i--;
                }
            }

            for (int i = 0; i < enProjectiles.Count; i++)
            {
                if (enProjectiles[i].GetCollusion().Intersects(Hero1.CountCollusion()))
                {
                    enProjectiles.RemoveAt(i);
                    i--;
                    Hero1.ChengeHP(-1);
                    continue;
                }
                enProjectiles[i].Update();
                if (enProjectiles[i].Hiden || enProjectiles[i].stopedTic > 100)
                {
                    enProjectiles.RemoveAt(i);
                    i--;
                }
            }

            for (int i = 0; i < coins.Count; i++)
            {
                coins[i].PullToHero();
                if (coins[i].Garbed)
                {
                    coins.RemoveAt(i);
                    i--;
                    Hero1.ChengeCoinsValue(1);
                }
            }

        }

        static public void Draw()
        {
            foreach (Coin coin in coins)
            {
                coin.Draw();
            }

            foreach (Projectile proj in projectiles)
            {
                proj.Draw();
            }

            foreach (Projectile proj in enProjectiles)
            {
                proj.Draw();
            }
        }
    }

    public class Projectile
    {
        public Point cursorePos = new Point();
        public Vector2 position = new Vector2();
        public Vector2 startPos = new Vector2();
        private float rotationAngle;
        private float speed = 3;
        public bool enemyPrj;
        public int stopedTic;
        public bool damageGained = false;
        public int forceTic;
        Color color = Color.White;
        private Point TextureSize;

        public static Texture2D Texture2D { get; set; }
        public static Texture2D ProjTexture2D { get; set; }
        public Texture2D Texture { get; set; }
        public bool Hiden
        {
            get
            {
                return (position.X < Level.roomPos.X ||
                    position.Y < Level.roomPos.Y - Hero1.Texture2D.Height ||
                    position.X > Objects.windowWidth - 15 - Level.roomPos.X ||
                    position.Y > Objects.windowHeight - 15 - Level.roomPos.Y);
            }
        }

        public Rectangle GetCollusion()
        {
            if (enemyPrj)
                CountTextureSize(ProjTexture2D);
            else
                CountTextureSize(Texture2D);
            return new Rectangle((int)position.X,
            (int)this.position.Y, TextureSize.X, TextureSize.Y);
        }
        public void InitTexture()
        {
            if(enemyPrj)
            Texture = ProjTexture2D;
            else
            Texture = Texture2D;     
        }

        public void CountTextureSize(Texture2D texture)
        {
            TextureSize = new Point(texture.Width, texture.Height);
        }

        public void Update()
        {
            if (damageGained)
                forceTic++;
            if (speed <= 0.5f)
                stopedTic++;
            else
            if (enemyPrj)
                speed -= 0.05f;
            position += CalcDir()*speed;
        }

        public Vector2 CalcDir()
        {
            var angle = Math.Atan2(cursorePos.Y - startPos.Y, cursorePos.X - startPos.X);
            rotationAngle = (float)angle;
            var dir = new Vector2((float)(Math.Cos(angle) * 5), (float)(Math.Sin(angle) * 5));
            return dir;
        }

        public void Draw()
        {
            if (!enemyPrj)
            {
                if (position == startPos)
                    Objects.spriteBatch.Draw(Game1.empty, position, null, color, rotationAngle, Vector2.Zero, 1, SpriteEffects.None, 0);
                else if (Objects.HeroDirIsRirht)
                {
                    Objects.spriteBatch.Draw(Texture, position, null, color, rotationAngle, Vector2.Zero, 1, SpriteEffects.None, 0);
                }
                else
                {
                    Objects.spriteBatch.Draw(Texture, position, null, color, (float)(rotationAngle - Math.PI), Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);
                }
            }
            else
                Objects.spriteBatch.Draw(Texture, position, null, color, 0, Vector2.Zero, 1, SpriteEffects.None, 0);

        }
    }

    public class Chest : GameObject
    {
        public bool closed;
        public static Texture2D Texture2D { get; set; }
        public static Texture2D SecTexture2D { get; set; }
        private static Point TextureSize;
        public void Open(Random rand)
        {
            ChengeCondition(true);
            for(int i = 0; i < rand.Next(1, 20); i++)
                Objects.AddCoins(GetPos());
        }

        public Rectangle GetCollusion()
        {
            return new Rectangle((int)this.GetPos().X,
            (int)this.GetPos().Y, TextureSize.X, TextureSize.Y);
        }

        public static void CountTextureSize(Point size)
        {
            TextureSize = size;
        }

        public void Draw()
        {
            if (GetCondition())
                spriteBatch.Draw(SecTexture2D, GetPos(), null, Color.White, 0, Vector2.Zero, 2, SpriteEffects.None, 0.5f);
            else
                spriteBatch.Draw(Texture2D, GetPos(), null, Color.White, 0, Vector2.Zero, 2, SpriteEffects.None, 0.5f);
        }
    }
}
