using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Timers;
using System;
using System.Collections.Generic;

namespace BulletHell
{

    class HealPotion : IItem
    {
        public static Texture2D ItemTexture { get; set; }
        public Vector2 Position { get; set; }
        public void UseItem()
        {
            Hero1.ChengeHP(Room.Rand.Next(5, 10));
        }
    }

    class BigHealPotion : IItem
    {
        public static Texture2D ItemTexture { get; set; }
        public Vector2 Position { get; set; }
        public void UseItem()
        {
            Hero1.ChengeHP(Room.Rand.Next(10, 20));
        }
    }

    class StrangePotion : IItem
    {
        public static Texture2D ItemTexture { get; set; }
        public Vector2 Position { get; set; }
        public void UseItem()
        {
            var chance = Room.Rand.Next(0, 100);
            if (chance <= 70)
                Hero1.ChengeHP(Room.Rand.Next(-10, -1));
            else
                Hero1.ChengeHP(100);
        }
    }

    class RandomPotion : IItem
    {
        public static Texture2D ItemTexture { get; set; }
        public Vector2 Position { get; set; }
        public void UseItem()
        {
            var chance = Room.Rand.Next(0, 100);
            if (chance <= 20)
                Hero1.ChengeHP(Room.Rand.Next(-5, -1));
            else if (chance <= 90)
                Hero1.ChengeHP(Room.Rand.Next(2, 15));
            else
                Hero1.ChengeMaxHP(1);
        }
    }

    class Ammo : IItem
    {
        public static Texture2D ItemTexture { get; set; }
        public Vector2 Position { get; set; }
        public void UseItem()
        {
            Hero1.inventory[Hero1.weaponInHand].AddAmmo(Hero1.inventory[Hero1.weaponInHand].GetPlusAmmo());
        }
    }

    class Heart : IItem
    {
        public static Texture2D ItemTexture { get; set; }
        public Vector2 Position { get; set; }
        public void UseItem()
        {
            Hero1.ChengeMaxHP(1);
        }
    }

    class PistolItem : IItem
    {
        public static Texture2D ItemTexture { get; set; }
        public Vector2 Position { get; set; }
        public void UseItem()
        {
            Hero1.inventory.Add(new Pistol());
            Hero1.inventory.RemoveAt(Hero1.weaponInHand);
        }
    }

    class SimpleShotGunItem : IItem
    {
        public static Texture2D ItemTexture { get; set; }
        public Vector2 Position { get; set; }
        public void UseItem()
        {
            Hero1.inventory.Add(new SimpleShotGun());
            Hero1.inventory.RemoveAt(Hero1.weaponInHand);
        }
    }

    class CrosBowItem : IItem
    {
        public static Texture2D ItemTexture { get; set; }
        public Vector2 Position { get; set; }
        public void UseItem()
        {
            Hero1.inventory.Add(new CrosBow());
            Hero1.inventory.RemoveAt(Hero1.weaponInHand);
        }
    }

    class SubmachineGunItem : IItem
    {
        public static Texture2D ItemTexture { get; set; }
        public Vector2 Position { get; set; }
        public void UseItem()
        {
            Hero1.inventory.Add(new SubmachineGun());
            Hero1.inventory.RemoveAt(Hero1.weaponInHand);
        }
    }

    class MachineGunPistolItem : IItem
    {
        public static Texture2D ItemTexture { get; set; }
        public Vector2 Position { get; set; }
        public void UseItem()
        {
            Hero1.inventory.Add(new MachineGunPistol());
            Hero1.inventory.RemoveAt(Hero1.weaponInHand);
        }
    }

    class DeagleItem : IItem
    {
        public static Texture2D ItemTexture { get; set; }
        public Vector2 Position { get; set; }
        public void UseItem()
        {
            Hero1.inventory.Add(new Deagle());
            Hero1.inventory.RemoveAt(Hero1.weaponInHand);
        }
    }

