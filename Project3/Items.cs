using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Timers;
using System;
using System.Collections.Generic;

namespace BulletHell
{
 

    class Coin : GameObject
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
            UpdatePos(this.GetPos() + this.CalcDir());
        }

        private Vector2 CalcDir()
        {
            var angle = Math.Atan2(Hero1.GetPos().Y - this.GetPos().Y, Hero1.GetPos().X - this.GetPos().X);
            var dir = new Vector2((float)(Math.Cos(angle) * 5), (float)(Math.Sin(angle) * 5));
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
}
