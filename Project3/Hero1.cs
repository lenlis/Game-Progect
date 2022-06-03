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
        static int keys;
        static float reloadTimer = 0;
        static bool isReloaded = true;
        public static int weaponInHand = 0;
        static public readonly List<IWeapon> inventory = new List<IWeapon>();
        public static Texture2D Texture2D { get; set; }
        public static Point TextureSize;

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

        public static int GetKeysValue()
        {
            return keys;
        }

        public static void ChengeCoinsValue(int shift)
        {
            coins += shift;
        }

        public static void ChengeKeysValue(int shift)
        {
            keys += shift;
        }

        public static void Draw( MouseState mouseState, SpriteBatch spriteBatch)
        {
            if (mouseState.X < Hero1.GetPos().X + 30)
            {

                spriteBatch.Draw(Hero1.Texture2D, Hero1.GetPos(), null, Color.White, 0, Vector2.Zero, 2, SpriteEffects.FlipHorizontally, 0.1f);
                spriteBatch.Draw(inventory[weaponInHand].GetGunTexture(), CalcGunPos(true), null, Color.White, (float)CalcAng(mouseState, CalcGunPos(true)), Vector2.Zero, 1, SpriteEffects.FlipVertically, 0);
            }
            else
            {

                spriteBatch.Draw(Hero1.Texture2D, Hero1.GetPos(), null, Color.White, 0, Vector2.Zero, 2, SpriteEffects.None, 0.1f);
                spriteBatch.Draw(inventory[weaponInHand].GetGunTexture(), CalcGunPos(false), null, Color.White, (float)CalcAng(mouseState, CalcGunPos(false)), Vector2.Zero, 1, SpriteEffects.None, 0);
            }
        }

        private static double CalcAng(MouseState mouseState, Vector2 Pos)
        {
            return Math.Atan2(mouseState.Y - Pos.Y, mouseState.X - Pos.X);
        }

        public static Vector2 CalcGunPos(bool deflection)
        {
            int intDef;
            if (deflection)
                intDef = 1;
            else
                intDef = -1;
            return new Vector2(Position.X + (TextureSize.X / 2) + intDef * (inventory[weaponInHand].GetGunTexture().Width / 2),
                             Position.Y + (TextureSize.Y / 2) + intDef * (inventory[weaponInHand].GetGunTexture().Height / 2));
        }
    }
}
