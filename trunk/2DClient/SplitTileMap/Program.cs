using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace SplitTileMap
{
    class Program
    {
        static void Main(string[] args)
        {
            Application.Run(new FormMain());
            //Launcher.ExecuteRenamer();
//            Launcher.ExecuteMapper(args);
//            Launcher.ExecuteSplitter(args);
/*
            switch (args[0])
            {
                case "split":
                    ExecuteSplitter(args);
                    break;

                case "map":
                    ExecuteMapper(args);
                    break;
            }
*/
        }
    }
}