using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Timers;
using System;
using System.Collections.Generic;

namespace BulletHell
{
    interface IItem
    {
        public static Texture2D ItemTexture { get; set; }

        public void UseItem()
        {

        }
    }
}
