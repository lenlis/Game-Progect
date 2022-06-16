using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Timers;
using System;
using System.Collections.Generic;

namespace BulletHell
{
    public interface IWeapon
    {
        public static Texture2D GunTexture { get; set; }
        public static Texture2D PrjTexture { get; set; }

        public void Shoot();

        public void AddAmmo(int ammo);

        public int GetAmmo();

        public Texture2D GetPrjTexture();

        public int GetPlusAmmo();

        public Texture2D GetGunTexture();

        public float GetReloadSpeed();

        public float GetProjectaleSpeed();

        public float GetForceTic();

        public int GetID();

        public void ThrowWeapon();
    }
}
