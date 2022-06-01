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
        
        public static bool HeroDirIsRirht;
        public static Texture2D ProjTexture2D { get; set; }

        
        
        public static void Init(SpriteBatch spriteBatch, int Width, int Height)
        {
            windowWidth = Width;
            windowHeight = Height;
            Objects.spriteBatch = spriteBatch;
        }

        public static void AddCoins(Vector2 Pos, List<Coin> coins)
        {
            coins.Add(new Coin());
            coins[^1].UpdatePos(Pos);
        }
        public static void CreateProj(Vector2 pos, Point target, Texture2D texture, float speed,
            List<Projectile> projectiles, bool isEnPrj, float damage, float deflection)
        {
            var start = new Vector2(pos.X + Hero1.Texture2D.Width/2, pos.Y + Hero1.Texture2D.Height/2);
            if (target.X > pos.X + 30)
            {
                projectiles.Add(new Projectile()
                {
                    damage = damage,
                    Texture = texture,
                    speed = speed,
                    deflection = deflection,
                    position = start,
                    startPos = start,
                    cursorePos = target,
                    targetIsRight = true,
                    enemyPrj = isEnPrj
                }) ;

            }
            else
            {
                projectiles.Add(new Projectile() {
                    damage = damage,
                    Texture = texture,
                    speed = speed,
                    deflection = deflection,
                    position = start,
                    startPos = start,
                    cursorePos = target,
                    targetIsRight = false,
                    enemyPrj = isEnPrj
                });
            }           
        }

        public static void Update(List<Projectile> projectiles, List<Projectile> enProjectiles, List<Coin> coins)
        {
            for (int i = 0; i < projectiles.Count; i++)
            {
                projectiles[i].Update();
                if (projectiles[i].Hiden || projectiles[i].forceTic > Hero1.inventory[Hero1.weaponInHand].GetForceTic())
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
        
    }

    public class Projectile
    {
        public Point cursorePos = new Point();
        public Vector2 position = new Vector2();
        public Vector2 startPos = new Vector2();
        private float rotationAngle;
        public float speed;
        public float deflection;
        public float damage;
        public bool enemyPrj;
        public bool targetIsRight;
        public int stopedTic;
        public bool damageGained = false;
        public int forceTic;
        Color color = Color.White;
        private Point TextureSize;
        
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
            CountTextureSize(Texture);
            return new Rectangle((int)position.X,
            (int)this.position.Y, TextureSize.X, TextureSize.Y);
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
            var dir = new Vector2((float)(Math.Cos(angle + deflection) * 5), (float)(Math.Sin(angle + deflection) * 5));
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
                Objects.AddCoins(GetPos(), Level.rooms[Level.GetHeroInRoomValue()].coins);
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
