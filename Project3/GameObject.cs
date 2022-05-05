using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Timers;
using System;
using System.Collections.Generic;

namespace BulletHell
{
    public class GameObject
    {
        Vector2 roomPos;
        static public SpriteBatch spriteBatch = Objects.spriteBatch;

        bool isSecCondition;
        

        public Vector2 GetPos()
        {
            return roomPos;
        }

        

        public void ChengeCondition(bool cond)
        {
            isSecCondition = cond;
        }

        public bool GetCondition()
        {
            return isSecCondition;
        }

        

        public void UpdatePos(Vector2 Pos)
        {
            roomPos.X = Pos.X;
            roomPos.Y = Pos.Y;
        }


    }
}
