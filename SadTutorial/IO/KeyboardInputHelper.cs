using SadConsole;
using SadConsole.Input;
using SadRogue.Primitives;
using System.Collections.Generic;

namespace SadTutorial.IO
{
    public static class KeyboardInputHelper
    {
        public static bool KeyPressed(Keys key)
        {
            return GameHost.Instance.Keyboard.IsKeyPressed(key);
        }

        static HashSet<Keys> TriggeredHotkeys = new();
        static HashSet<Keys> SecondaryList = new();
        public static bool HotkeyDown(Keys key)
        {
            if (!TriggeredHotkeys.Contains(key) && GameHost.Instance.Keyboard.IsKeyPressed(key))
            {
                TriggeredHotkeys.Add(key);
                return true;
            }

            return false;
        }

        public static void ClearKeys()
        {
            SecondaryList.Clear();
            foreach (Keys key in TriggeredHotkeys)
            {
                if (GameHost.Instance.Keyboard.IsKeyDown(key))
                {
                    SecondaryList.Add(key);
                }
            }
            TriggeredHotkeys.Clear();

            foreach (Keys key in SecondaryList)
            {
                TriggeredHotkeys.Add(key);
            }
        }

        public static bool IsShiftDown()
        {
            if (GameHost.Instance.Keyboard.IsKeyDown(Keys.LeftShift) || GameHost.Instance.Keyboard.IsKeyDown(Keys.RightShift))
                return true;
            return false;
        }
        public static bool IsControlDown()
        {
            if (GameHost.Instance.Keyboard.IsKeyDown(Keys.LeftControl) || GameHost.Instance.Keyboard.IsKeyDown(Keys.RightControl))
                return true;
            return false;
        }
    }

}
