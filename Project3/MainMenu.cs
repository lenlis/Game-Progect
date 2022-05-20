using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Timers;
using System;
using System.Collections.Generic;

namespace BulletHell
{
    class MainMenu
    {
        public static Texture2D startB;
        public static Texture2D exitB;
        public static Texture2D menuBack;
        private static float startSk = 1.1f;
        private static float exitSk = 1f;

        public static void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(menuBack, new Vector2(0, 0), Color.White);
            spriteBatch.Draw(startB, new Vector2(50, 50),null, Color.White, 0, Vector2.Zero, startSk, SpriteEffects.None, 0);
            spriteBatch.Draw(exitB, new Vector2(50, 250), null, Color.White, 0, Vector2.Zero, exitSk, SpriteEffects.None, 0);
        }

        public static bool IsStartClick(MouseState mouseState)
        {

            if ((mouseState.X >= 50 && mouseState.Y >= 50) && (mouseState.X <= 420 && mouseState.Y <= 193))
            {
                if (startSk <= 1.2f)
                    startSk += 0.02f;
                return true;
            }
            else
            {
                startSk = 1.1f;
                return false;
            }
        }
        public static bool IsExitClick(MouseState mouseState)
        {
            if ((mouseState.X >= 50 && mouseState.Y >= 250) && (mouseState.X <= 420 && mouseState.Y <= 380))
            {
                if (exitSk <= 1.1f)
                    exitSk += 0.02f;
                return true;
            }
            else
            {
                exitSk = 1f;
                return false;
            }
        }
    }
}
