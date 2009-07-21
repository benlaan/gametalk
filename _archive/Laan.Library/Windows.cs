using System;
using System.IO;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Diagnostics;

using Microsoft.Win32;
using System.DirectoryServices;

namespace Laan.Utilities.Windows
{
    public class Win32Libs
	{
        [DllImport("kernel32.dll")]
        public static extern bool AttachConsole(int dwProcessId);
        public const int ATTACH_PARENT_PROCESS = -1;
    }
    
    public class IconInfo
    {

        public IconInfo(string description)
        {
            Description = description;
        }

        public Icon Icon;
        public string Description;
    }

    public delegate void LaunchEventHandler(object sender, LaunchArgs args);

    public class LaunchArgs: EventArgs
    {

        public LaunchArgs(string application, string parameters)
        {
            _application = application;
            _parameters = parameters;
        }

        private string _application;
        private string _parameters;
        private bool   _waitForExit = false;

        internal void DoProcessCompleted()
        {
            if (OnProcessCompleted != null)
                OnProcessCompleted(this, this);
        }

        public string Application
        {
            get { return _application; }
            set { _application = value; }
        }

        public event LaunchEventHandler OnProcessCompleted;

        public string Parameters
        {
            get { return _parameters; }
            set { _parameters = value; }
        }

        public bool WaitForExit
        {
            get { return _waitForExit; }
            set { _waitForExit = value; }
        }
    }

    public class User
    {
        internal static string _name = "";

        static public string Name
        {
            get {

                if (_name == "")
                {
                    string domainName = System.Security.Principal.WindowsIdentity.GetCurrent().Name.Replace(@"\", @"/");
                    DirectoryEntry entry = new DirectoryEntry("WinNT://" + domainName);

                    if (entry != null)
                        _name = (string)entry.Properties["FullName"].Value;
                    else
                        _name = domainName;
                }
                return _name;
            }
        }
    }

    public class Reg
    {
        public static RegistryKey OpenRoot(string key)
        {
            switch (key.Remove(key.IndexOf("\\")))
            {
                case "HKEY_CLASSES_ROOT":
                    return Registry.ClassesRoot;
                case "HKEY_CURRENT_USER":
                    return Registry.CurrentUser;
                case "HKEY_LOCAL_MACHINE":
                    return Registry.LocalMachine;
                case "HKEY_USERS":
                    return Registry.Users;
                case "HKEY_CURRENT_CONFIG":
                    return Registry.CurrentConfig;
                default:
                    throw new Exception("Invalid registry key");
            }

        }

        public static RegistryKey OpenKey(string key)
        {
            RegistryKey _root;
            RegistryKey _key;

            _root = OpenRoot(key);

            string _value = key.Substring(key.IndexOf("\\") + 1);

            _key = _root.OpenSubKey(_value, true);
            if (_key == null)
                _key = _root.CreateSubKey(_value);

            return _key;
        }
    }

    public class Shell
    {
        [DllImport("shell32.dll", CharSet=CharSet.Auto)]
        private static extern uint ExtractIconEx
        (
            string szFileName,
            int nIconIndex,
            IntPtr[] phiconLarge,
            IntPtr[] phiconSmall,
            uint nIcons
        );

        private static string ReadDefaultKey(RegistryKey root, string key)
        {
            return ReadKey(root, key, "");
        }
        
        private static string ReadKey(RegistryKey root, string key, string value)
        {
            RegistryKey subKey = root.OpenSubKey(key);
            if(subKey != null)
            {
                object data = subKey.GetValue(value);
                if (data != null)
                    return (string)data;
            }
            return "";
        }

        public static void Launch(LaunchArgs launchArgs)
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = launchArgs.Application;
            psi.Arguments = launchArgs.Parameters;

            Process p = Process.Start(psi);

            if (launchArgs.WaitForExit)
            {
                p.WaitForInputIdle();
                p.WaitForExit();

                launchArgs.DoProcessCompleted();
            }
        }

        public static IconInfo ExtractIconEx(string fileName)
        {
            RegistryKey root = Registry.ClassesRoot;
            string fileAssociation = ReadDefaultKey(root, Path.GetExtension(fileName));
            string defaultIconPath = ReadDefaultKey(root, fileAssociation + "\\DefaultIcon");
            string fileDescription = ReadDefaultKey(root, fileAssociation);

            IconInfo result = new IconInfo(fileDescription);

            // split icon path, from the format "path\app.exe,1"
            string[] items = Regex.Split((string)defaultIconPath, ",");

            if (items.Length == 2)
            {
                // prepare pointers into Win32
                IntPtr[] pLarge = new IntPtr[1] {IntPtr.Zero};
                IntPtr[] pSmall = new IntPtr[1] {IntPtr.Zero};

                uint readIconCount = Laan.Utilities.Windows.Shell.ExtractIconEx(
                    items[0],
                    Int32.Parse(items[1]),
                    pLarge,
                    pSmall,
                    1
                );
    
                if(readIconCount > 0 && pSmall[0] != IntPtr.Zero)
                    result.Icon = (Icon)Icon.FromHandle(pSmall[0]).Clone();
    
            }
            return result;
        }
    }
}

