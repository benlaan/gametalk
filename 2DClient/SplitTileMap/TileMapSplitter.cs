using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace SplitTileMap
{
    public class TileMapSplitter
    {
        private Image  _bitmap;
        private Size   _tileSize;
        private string _fileName;
        private string _outputDirectory;

        public TileMapSplitter(string fileName)
        {
            _fileName = fileName;
        }

        private void ReloadFile()
        {
            if (!File.Exists(_fileName))
                throw new ArgumentException(String.Format("File not found {0}", _fileName));

            Execute(_outputDirectory);
        }

        private void SetOutputDirectory(string outputDirectory)
        {
            _outputDirectory = outputDirectory;
            if (!Directory.Exists(_outputDirectory))
                Directory.CreateDirectory(_outputDirectory);
        }

        private void BuildTile(int row, int col)
        {
            using (Bitmap tileBitmap = new Bitmap(_bitmap, TileSize))
            {
                Graphics graphics = Graphics.FromImage(tileBitmap);
                graphics.DrawImage(
                    _bitmap, 
                    new Rectangle(new Point(0, 0), TileSize), 
                    new Rectangle(new Point(col, row), TileSize), 
                    GraphicsUnit.Pixel
                );
                
                string fileName = String.Format(
                    "{0}\\tile{1:00}-{2:00}.png", 
                    _outputDirectory, row / TileSize.Height, 
                    col / TileSize.Width
                );
                
                tileBitmap.Save(fileName, ImageFormat.Png);
            }
        }

        public void Execute(string outputDirectory)
        {
            if (_tileSize.Height == 0 || _tileSize.Width == 0)
                throw new ArgumentException("TileSize must have non-zero dimensions");

            SetOutputDirectory(outputDirectory);

            _bitmap = Bitmap.FromFile(_fileName);

            for (int row = 0; row < _bitmap.Height; row += TileSize.Height)
                for (int col = 0; col < _bitmap.Width; col += TileSize.Width)
                    BuildTile(row, col);
        }

        public string FileName
        {
            get { return _fileName; }
            set
            {
                if (_fileName == value)
                    return;

                ReloadFile();
                _fileName = value;
            }
        }

        public Size TileSize
        {
            get { return _tileSize; }
            set { _tileSize = value; }
        }

    }
}
