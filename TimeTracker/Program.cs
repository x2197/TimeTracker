using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracker
{
    class Program
    {
        const int Hide = 0;
        const int Show = 1;

        [DllImport("Kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("User32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int cmdShow);

        static void Main(string[] args)
        {
            IntPtr hWndConsole = GetConsoleWindow();
            if (hWndConsole != IntPtr.Zero)
            {
                ShowWindow(hWndConsole, Hide);
            }

            var t = new TimeTracker();
            while (true)
            {
                t.updateDbWithProcesses();
                System.Threading.Thread.Sleep(5 * 1000);

            }
        }
    }
}
