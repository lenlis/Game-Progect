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

        static double x = 10;
        static double y = 10;

        public static Vector2 GetPos()
        {
            return new Vector2((float)x, (float)y);
        }

        public static void UpdatePos(Vector2 Pos)
        {
            x = Pos.X;
            y = Pos.Y;
        }
    }
}