    class PPSItem : IItem
    {
        public static Texture2D ItemTexture { get; set; }
        public Vector2 Position { get; set; }
        public void UseItem()
        {
            Hero1.inventory.Add(new PPS());
            Hero1.inventory.RemoveAt(Hero1.weaponInHand);
        }
    }

    class P2070Item : IItem
    {
        public static Texture2D ItemTexture { get; set; }
        public Vector2 Position { get; set; }
        public void UseItem()
        {
            Hero1.inventory.Add(new P2070());
            Hero1.inventory.RemoveAt(Hero1.weaponInHand);
        }
    }

    class SniperRifleItem : IItem
    {
        public static Texture2D ItemTexture { get; set; }
        public Vector2 Position { get; set; }
        public void UseItem()
        {
            Hero1.inventory.Add(new SniperRifle());
            Hero1.inventory.RemoveAt(Hero1.weaponInHand);
        }
    }

    class RifleItem : IItem
    {
        public static Texture2D ItemTexture { get; set; }
        public Vector2 Position { get; set; }
        public void UseItem()
        {
            Hero1.inventory.Add(new Rifle());
            Hero1.inventory.RemoveAt(Hero1.weaponInHand);
        }
    }

    class BenelleShotGunItem : IItem
    {
        public static Texture2D ItemTexture { get; set; }
        public Vector2 Position { get; set; }
        public void UseItem()
        {
            Hero1.inventory.Add(new BenelleShotGun());
            Hero1.inventory.RemoveAt(Hero1.weaponInHand);
        }
    }

    class RemingtonShotGunItem : IItem
    {
        public static Texture2D ItemTexture { get; set; }
        public Vector2 Position { get; set; }
        public void UseItem()
        {
            Hero1.inventory.Add(new BenelleShotGun());
            Hero1.inventory.RemoveAt(Hero1.weaponInHand);
        }
    }

    class Pistol : IWeapon
    {
        public static Texture2D GunTexture { get; set; }
        public static Texture2D PrjTexture { get; set; }
        public readonly float ReloadSpeed = 25;
        public readonly float ProjectaleSpeed = 3;
        public readonly float ForceTic = 5;
        public readonly int Damage = 1;
        public readonly int PlusAmmo = 0;
        public readonly int ID = 0;
        public int AvailableAmmo;
        public readonly float Spreading = 0;
        

        public void Shoot()
        {
            Objects.CreateProj(Hero1.GetPos(), new Point(Mouse.GetState().X, Mouse.GetState().Y),
                PrjTexture, ProjectaleSpeed, Level.rooms[Level.GetHeroInRoomValue()].projectiles, false, Damage, Spreading);
        }

        public Texture2D GetPrjTexture()
        {
            return PrjTexture;
        }

        public Texture2D GetGunTexture()
        {
            return GunTexture;
        }

        public int GetPlusAmmo()
        {
            return PlusAmmo;
        }

        public float GetReloadSpeed()
        {
            return ReloadSpeed;
        }
        public float GetProjectaleSpeed()
        {
            return ProjectaleSpeed;
        }
        public float GetForceTic()
        {
            return ForceTic;
        }

        public int GetID()
        {
            return ID;
        }

        public void AddAmmo(int ammo)
        {
            AvailableAmmo += ammo;
        }
    }

    class SimpleShotGun : IWeapon
    {
        public static Texture2D GunTexture { get; set; }
        public static Texture2D PrjTexture { get; set; }
        public readonly float ReloadSpeed = 60;
        public readonly float ProjectaleSpeed = 3;
        public readonly float ForceTic = 7;
        public readonly int Damage = 1;
        public readonly int PlusAmmo = 20;
        public readonly int ID = 1;
        public int AvailableAmmo;
        public readonly float Spreading = 0;



        public void Shoot()
        {
            for(var i = 1; i <= 3; i++)
                Objects.CreateProj(Hero1.GetPos(),
                    new Point(Mouse.GetState().X, Mouse.GetState().Y),
                    PrjTexture, ProjectaleSpeed, Level.rooms[Level.GetHeroInRoomValue()].projectiles, false, Damage, GetSpread(i));
        }

