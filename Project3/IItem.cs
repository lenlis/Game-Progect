using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Timers;
using System;
using System.Collections.Generic;

namespace BulletHell
{
    public interface IItem
    {
        public static Texture2D ItemTexture { get; set; }
        public Vector2 Position { get; set; }
        public void UseItem();
        public Texture2D GetTexture();
        public Vector2 GetPos();
        public Rectangle GetCollusion();
    }
}
