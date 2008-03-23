using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace SplitTileMap
{
    internal class Launcher
    {
        public const string cTILE_PATH = ".\\..\\..\\..\\Tiles";
        private const string cOUTPUT_FILE = ".\\..\\..\\..\\OutputMap.png";

        internal static void ExecuteSplitter(string[] args)
        {
            string fileName;
            Size size = new Size();
            string outputDirectory;

            if (args.Length == 5)
            {
                fileName = args[1];
                size.Height = Int32.Parse(args[2]);
                size.Width = Int32.Parse(args[3]);
                outputDirectory = args[4];
            }
            else
            {
                fileName = @"P:\Personal Projects\Risk\Tiles\BeachTiles.png"; // config file
                size.Height = 48; // config file
                size.Width = 64; // config file
                outputDirectory = cTILE_PATH;
            }

            TileMapSplitter splitter = new TileMapSplitter(fileName);
            splitter.TileSize = size;
            splitter.Execute(outputDirectory);

        }

        internal static void ExecuteMapper(string[] args)
        {
            string fileName;
            Size size = new Size();
            string tilePath;

            if (args.Length == 5)
            {
                fileName = args[1];
                size.Height = Int32.Parse(args[2]);
                size.Width = Int32.Parse(args[3]);
                tilePath = args[4];
            }
            else
            {
                fileName = Properties.Settings.Default.PoliticalMap;
                size.Height = 24; // config file
                size.Width = 32; // config file
                tilePath = cTILE_PATH;
            }

            TileMapEngine engine = new TileMapEngine();
            engine.Bitmap = new TileBitmap(fileName);
            engine.TilePath = tilePath;
            engine.Initialise(48);
            engine.GenerateMapToFile(cOUTPUT_FILE);
        }

        internal static void ExecuteRenamer()
        {
            string fileName = Properties.Settings.Default.PoliticalMap;
            Size size = new Size();
            string tilePath = cTILE_PATH;
            size.Height = 24; // config file
            size.Width = 32; // config file

            TileMapEngine engine = new TileMapEngine();
            engine.Bitmap = new TileBitmap(fileName);
            engine.TilePath = tilePath;

            //foreach (KeyValuePair<int, string> pair in engine._tiles)
            //{
            //    // , Convert.ToString(pair.Key, 2).PadLeft(8, '0')
            //    string toFile = String.Format("tile{0}.png", pair.Key.ToString().PadLeft(3, '0'));
            //    string fromFile = String.Format("tile{0}.png", pair.Value);
            //    Debug.WriteLine("MOVE " + fromFile + " " + toFile);
            //}
        }
    }
}
