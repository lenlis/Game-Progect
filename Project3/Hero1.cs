using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Timers;
using System;
using System.Collections.Generic;

namespace BulletHell
{
    public class Hero1
    {

        static Vector2 Position = new Vector2(10,10);
        static int HP;
        static int coins;

        public static Texture2D Texture2D { get; set; }


        private static Point TextureSize;


        public static void CountTextureSize(Point size)
        {
            TextureSize = size;
        }

        public static Rectangle CountCollusion()
        {
            return new Rectangle((int)Position.X,
            (int)Position.Y, TextureSize.X, TextureSize.Y);
        }
        public static Vector2 GetPos()
        {
            return Position;
        }

        public static void UpdatePos(Vector2 Pos)
        {
            Position.X = Pos.X;
            Position.Y = Pos.Y;
        }

        public static int GetHP()
        {
            return HP;
        }

        public static void ChengeHP(int shift)
        {
            HP += shift;
        }

        public static int GetCoinsValue()
        {
            return coins;
        }
        public static void ChengeCoinsValue(int shift)
        {
            coins += shift;
        }
    }
}