        public Texture2D GetPrjTexture()
        {
            return PrjTexture;
        }

        public Texture2D GetGunTexture()
        {
            return GunTexture;
        }

        public float GetReloadSpeed()
        {
            return ReloadSpeed;
        }

        public int GetPlusAmmo()
        {
            return PlusAmmo;
        }

        public float GetProjectaleSpeed()
        {
            return ProjectaleSpeed;
        }

        public float GetForceTic()
        {
            return ForceTic;
        }

        private float GetSpread(int turn)
        {
            
            switch (turn)
            {
                case 2:
                    return 0.25f;
                case 3:
                    return -0.25f;
            }
            return 0;
        }

        public int GetID()
        {
            return 1;
        }

        public void AddAmmo(int ammo)
        {
            AvailableAmmo += ammo;
        }
    }  

    class CrosBow : IWeapon
    {
        public static Texture2D GunTexture { get; set; }
        public static Texture2D PrjTexture { get; set; }
        public readonly float ReloadSpeed = 80;
        public readonly float ProjectaleSpeed = 7;
        public readonly float ForceTic = 1000;
        public readonly int Damage = 5;
        public readonly int PlusAmmo = 10;
        public readonly int ID = 2;
        public int AvailableAmmo;
        public readonly float Spreading = 0;


        public void Shoot()
        {
            Objects.CreateProj(Hero1.GetPos(), new Point(Mouse.GetState().X, Mouse.GetState().Y),
                PrjTexture, ProjectaleSpeed, Level.rooms[Level.GetHeroInRoomValue()].projectiles, false, Damage, 0);
        }

        public Texture2D GetPrjTexture()
        {
            return PrjTexture;
        }

        public int GetPlusAmmo()
        {
            return PlusAmmo;
        }

        public Texture2D GetGunTexture()
        {
            return GunTexture;
        }

        public float GetReloadSpeed()
        {
            return ReloadSpeed;
        }
        public float GetProjectaleSpeed()
        {
            return ProjectaleSpeed;
        }
        public float GetForceTic()
        {
            return ForceTic;
        }
        public int GetID()
        {
            return 2;
        }
        public void AddAmmo(int ammo)
        {
            AvailableAmmo += ammo;
        }
    }

    class SubmachineGun : IWeapon
    {
        public static Texture2D GunTexture { get; set; }
        public static Texture2D PrjTexture { get; set; }
        public readonly float ReloadSpeed = 10;
        public readonly float ProjectaleSpeed = 3;
        public readonly float ForceTic = 1;
        public readonly float Damage = 0.4f;
        public readonly int PlusAmmo = 50;
        public readonly int ID = 3;
        public int AvailableAmmo;
        public readonly float Spreading = 0.2f;


        public void Shoot()
        {
            var spread = Math.Min((float)Room.Rand.NextDouble() / 3, Spreading) * Math.Min(Room.Rand.Next(-1, 2), 1);
            Objects.CreateProj(Hero1.GetPos(), new Point(Mouse.GetState().X, Mouse.GetState().Y),
                PrjTexture, ProjectaleSpeed, Level.rooms[Level.GetHeroInRoomValue()].projectiles, false, Damage, spread);
        }

        public Texture2D GetPrjTexture()
        {
            return PrjTexture;
        }

        public int GetPlusAmmo()
        {
            return PlusAmmo;
        }

        public Texture2D GetGunTexture()
        {
            return GunTexture;
        }

        public float GetReloadSpeed()
        {
            return ReloadSpeed;
        }
        public float GetProjectaleSpeed()
        {
            return ProjectaleSpeed;
        }
        public float GetForceTic()
        {
            return ForceTic;
        }
        public int GetID()
        {
            return 2;
        }
        public void AddAmmo(int ammo)
        {
            AvailableAmmo += ammo;
        }
    }

