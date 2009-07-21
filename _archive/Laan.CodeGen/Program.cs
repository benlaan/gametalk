using System;
using System.Diagnostics;
using System.Windows.Forms;

using Laan.Utilities.Windows;
using Laan.Utilities;

namespace Laan.CodeGen
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                Win32Libs.AttachConsole(Win32Libs.ATTACH_PARENT_PROCESS);
                Debug.Listeners.Add(new ConsoleTraceListener());

                CommandLineParser parser = new CommandLineParser();
                parser.Parse(args);

                string mapFile = parser.Arguments["map"] ?? "RiskMap.xml";
                string outputPath = parser.Arguments["output"] ?? @"\..\..\..\CodeGen\output";

                CodeGenerator.Run(mapFile, outputPath, true);
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FormCodeGen());
            }
        }
    }
}
