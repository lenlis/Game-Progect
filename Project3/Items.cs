using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Timers;
using System;
using System.Collections.Generic;

namespace BulletHell
{
    class Loot
    {
        public static IItem[] loot = new IItem[18] 
        { 
            new HealPotion(),
            new BigHealPotion(),
            new StrangePotion(),
            new RandomPotion(), 
            new Ammo(), 
            new Heart(), 
            new PistolItem(),
            new SimpleShotGunItem(), 
            new CrosBowItem(),
            new SubMachineGunItem(),
            new MachineGunPistolItem(),
            new DeagleItem(), 
            new PPShItem(),
            new P2070Item(),
            new SniperRifleItem(),
            new RifleItem(),
            new BenelleShotGunItem(),
            new RemingtonShotGunItem() 
        };

        private static void ReInitLootArray()
        {
            loot = new IItem[18]
        {
            new HealPotion(),
            new BigHealPotion(),
            new StrangePotion(),
            new RandomPotion(),
            new Ammo(),
            new Heart(),
            new PistolItem(),
            new SimpleShotGunItem(),
            new CrosBowItem(),
            new SubMachineGunItem(),
            new MachineGunPistolItem(),
            new DeagleItem(),
            new PPShItem(),
            new P2070Item(),
            new SniperRifleItem(),
            new RifleItem(),
            new BenelleShotGunItem(),
            new RemingtonShotGunItem()
        };
        }

        public static void AddLoot(int lootNum, Vector2 Pos, List<IItem> items)
        {
            ReInitLootArray();
            items.Add(loot[lootNum]);
            items[^1].Position = Pos;
        }
        public static IItem AddLootInCase(int lootNum)
        {
            return loot[lootNum];
        }
    }


    class HealPotion : IItem
    {
        public static Texture2D ItemTexture { get; set; }
        public Vector2 Position { get; set; }
        public void UseItem()
        {
            Hero1.ChengeHP(Room.Rand.Next(5, 10));
        }

        public Texture2D GetTexture()
        {
            return ItemTexture;
        }

        public Vector2 GetPos()
        {
            return Position;
        }