    class MachineGunPistol : IWeapon
    {
        public static Texture2D GunTexture { get; set; }
        public static Texture2D PrjTexture { get; set; }
        public readonly float ReloadSpeed = 5;
        public readonly float ProjectaleSpeed = 3;
        public readonly float ForceTic = 1;
        public readonly float Damage = 0.2f;
        public readonly int PlusAmmo = 50;
        public readonly int ID = 4;
        public int AvailableAmmo;
        public readonly float Spreading = 0.4f;


        public void Shoot()
        {
            var spread = Math.Min((float)Room.Rand.NextDouble()/3, Spreading) * Math.Min(Room.Rand.Next(-1, 2), 1);
            Objects.CreateProj(Hero1.GetPos(), new Point(Mouse.GetState().X, Mouse.GetState().Y),
                PrjTexture, ProjectaleSpeed, Level.rooms[Level.GetHeroInRoomValue()].projectiles, false, Damage, spread);
        }

        public Texture2D GetPrjTexture()
        {
            return PrjTexture;
        }

        public int GetPlusAmmo()
        {
            return PlusAmmo;
        }

        public Texture2D GetGunTexture()
        {
            return GunTexture;
        }

        public float GetReloadSpeed()
        {
            return ReloadSpeed;
        }
        public float GetProjectaleSpeed()
        {
            return ProjectaleSpeed;
        }
        public float GetForceTic()
        {
            return ForceTic;
        }
        public int GetID()
        {
            return 2;
        }
        public void AddAmmo(int ammo)
        {
            AvailableAmmo += ammo;
        }
    }

    class Deagle : IWeapon
    {
        public static Texture2D GunTexture { get; set; }
        public static Texture2D PrjTexture { get; set; }
        public readonly float ReloadSpeed = 40;
        public readonly float ProjectaleSpeed = 4;
        public readonly float ForceTic = 7;
        public readonly int Damage = 3;
        public readonly int PlusAmmo = 20;
        public readonly int ID = 5;
        public int AvailableAmmo;
        public readonly float Spreading = 0;


        public void Shoot()
        {
            Objects.CreateProj(Hero1.GetPos(), new Point(Mouse.GetState().X, Mouse.GetState().Y),
                PrjTexture, ProjectaleSpeed, Level.rooms[Level.GetHeroInRoomValue()].projectiles, false, Damage, Spreading);
        }

        public Texture2D GetPrjTexture()
        {
            return PrjTexture;
        }

        public Texture2D GetGunTexture()
        {
            return GunTexture;
        }

        public int GetPlusAmmo()
        {
            return PlusAmmo;
        }

        public float GetReloadSpeed()
        {
            return ReloadSpeed;
        }
        public float GetProjectaleSpeed()
        {
            return ProjectaleSpeed;
        }
        public float GetForceTic()
        {
            return ForceTic;
        }

        public int GetID()
        {
            return ID;
        }

        public void AddAmmo(int ammo)
        {
            AvailableAmmo += ammo;
        }
    }

    class PPS : IWeapon
    {
        public static Texture2D GunTexture { get; set; }
        public static Texture2D PrjTexture { get; set; }
        public readonly float ReloadSpeed = 7;
        public readonly float ProjectaleSpeed = 3;
        public readonly float ForceTic = 1;
        public readonly float Damage = 0.3f;
        public readonly int PlusAmmo = 70;
        public readonly int ID = 6;
        public int AvailableAmmo;
        public readonly float Spreading = 0;


        public void Shoot()
        {
            Objects.CreateProj(Hero1.GetPos(), new Point(Mouse.GetState().X, Mouse.GetState().Y),
                PrjTexture, ProjectaleSpeed, Level.rooms[Level.GetHeroInRoomValue()].projectiles, false, Damage, Spreading);
        }

        public Texture2D GetPrjTexture()
        {
            return PrjTexture;
        }

        public Texture2D GetGunTexture()
        {
            return GunTexture;
        }

        public int GetPlusAmmo()
        {
            return PlusAmmo;
        }

