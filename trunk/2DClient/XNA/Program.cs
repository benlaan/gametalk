using System;

namespace Laan.Riskier.Client.TileMap
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (TileMap game = new TileMap())
            {
                game.Run();
            }
        }
    }
}

