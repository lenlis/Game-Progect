using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Timers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BulletHell
{
    internal static class MoveSet
    {

        static public void MoveLeft()
        {
            var heroPos = Hero1.GetPos();
            if (heroPos.X > Level.roomPos.X)
                heroPos.X -= 230 * (30 / 1000f);
            Hero1.UpdatePos(heroPos);
        }

        static public void MoveRight()
        {
            var heroPos = Hero1.GetPos();
            if (heroPos.X < Objects.windowWidth - Hero1.TextureSize.X - Level.roomPos.X)
                heroPos.X += 230 * (30 / 1000f);
            Hero1.UpdatePos(heroPos);
        }

        static public void MoveUp()
        {
            var heroPos = Hero1.GetPos();
            if (heroPos.Y > Level.roomPos.Y - Hero1.TextureSize.Y + 20)
                heroPos.Y -= 230 * (30 / 1000f);
            Hero1.UpdatePos(heroPos);
        }

        static public void MoveDown()
        {
            var heroPos = Hero1.GetPos();
            if (heroPos.Y < Objects.windowHeight - Hero1.TextureSize.Y - Level.roomPos.Y)
                heroPos.Y += 230 * (30 / 1000f);
            Hero1.UpdatePos(heroPos);
        }

        static public void Soot() 
        {
            Hero1.inventory[Hero1.weaponInHand].Shoot();
            Hero1.Reload();
        }

        static public void ChangeWeapon(long shift)
        {
            if (shift < 0)
            {
                if (Hero1.weaponInHand != 0)
                    Hero1.weaponInHand--;
                else
                    Hero1.weaponInHand = Hero1.inventory.Count() - 1;
            }
            else
            {
                if (Hero1.weaponInHand != Hero1.inventory.Count() - 1)
                    Hero1.weaponInHand++;
                else
                    Hero1.weaponInHand = 0;
            }
        }
    }
}