        public Rectangle GetCollusion()
        {
            return new Rectangle((int)Position.X,
            (int)Position.Y, ItemTexture.Width, ItemTexture.Height);
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
        public Texture2D GetTexture()
        {
            return ItemTexture;
        }
        public Vector2 GetPos()
        {
            return Position;
        }
        public Rectangle GetCollusion()
        {
            return new Rectangle((int)Position.X,
            (int)Position.Y, ItemTexture.Width, ItemTexture.Height);
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
        public Vector2 GetPos()
        {
            return Position;
        }
        public Texture2D GetTexture()
        {
            return ItemTexture;
        }
        public Rectangle GetCollusion()
        {
            return new Rectangle((int)Position.X,
            (int)Position.Y, ItemTexture.Width, ItemTexture.Height);
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
        public Vector2 GetPos()
        {
            return Position;
        }
        public Texture2D GetTexture()
        {
            return ItemTexture;
        }
        public Rectangle GetCollusion()
        {
            return new Rectangle((int)Position.X,
            (int)Position.Y, ItemTexture.Width, ItemTexture.Height);
        }
    }

    class Ammo : IItem
    {
        public static Texture2D ItemTexture { get; set; }
        public Vector2 Position { get; set; }
        public void UseItem()
        {
            foreach(var weap in Hero1.inventory)
                weap.AddAmmo(weap.GetPlusAmmo());
        }
        public Vector2 GetPos()
        {
            return Position;
        }
        public Texture2D GetTexture()
        {
            return ItemTexture;
        }
        public Rectangle GetCollusion()
        {
            return new Rectangle((int)Position.X,
            (int)Position.Y, ItemTexture.Width, ItemTexture.Height);
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
        public Vector2 GetPos()
        {
            return Position;
        }
        public Texture2D GetTexture()
        {
            return ItemTexture;
        }
        public Rectangle GetCollusion()
        {
            return new Rectangle((int)Position.X,
            (int)Position.Y, ItemTexture.Width, ItemTexture.Height);
        }
    }

    class PistolItem : IItem
    {
        public static Texture2D ItemTexture { get; set; }
        public Vector2 Position { get; set; }
        public void UseItem()
        {
            if (Hero1.inventory.Count == 2)
            {
                Hero1.inventory[Hero1.weaponInHand].ThrowWeapon();
                Hero1.inventory.RemoveAt(Hero1.weaponInHand);
            }
            Hero1.inventory.Add(new Pistol());    
        }
        public Vector2 GetPos()
        {
            return Position;
        }
        public Texture2D GetTexture()
        {
            return ItemTexture;
        }
        public Rectangle GetCollusion()
        {
            return new Rectangle((int)Position.X,
            (int)Position.Y, ItemTexture.Width, ItemTexture.Height);
        }
    }

    class SimpleShotGunItem : IItem
    {
        public static Texture2D ItemTexture { get; set; }
        public Vector2 Position { get; set; }
        public void UseItem()
        {
            if (Hero1.inventory.Count == 2)
            {
                Hero1.inventory[Hero1.weaponInHand].ThrowWeapon();
                Hero1.inventory.RemoveAt(Hero1.weaponInHand);
            }
            Hero1.inventory.Add(new SimpleShotGun());
        }
        public Vector2 GetPos()
        {
            return Position;
        }
        public Texture2D GetTexture()
        {
            return ItemTexture;
        }
        public Rectangle GetCollusion()
        {
            return new Rectangle((int)Position.X,
            (int)Position.Y, ItemTexture.Width, ItemTexture.Height);
        }
    }

    class CrosBowItem : IItem
    {
        public static Texture2D ItemTexture { get; set; }
        public Vector2 Position { get; set; }
        public void UseItem()
        {
            if (Hero1.inventory.Count == 2)
            {
                Hero1.inventory[Hero1.weaponInHand].ThrowWeapon();
                Hero1.inventory.RemoveAt(Hero1.weaponInHand);
            }
            Hero1.inventory.Add(new CrosBow());
        }
        public Vector2 GetPos()
        {
            return Position;
        }
        public Texture2D GetTexture()
        {
            return ItemTexture;
        }
        public Rectangle GetCollusion()
        {
            return new Rectangle((int)Position.X,
            (int)Position.Y, ItemTexture.Width, ItemTexture.Height);
        }
    }

    class SubMachineGunItem : IItem
    {
        public static Texture2D ItemTexture { get; set; }
        public Vector2 Position { get; set; }
        public void UseItem()
        {
            if (Hero1.inventory.Count == 2)
            {
                Hero1.inventory[Hero1.weaponInHand].ThrowWeapon();
                Hero1.inventory.RemoveAt(Hero1.weaponInHand);
            }
            Hero1.inventory.Add(new SubMachineGun());
        }
        public Vector2 GetPos()
        {
            return Position;
        }
        public Texture2D GetTexture()
        {
            return ItemTexture;
        }
        public Rectangle GetCollusion()
        {
            return new Rectangle((int)Position.X,
            (int)Position.Y, ItemTexture.Width, ItemTexture.Height);
        }
    }

    class MachineGunPistolItem : IItem
    {
        public static Texture2D ItemTexture { get; set; }
        public Vector2 Position { get; set; }
        public void UseItem()
        {
            if (Hero1.inventory.Count == 2)
            {
                Hero1.inventory[Hero1.weaponInHand].ThrowWeapon();
                Hero1.inventory.RemoveAt(Hero1.weaponInHand);
            }
            Hero1.inventory.Add(new MachineGunPistol());
        }
        public Vector2 GetPos()
        {
            return Position;
        }
        public Texture2D GetTexture()
        {
            return ItemTexture;
        }
        public Rectangle GetCollusion()
        {
            return new Rectangle((int)Position.X,
            (int)Position.Y, ItemTexture.Width, ItemTexture.Height);
        }
    }

    class DeagleItem : IItem
    {
        public static Texture2D ItemTexture { get; set; }
        public Vector2 Position { get; set; }
        public void UseItem()
        {
            if (Hero1.inventory.Count == 2)
            {
                Hero1.inventory[Hero1.weaponInHand].ThrowWeapon();
                Hero1.inventory.RemoveAt(Hero1.weaponInHand);
            }
            Hero1.inventory.Add(new Deagle());
        }
        public Vector2 GetPos()
        {
            return Position;
        }
        public Texture2D GetTexture()
        {
            return ItemTexture;
        }
        public Rectangle GetCollusion()
        {
            return new Rectangle((int)Position.X,
            (int)Position.Y, ItemTexture.Width, ItemTexture.Height);
        }
    }

    class PPShItem : IItem
    {
        public static Texture2D ItemTexture { get; set; }
        public Vector2 Position { get; set; }
        public void UseItem()
        {
            if (Hero1.inventory.Count == 2)
            {
                Hero1.inventory[Hero1.weaponInHand].ThrowWeapon();
                Hero1.inventory.RemoveAt(Hero1.weaponInHand);
            }
            Hero1.inventory.Add(new PPSh());
        }
        public Vector2 GetPos()
        {
            return Position;
        }
        public Texture2D GetTexture()
        {
            return ItemTexture;
        }
        public Rectangle GetCollusion()
        {
            return new Rectangle((int)Position.X,
            (int)Position.Y, ItemTexture.Width, ItemTexture.Height);
        }
    }

    class P2070Item : IItem
    {
        public static Texture2D ItemTexture { get; set; }
        public Vector2 Position { get; set; }
        public void UseItem()
        {
            if (Hero1.inventory.Count == 2)
            {
                Hero1.inventory[Hero1.weaponInHand].ThrowWeapon();
                Hero1.inventory.RemoveAt(Hero1.weaponInHand);
            }
            Hero1.inventory.Add(new P2070());
        }
        public Vector2 GetPos()
        {
            return Position;
        }
        public Texture2D GetTexture()
        {
            return ItemTexture;
        }
        public Rectangle GetCollusion()
        {
            return new Rectangle((int)Position.X,
            (int)Position.Y, ItemTexture.Width, ItemTexture.Height);
        }
    }

    class SniperRifleItem : IItem
    {
        public static Texture2D ItemTexture { get; set; }
        public Vector2 Position { get; set; }
        public void UseItem()
        {
            if (Hero1.inventory.Count == 2)
            {
                Hero1.inventory[Hero1.weaponInHand].ThrowWeapon();
                Hero1.inventory.RemoveAt(Hero1.weaponInHand);
            }
            Hero1.inventory.Add(new SniperRifle());
        }
        public Vector2 GetPos()
        {
            return Position;
        }
        public Texture2D GetTexture()
        {
            return ItemTexture;
        }
        public Rectangle GetCollusion()
        {
            return new Rectangle((int)Position.X,
            (int)Position.Y, ItemTexture.Width, ItemTexture.Height);
        }
    }

    class RifleItem : IItem
    {
        public static Texture2D ItemTexture { get; set; }
        public Vector2 Position { get; set; }
        public void UseItem()
        {
            if (Hero1.inventory.Count == 2)
            {
                Hero1.inventory[Hero1.weaponInHand].ThrowWeapon();
                Hero1.inventory.RemoveAt(Hero1.weaponInHand);
            }
            Hero1.inventory.Add(new Rifle());
        }
        public Vector2 GetPos()
        {
            return Position;
        }
        public Texture2D GetTexture()
        {
            return ItemTexture;
        }
        public Rectangle GetCollusion()
        {
            return new Rectangle((int)Position.X,
            (int)Position.Y, ItemTexture.Width, ItemTexture.Height);
        }
    }

    class BenelleShotGunItem : IItem
    {
        public static Texture2D ItemTexture { get; set; }
        public Vector2 Position { get; set; }
        public void UseItem()
        {
            if (Hero1.inventory.Count == 2)
            {
                Hero1.inventory[Hero1.weaponInHand].ThrowWeapon();
                Hero1.inventory.RemoveAt(Hero1.weaponInHand);
            }
            Hero1.inventory.Add(new BenelleShotGun());
        }
        public Vector2 GetPos()
        {
            return Position;
        }
        public Texture2D GetTexture()
        {
            return ItemTexture;
        }
        public Rectangle GetCollusion()
        {
            return new Rectangle((int)Position.X,
            (int)Position.Y, ItemTexture.Width, ItemTexture.Height);
        }
    }

    class RemingtonShotGunItem : IItem
    {
        public static Texture2D ItemTexture { get; set; }
        public Vector2 Position { get; set; }
        public void UseItem()
        {
            if (Hero1.inventory.Count == 2)
            {
                Hero1.inventory[Hero1.weaponInHand].ThrowWeapon();
                Hero1.inventory.RemoveAt(Hero1.weaponInHand);
            }
            Hero1.inventory.Add(new BenelleShotGun());
        }
        public Vector2 GetPos()
        {
            return Position;
        }
        public Texture2D GetTexture()
        {
            return ItemTexture;
        }
        public Rectangle GetCollusion()
        {
            return new Rectangle((int)Position.X,
            (int)Position.Y, ItemTexture.Width, ItemTexture.Height);
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

        public int GetAmmo()
        {
            return AvailableAmmo;
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

        public void ThrowWeapon()
        {
            Level.rooms[Level.GetHeroInRoomValue()].items.Add(new PistolItem() {Position = Hero1.GetPos()});
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
        public int AvailableAmmo = 20;
        public readonly float Spreading = 0;



        public void Shoot()
        {
            if (AvailableAmmo > 0)
            {
                AvailableAmmo--;
                for (var i = 1; i <= 3; i++)
                    Objects.CreateProj(Hero1.GetPos(),
                        new Point(Mouse.GetState().X, Mouse.GetState().Y),
                        PrjTexture, ProjectaleSpeed, Level.rooms[Level.GetHeroInRoomValue()].projectiles, false, Damage, GetSpread(i));
            }
        }

        public int GetAmmo()
        {
            return AvailableAmmo;
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

            return turn switch
            {
                2 => 0.25f,
                3 => -0.25f,
                _ => 0,
            };
        }

        public int GetID()
        {
            return 1;
        }

        public void AddAmmo(int ammo)
        {
            AvailableAmmo += ammo;
        }

        public void ThrowWeapon()
        {
            Level.rooms[Level.GetHeroInRoomValue()].items.Add(new SimpleShotGunItem() { Position = Hero1.GetPos() });
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
        public int AvailableAmmo = 10;
        public readonly float Spreading = 0;


        public void Shoot()
        {
            if (AvailableAmmo > 0)
            {
                AvailableAmmo--;
                Objects.CreateProj(Hero1.GetPos(), new Point(Mouse.GetState().X, Mouse.GetState().Y),
                PrjTexture, ProjectaleSpeed, Level.rooms[Level.GetHeroInRoomValue()].projectiles, false, Damage, 0);
            }
        }

        public int GetAmmo()
        {
            return AvailableAmmo;
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

        public void ThrowWeapon()
        {
            Level.rooms[Level.GetHeroInRoomValue()].items.Add(new CrosBowItem() { Position = Hero1.GetPos() });
        }
    }

    class SubMachineGun : IWeapon
    {
        public static Texture2D GunTexture { get; set; }
        public static Texture2D PrjTexture { get; set; }
        public readonly float ReloadSpeed = 10;
        public readonly float ProjectaleSpeed = 3;
        public readonly float ForceTic = 1;
        public readonly float Damage = 0.4f;
        public readonly int PlusAmmo = 50;
        public readonly int ID = 3;
        public int AvailableAmmo = 50;
        public readonly float Spreading = 0.2f;


        public void Shoot()
        {
            if (AvailableAmmo > 0)
            {
                AvailableAmmo--;
                var spread = Math.Min((float)Room.Rand.NextDouble() / 3, Spreading) * Math.Min(Room.Rand.Next(-1, 2), 1);
                Objects.CreateProj(Hero1.GetPos(), new Point(Mouse.GetState().X, Mouse.GetState().Y),
                    PrjTexture, ProjectaleSpeed, Level.rooms[Level.GetHeroInRoomValue()].projectiles, false, Damage, spread);
            }
        }

        public Texture2D GetPrjTexture()
        {
            return PrjTexture;
        }

        public int GetPlusAmmo()
        {
            return PlusAmmo;
        }

        public int GetAmmo()
        {
            return AvailableAmmo;
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

        public void ThrowWeapon()
        {
            Level.rooms[Level.GetHeroInRoomValue()].items.Add(new SubMachineGunItem() { Position = Hero1.GetPos() });
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
        public readonly int PlusAmmo = 150;
        public readonly int ID = 4;
        public int AvailableAmmo = 150;
        public readonly float Spreading = 0.4f;


        public void Shoot()
        {
            if (AvailableAmmo > 0)
            {
                AvailableAmmo--;
                var spread = Math.Min((float)Room.Rand.NextDouble() / 3, Spreading) * Math.Min(Room.Rand.Next(-1, 2), 1);
                Objects.CreateProj(Hero1.GetPos(), new Point(Mouse.GetState().X, Mouse.GetState().Y),
                    PrjTexture, ProjectaleSpeed, Level.rooms[Level.GetHeroInRoomValue()].projectiles, false, Damage, spread);
            }
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

        public int GetAmmo()
        {
            return AvailableAmmo;
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

        public void ThrowWeapon()
        {
            Level.rooms[Level.GetHeroInRoomValue()].items.Add(new MachineGunPistolItem() { Position = Hero1.GetPos() });
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
        public int AvailableAmmo = 20;
        public readonly float Spreading = 0;


        public void Shoot()
        {
            if (AvailableAmmo > 0)
            {
                AvailableAmmo--;
                Objects.CreateProj(Hero1.GetPos(), new Point(Mouse.GetState().X, Mouse.GetState().Y),
                    PrjTexture, ProjectaleSpeed, Level.rooms[Level.GetHeroInRoomValue()].projectiles, false, Damage, Spreading);
            }
            
        }

        public int GetAmmo()
        {
            return AvailableAmmo;
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

        public void ThrowWeapon()
        {
            Level.rooms[Level.GetHeroInRoomValue()].items.Add(new DeagleItem() { Position = Hero1.GetPos() });
        }
    }

    class PPSh : IWeapon
    {
        public static Texture2D GunTexture { get; set; }
        public static Texture2D PrjTexture { get; set; }
        public readonly float ReloadSpeed = 7;
        public readonly float ProjectaleSpeed = 3;
        public readonly float ForceTic = 1;
        public readonly float Damage = 0.3f;
        public readonly int PlusAmmo = 170;
        public readonly int ID = 6;
        public int AvailableAmmo = 170;
        public readonly float Spreading = 0.2f;


        public void Shoot()
        {
            if (AvailableAmmo > 0)
            {
                AvailableAmmo--;
                var spread = Math.Min((float)Room.Rand.NextDouble() / 3, Spreading) * Math.Min(Room.Rand.Next(-1, 2), 1);
                Objects.CreateProj(Hero1.GetPos(), new Point(Mouse.GetState().X, Mouse.GetState().Y),
                PrjTexture, ProjectaleSpeed, Level.rooms[Level.GetHeroInRoomValue()].projectiles, false, Damage, spread);
            }
        }

        public int GetAmmo()
        {
            return AvailableAmmo;
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

        public void ThrowWeapon()
        {
            Level.rooms[Level.GetHeroInRoomValue()].items.Add(new PPShItem() { Position = Hero1.GetPos() });
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
        public int AvailableAmmo = 40;
        public readonly float Spreading = 0;


        public void Shoot()
        {
            if (AvailableAmmo > 0)
            {
                AvailableAmmo--;
                Objects.CreateProj(Hero1.GetPos(), new Point(Mouse.GetState().X, Mouse.GetState().Y),
                PrjTexture, ProjectaleSpeed, Level.rooms[Level.GetHeroInRoomValue()].projectiles, false, Damage, Spreading);
            }
        }

        public int GetAmmo()
        {
            return AvailableAmmo;
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

        public void ThrowWeapon()
        {
            Level.rooms[Level.GetHeroInRoomValue()].items.Add(new P2070Item() { Position = Hero1.GetPos() });
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
        public int AvailableAmmo = 10;
        public readonly float Spreading = 0;


        public void Shoot()
        {
            if (AvailableAmmo > 0)
            {
                AvailableAmmo--;
                Objects.CreateProj(Hero1.GetPos(), new Point(Mouse.GetState().X, Mouse.GetState().Y),
                PrjTexture, ProjectaleSpeed, Level.rooms[Level.GetHeroInRoomValue()].projectiles, false, Damage, 0);
            }
        }

        public Texture2D GetPrjTexture()
        {
            return PrjTexture;
        }

        public int GetAmmo()
        {
            return AvailableAmmo;
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

        public void ThrowWeapon()
        {
            Level.rooms[Level.GetHeroInRoomValue()].items.Add(new SniperRifleItem() { Position = Hero1.GetPos() });
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
        public int AvailableAmmo = 20;
        public readonly float Spreading = 0;


        public void Shoot()
        {
            if (AvailableAmmo > 0)
            {
                AvailableAmmo--;
                Objects.CreateProj(Hero1.GetPos(), new Point(Mouse.GetState().X, Mouse.GetState().Y),
                PrjTexture, ProjectaleSpeed, Level.rooms[Level.GetHeroInRoomValue()].projectiles, false, Damage, 0);
            }
        }

        public int GetAmmo()
        {
            return AvailableAmmo;
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

        public void ThrowWeapon()
        {
            Level.rooms[Level.GetHeroInRoomValue()].items.Add(new RifleItem() { Position = Hero1.GetPos() });
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
        public int AvailableAmmo = 20;
        public readonly float Spreading = 0;



        public void Shoot()
        {
            if (AvailableAmmo > 0)
            {
                AvailableAmmo--;
                for (var i = 1; i <= 3; i++)
                    Objects.CreateProj(Hero1.GetPos(),
                        new Point(Mouse.GetState().X, Mouse.GetState().Y),
                        PrjTexture, ProjectaleSpeed, Level.rooms[Level.GetHeroInRoomValue()].projectiles, false, Damage, GetSpread(i));
            }
        }

        public int GetAmmo()
        {
            return AvailableAmmo;
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

            return turn switch
            {
                2 => 0.25f,
                3 => -0.25f,
                _ => 0,
            };
        }

        public int GetID()
        {
            return 1;
        }

        public void AddAmmo(int ammo)
        {
            AvailableAmmo += ammo;
        }

        public void ThrowWeapon()
        {
            Level.rooms[Level.GetHeroInRoomValue()].items.Add(new BenelleShotGunItem() { Position = Hero1.GetPos() });
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
        public int AvailableAmmo = 20;
        public readonly float Spreading = 0;



        public void Shoot()
        {
            if (AvailableAmmo > 0)
            {
                AvailableAmmo--;
                for (var i = 1; i <= 3; i++)
                    Objects.CreateProj(Hero1.GetPos(),
                        new Point(Mouse.GetState().X, Mouse.GetState().Y),
                        PrjTexture, ProjectaleSpeed, Level.rooms[Level.GetHeroInRoomValue()].projectiles, false, Damage, GetSpread(i));
            }
        }

        public int GetAmmo()
        {
            return AvailableAmmo;
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

            return turn switch
            {
                2 => 0.15f,
                3 => -0.15f,
                _ => 0,
            };
        }

        public int GetID()
        {
            return 1;
        }

        public void AddAmmo(int ammo)
        {
            AvailableAmmo += ammo;
        }

        public void ThrowWeapon()
        {
            Level.rooms[Level.GetHeroInRoomValue()].items.Add(new RemingtonShotGunItem() { Position = Hero1.GetPos() });
        }
    }
}
