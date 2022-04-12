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
        static public int windowHigth;
        
        static public SpriteBatch spriteBatch;
        public static Vector2 Position = new Vector2();
        Color color = Color.White;
        static List<Projectile> projectiles = new List<Projectile>();

        
        public static void Init(SpriteBatch spriteBatch, int Width, int Higth)
        {
            windowWidth = Width;
            windowHigth = Higth;
            Objects.spriteBatch = spriteBatch;
        }

        public static void Fire()
        {
            projectiles.Add(new Projectile());
            projectiles[projectiles.Count - 1].position = new Vector2(Hero1.GetPos().X + 20, Hero1.GetPos().Y);
            projectiles[projectiles.Count - 1].heroPos = Hero1.GetPos();
            projectiles[projectiles.Count - 1].cursorePos = new Point(Mouse.GetState().X, Mouse.GetState().Y);
        }

        public static void Update()
        {
            for(int i = 0; i < projectiles.Count; i++)
            {
                projectiles[i].Update();
                if (projectiles[i].Hiden)
                {
                    projectiles.RemoveAt(i);
                    i--;
                }
            }

        }

        static public void Draw()
        {
            foreach (Projectile proj in projectiles)
            {
                proj.Draw();
            }
        }
    }

    class Projectile
    {
        public Point cursorePos = new Point();
        public Vector2 position = new Vector2();
        public Vector2 heroPos = new Vector2();
        Color color = Color.White;

        public static Texture2D Texture2D { get; set; }
        public bool Hiden
        {
            get
            {
                return (position.X > Objects.windowWidth || position.Y > Objects.windowHigth);
            }
        }

        public void Update()
        {
                position += CalcDir()*2;
        }

        private Vector2 CalcDir()
        {
            var angle = Math.Atan2(cursorePos.Y - heroPos.Y, cursorePos.X - heroPos.X);
            var dir = new Vector2((float)(Math.Cos(angle) * 5), (float)(Math.Sin(angle) * 5));
            return dir;
        }

        public void Draw()
        {
            Objects.spriteBatch.Draw(Texture2D, position, color);
        }
    }

}