        public float GetReloadSpeed()
        {
            return ReloadSpeed;
        }
        public float GetProjectaleSpeed()
        {
            return ProjectaleSpeed;
        }
        public float GetForceTic()
        {
            return ForceTic;
        }

        public int GetID()
        {
            return ID;
        }

        public void AddAmmo(int ammo)
        {
            AvailableAmmo += ammo;
        }
    }

    class P2070 : IWeapon
    {
        public static Texture2D GunTexture { get; set; }
        public static Texture2D PrjTexture { get; set; }
        public readonly float ReloadSpeed = 20;
        public readonly float ProjectaleSpeed = 3;
        public readonly float ForceTic = 4;
        public readonly int Damage = 1;
        public readonly int PlusAmmo = 40;
        public readonly int ID = 7;
        public int AvailableAmmo;
        public readonly float Spreading = 0;


        public void Shoot()
        {
            Objects.CreateProj(Hero1.GetPos(), new Point(Mouse.GetState().X, Mouse.GetState().Y),
                PrjTexture, ProjectaleSpeed, Level.rooms[Level.GetHeroInRoomValue()].projectiles, false, Damage, Spreading);
        }

        public Texture2D GetPrjTexture()
        {
            return PrjTexture;
        }

        public Texture2D GetGunTexture()
        {
            return GunTexture;
        }

        public int GetPlusAmmo()
        {
            return PlusAmmo;
        }

        public float GetReloadSpeed()
        {
            return ReloadSpeed;
        }
        public float GetProjectaleSpeed()
        {
            return ProjectaleSpeed;
        }
        public float GetForceTic()
        {
            return ForceTic;
        }

        public int GetID()
        {
            return ID;
        }

        public void AddAmmo(int ammo)
        {
            AvailableAmmo += ammo;
        }
    }

    class SniperRifle : IWeapon
    {
        public static Texture2D GunTexture { get; set; }
        public static Texture2D PrjTexture { get; set; }
        public readonly float ReloadSpeed = 70;
        public readonly float ProjectaleSpeed = 10;
        public readonly float ForceTic = 20;
        public readonly int Damage = 7;
        public readonly int PlusAmmo = 10;
        public readonly int ID = 8;
        public int AvailableAmmo;
        public readonly float Spreading = 0;


        public void Shoot()
        {
            Objects.CreateProj(Hero1.GetPos(), new Point(Mouse.GetState().X, Mouse.GetState().Y),
                PrjTexture, ProjectaleSpeed, Level.rooms[Level.GetHeroInRoomValue()].projectiles, false, Damage, 0);
        }

        public Texture2D GetPrjTexture()
        {
            return PrjTexture;
        }

        public int GetPlusAmmo()
        {
            return PlusAmmo;
        }

        public Texture2D GetGunTexture()
        {
            return GunTexture;
        }

        public float GetReloadSpeed()
        {
            return ReloadSpeed;
        }
        public float GetProjectaleSpeed()
        {
            return ProjectaleSpeed;
        }
        public float GetForceTic()
        {
            return ForceTic;
        }
        public int GetID()
        {
            return 2;
        }
        public void AddAmmo(int ammo)
        {
            AvailableAmmo += ammo;
        }
    }

    class Rifle : IWeapon
    {
        public static Texture2D GunTexture { get; set; }
        public static Texture2D PrjTexture { get; set; }
        public readonly float ReloadSpeed = 40;
        public readonly float ProjectaleSpeed = 5;
        public readonly float ForceTic = 10;
        public readonly int Damage = 3;
        public readonly int PlusAmmo = 20;
        public readonly int ID = 9;
        public int AvailableAmmo;
        public readonly float Spreading = 0;


        public void Shoot()
        {
            Objects.CreateProj(Hero1.GetPos(), new Point(Mouse.GetState().X, Mouse.GetState().Y),
                PrjTexture, ProjectaleSpeed, Level.rooms[Level.GetHeroInRoomValue()].projectiles, false, Damage, 0);
        }

