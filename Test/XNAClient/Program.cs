using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Risk.Client.Drawing
{
    static class Program
    {

        static FormMain frm;

        [STAThread]
        static void Main()
        {

            using (frm = new FormMain())
            {
                Application.EnableVisualStyles();

                frm.Initialize();

                frm.Show();

                // Hook the application's idle event
                System.Windows.Forms.Application.Idle += new EventHandler(OnApplicationIdle);
                System.Windows.Forms.Application.Run(frm);
            }
        }

        static private void OnApplicationIdle(object sender, EventArgs e)
        {
            while (AppStillIdle)
            {
                // Render a frame during idle time (no messages are waiting)
                frm.UpdateComponents();
                frm.DrawComponents();
            }
        }

        static private bool AppStillIdle
        {
            get
            {
                NativeMethods.Message msg;
                return !NativeMethods.PeekMessage(out msg, IntPtr.Zero, 0, 0, 0);
            }
        }
    }

    class NativeMethods
    {
        // declarations for those two native methods members

        [StructLayout(LayoutKind.Sequential)]
        public struct Message
        {
            public IntPtr hWnd;
            public uint msg;
            public IntPtr wParam;
            public IntPtr lPaarm;
            public uint time;
            public System.Drawing.Point p;
        }

        // We won't use this maliciously
        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool PeekMessage(
            out Message msg, 
            IntPtr hWnd, 
            uint messageFilterMin, 
            uint messageFilterMax, 
            uint flags
        );
    }
}