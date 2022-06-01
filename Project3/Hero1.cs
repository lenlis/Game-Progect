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
        static int HP = 25;
        static int maxHP = 25;
        static int coins;
        static float reloadTimer = 0;
        static bool isReloaded = true;
        public static int weaponInHand = 0;
        static public readonly List<IWeapon> inventory = new List<IWeapon>();
        public static Texture2D Texture2D { get; set; }
        private static Point TextureSize;

        public static void Reload()
        {
            if (reloadTimer < inventory[weaponInHand].GetReloadSpeed())
            {
                isReloaded = false;
                reloadTimer++;
            }
            else
            {
                isReloaded = true;
                reloadTimer = 0;
            }
        }
        public static bool IsWeapReloaded()
        {
            return isReloaded;
        }
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
            if (HP + shift <= maxHP)
                HP += shift;
            else
                HP = maxHP;
        }
        public static void ChengeMaxHP(int shift)
        {
            maxHP += shift;
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