        public Texture2D GetPrjTexture()
        {
            return PrjTexture;
        }

        public int GetPlusAmmo()
        {
            return PlusAmmo;
        }

        public Texture2D GetGunTexture()
        {
            return GunTexture;
        }

        public float GetReloadSpeed()
        {
            return ReloadSpeed;
        }
        public float GetProjectaleSpeed()
        {
            return ProjectaleSpeed;
        }
        public float GetForceTic()
        {
            return ForceTic;
        }
        public int GetID()
        {
            return 2;
        }
        public void AddAmmo(int ammo)
        {
            AvailableAmmo += ammo;
        }
    }

    class BenelleShotGun : IWeapon
    {
        public static Texture2D GunTexture { get; set; }
        public static Texture2D PrjTexture { get; set; }
        public readonly float ReloadSpeed = 40;
        public readonly float ProjectaleSpeed = 3;
        public readonly float ForceTic = 10;
        public readonly int Damage = 1;
        public readonly int PlusAmmo = 20;
        public readonly int ID = 10;
        public int AvailableAmmo;
        public readonly float Spreading = 0;



        public void Shoot()
        {
            for (var i = 1; i <= 3; i++)
                Objects.CreateProj(Hero1.GetPos(),
                    new Point(Mouse.GetState().X, Mouse.GetState().Y),
                    PrjTexture, ProjectaleSpeed, Level.rooms[Level.GetHeroInRoomValue()].projectiles, false, Damage, GetSpread(i));
        }

        public Texture2D GetPrjTexture()
        {
            return PrjTexture;
        }

        public Texture2D GetGunTexture()
        {
            return GunTexture;
        }

        public float GetReloadSpeed()
        {
            return ReloadSpeed;
        }

        public int GetPlusAmmo()
        {
            return PlusAmmo;
        }

        public float GetProjectaleSpeed()
        {
            return ProjectaleSpeed;
        }

        public float GetForceTic()
        {
            return ForceTic;
        }

        private float GetSpread(int turn)
        {

            switch (turn)
            {
                case 2:
                    return 0.25f;
                case 3:
                    return -0.25f;
            }
            return 0;
        }

        public int GetID()
        {
            return 1;
        }

        public void AddAmmo(int ammo)
        {
            AvailableAmmo += ammo;
        }
    }

    class RemingtonShotGun : IWeapon
    {
        public static Texture2D GunTexture { get; set; }
        public static Texture2D PrjTexture { get; set; }
        public readonly float ReloadSpeed = 60;
        public readonly float ProjectaleSpeed = 3;
        public readonly float ForceTic = 20;
        public readonly int Damage = 1;
        public readonly int PlusAmmo = 20;
        public readonly int ID = 11;
        public int AvailableAmmo;
        public readonly float Spreading = 0;



        public void Shoot()
        {
            for (var i = 1; i <= 3; i++)
                Objects.CreateProj(Hero1.GetPos(),
                    new Point(Mouse.GetState().X, Mouse.GetState().Y),
                    PrjTexture, ProjectaleSpeed, Level.rooms[Level.GetHeroInRoomValue()].projectiles, false, Damage, GetSpread(i));
        }

        public Texture2D GetPrjTexture()
        {
            return PrjTexture;
        }

        public Texture2D GetGunTexture()
        {
            return GunTexture;
        }

        public float GetReloadSpeed()
        {
            return ReloadSpeed;
        }

        public int GetPlusAmmo()
        {
            return PlusAmmo;
        }

        public float GetProjectaleSpeed()
        {
            return ProjectaleSpeed;
        }

        public float GetForceTic()
        {
            return ForceTic;
        }

        private float GetSpread(int turn)
        {

            switch (turn)
            {
                case 2:
                    return 0.15f;
                case 3:
                    return -0.15f;
            }
            return 0;
        }

        public int GetID()
        {
            return 1;
        }

        public void AddAmmo(int ammo)
        {
            AvailableAmmo += ammo;
        }
    }
}
