using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Enums;

namespace WpfApp1.Click
{
    internal static class ClickMethods
    {
        [DllImport("user32.dll")]

        static extern void mouse_event(int dwFlags, int dx, int dy, int dwdata, int dwextrainfo);

        public static void leftclick(int coordX, int coordY)
        {
            mouse_event(((int)(MouseEventFlags.LEFTDOWN)), coordX, coordY, 0, 0);
            mouse_event((int)(MouseEventFlags.LEFTUP), coordX, coordY, 0, 0);
        }
        public static void rightclick(int coordX, int coordY)
        {
            mouse_event(((int)(MouseEventFlags.RIGHTDOWN)), coordX, coordY, 0, 0);
            mouse_event((int)(MouseEventFlags.RIGHTUP), coordX, coordY, 0, 0);
        }
    }
}
