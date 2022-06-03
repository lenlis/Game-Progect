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
        public static GraphicsDevice graphicsDevice;
        public static bool HeroDirIsRirht;
        public static Texture2D ProjTexture2D { get; set; }

        
        
        public static void Init(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, int Width, int Height)
        {
            windowWidth = Width;
            windowHeight = Height;
            Objects.spriteBatch = spriteBatch;
            Objects.graphicsDevice = graphicsDevice;
        }

        public static void AddCoins(Vector2 Pos, List<Coin> coins)
        {
            coins.Add(new Coin());
            coins[^1].UpdatePos(Pos);
        }

        public static void AddKey(Vector2 Pos, List<Key> keys)
        {
            keys.Add(new Key());
            keys[^1].UpdatePos(Pos);
        }

        public static void CreateProj(Vector2 pos, Point target, Texture2D texture, float speed,
            List<Projectile> projectiles, bool isEnPrj, float damage, float deflection)
        {
            Vector2 start;
            if (target.X > pos.X + 30)
            {
                if (!isEnPrj)
                    start = Hero1.CalcGunPos(false);
                else
                    start = new Vector2(pos.X + SimpleEnem.Texture2D.Width / 2, pos.Y + SimpleEnem.Texture2D.Height / 2);
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
                if (!isEnPrj)
                    start = Hero1.CalcGunPos(true);
                else
                    start = new Vector2(pos.X + SimpleEnem.Texture2D.Width / 2, pos.Y + SimpleEnem.Texture2D.Height / 2);
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

        public static void Update(List<Projectile> projectiles, List<Projectile> enProjectiles, List<Coin> coins, List<Key> keys)
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

            for (int i = 0; i < keys.Count; i++)
            {
                keys[i].PullToHero();
                if (keys[i].Garbed)
                {
                    keys.RemoveAt(i);
                    i--;
                    Hero1.ChengeKeysValue(1);
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
                if (position == startPos)
                    Objects.spriteBatch.Draw(Game1.empty, position, null, color, rotationAngle, Vector2.Zero, 1, SpriteEffects.None, 0);
                else
                    Objects.spriteBatch.Draw(Texture, position, null, color, rotationAngle, Vector2.Zero, 1, SpriteEffects.None, 0);   
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
            var lotChance = rand.Next(0, 100);
            ChengeCondition(true);
            if (lotChance <= 20)
                for (int i = 0; i < rand.Next(1, 20); i++)
                    Objects.AddCoins(GetPos(), Level.rooms[Level.GetHeroInRoomValue()].coins);
            else if (lotChance <= 50)
                Loot.AddLoot(4, GetPos(), Level.rooms[Level.GetHeroInRoomValue()].items);
            else
                Loot.AddLoot(rand.Next(0, 17), GetPos(), Level.rooms[Level.GetHeroInRoomValue()].items);


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

    public class Coin : GameObject
    {
        public static Texture2D Texture2D { get; set; }
        private static Point TextureSize;

        public static void CountTextureSize(Point size)
        {
            TextureSize = size;
        }

        public Rectangle GetCollusion()
        {
            return new Rectangle((int)this.GetPos().X,
            (int)this.GetPos().Y, TextureSize.X, TextureSize.Y);
        }

        public void PullToHero()
        {
            UpdatePos(this.GetPos() + this.CalcDir() * 3);
        }

        private Vector2 CalcDir()
        {
            var angle = Math.Atan2(Hero1.GetPos().Y - this.GetPos().Y, Hero1.GetPos().X - this.GetPos().X);
            var length = Math.Sqrt((Hero1.GetPos().X - this.GetPos().X) * (Hero1.GetPos().X - this.GetPos().X) +
                (Hero1.GetPos().Y - this.GetPos().Y) * (Hero1.GetPos().Y - this.GetPos().Y));
            var dir = new Vector2((float)(Math.Cos(angle) * 200 / length), (float)(Math.Sin(angle) * 200 / length));
            return dir;
        }

        public void Draw()
        {
            spriteBatch.Draw(Texture2D, GetPos(), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.5f);
        }

        public bool Garbed
        {
            get
            {
                return (GetCollusion().Intersects(Hero1.CountCollusion()));
            }
        }
    }

    public class Key : GameObject
    {
        public static Texture2D Texture2D { get; set; }
        private static Point TextureSize;

        public static void CountTextureSize(Point size)
        {
            TextureSize = size;
        }

        public Rectangle GetCollusion()
        {
            return new Rectangle((int)this.GetPos().X,
            (int)this.GetPos().Y, TextureSize.X, TextureSize.Y);
        }

        public void PullToHero()
        {
            UpdatePos(this.GetPos() + this.CalcDir() * 3);
        }

        private Vector2 CalcDir()
        {
            var angle = Math.Atan2(Hero1.GetPos().Y - this.GetPos().Y, Hero1.GetPos().X - this.GetPos().X);
            var length = Math.Sqrt((Hero1.GetPos().X - this.GetPos().X) * (Hero1.GetPos().X - this.GetPos().X) +
                (Hero1.GetPos().Y - this.GetPos().Y) * (Hero1.GetPos().Y - this.GetPos().Y));
            var dir = new Vector2((float)(Math.Cos(angle) * 50 / length), (float)(Math.Sin(angle) * 50 / length));
            return dir;
        }



        public void Draw()
        {
            spriteBatch.Draw(Texture2D, GetPos(), null, Color.White, 0, Vector2.Zero, 2, SpriteEffects.None, 0.5f);
        }

        public bool Garbed
        {
            get
            {
                return (GetCollusion().Intersects(Hero1.CountCollusion()));
            }
        }
    }

    public class Showcase : GameObject
    {
        public static Texture2D Texture2D { get; set; }
        public IItem ItemInCase;
        public int Price;  

        public Rectangle GetCollusion()
        {
            return new Rectangle((int)this.GetPos().X,
            (int)this.GetPos().Y, Texture2D.Width, Texture2D.Height);
        }

        public void Buy()
        {
            ChengeCondition(true);
            ItemInCase.UseItem();
        }

        public void Draw()
        {
            if (!GetCondition())
            {
                spriteBatch.Draw(Texture2D, GetPos(), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.9f);
                spriteBatch.Draw(ItemInCase.GetTexture(),
                    new Vector2(GetPos().X + ItemInCase.GetTexture().Width / 2 + Texture2D.Width / 2, GetPos().Y - ItemInCase.GetTexture().Height),
                    null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.9f);
                spriteBatch.DrawString(Game1.font, Price.ToString(), new Vector2(GetPos().X + 10, GetPos().Y + 10), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.8f);
            }
            else
                spriteBatch.Draw(Texture2D, GetPos(), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.9f);

        }
    }

    class Portal : GameObject
    {
        public static Texture2D Texture2D { get; set; }
        public IItem ItemInCase;
        public int Price;

        public Rectangle GetCollusion()
        {
            return new Rectangle((int)this.GetPos().X,
            (int)this.GetPos().Y, Texture2D.Width, Texture2D.Height);
        }

        public void GoToNextLevel()
        {
            Level.ReInitLevel(Objects.graphicsDevice, Objects.windowWidth, Objects.windowHeight);
        }
        public void Draw()
        {
            spriteBatch.Draw(Texture2D, GetPos(), null, Color.White, 0, Vector2.Zero, 0.3f, SpriteEffects.None, 0.9f);
        }
    }

}
