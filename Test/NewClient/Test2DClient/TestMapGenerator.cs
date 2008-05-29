using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using NUnit.Framework;

using SplitTileMap;

namespace SplitTileMap
{
    [TestFixture]
    public class TestMapengine
    {
        [Test]
        public void TestSamplePoint1()
        {
            TileMapEngine engine = new TileMapEngine();

            MockBitmap mockBitmap = new MockBitmap(
                new int[][] { 
                    new int[] { 0, 0, 0 }, 
                    new int[] { 0, 1, 0 }, 
                    new int[] { 0, 0, 0 }
                }
            );
            engine.Bitmap = mockBitmap;
            int actual = engine.GetIndexFromMapPoint(new Point(0, 0));

            Assert.AreEqual(128, actual);
        }

        [Test]
        public void TestSamplePoint2()
        {
            TileMapEngine engine = new TileMapEngine();

            MockBitmap mockBitmap = new MockBitmap(
                new int[][] { 
                    new int[] { 0, 1, 0 }, 
                    new int[] { 1, 1, 0 }, 
                    new int[] { 0, 0, 0 }
                }
            );
            engine.Bitmap = mockBitmap;
            int actual = engine.GetIndexFromMapPoint(new Point(0, 0));

            Assert.AreEqual(16 + 64 + 128, actual);
        }

        [Test]
        public void TestSamplePoint3()
        {
            TileMapEngine engine = new TileMapEngine();

            MockBitmap mockBitmap = new MockBitmap(
                new int[][] { 
                    new int[] { 1, 1, 1 }, 
                    new int[] { 1, 0, 1 }, 
                    new int[] { 1, 1, 1 }
                }
            );
            engine.Bitmap = mockBitmap;
            int actual = engine.GetIndexFromMapPoint(new Point(1, 1));

            Assert.AreEqual(255, actual);
        }

        [Test]
        public void TestSamplePointTopEdge()
        {
            TileMapEngine engine = new TileMapEngine();

            MockBitmap mockBitmap = new MockBitmap(
                new int[][] { 
                    new int[] { 1, 1, 1 }, 
                    new int[] { 0, 0, 0 }, 
                    new int[] { 0, 0, 0 }
                }
            );
            engine.Bitmap = mockBitmap;
            int actual = engine.GetIndexFromMapPoint(new Point(1, 1));

            Assert.AreEqual(7, actual);
        }

        [Test]
        public void TestSamplePointBottomEdge()
        {
            TileMapEngine engine = new TileMapEngine();

            MockBitmap mockBitmap = new MockBitmap(
                new int[][] { 
                    new int[] { 0, 0, 0 }, 
                    new int[] { 0, 0, 0 },
                    new int[] { 1, 1, 1 } 
                }
            );
            engine.Bitmap = mockBitmap;
            int actual = engine.GetIndexFromMapPoint(new Point(1, 1));

            Assert.AreEqual(224, actual);
        }

        [Test]
        public void TestSamplePointLeftEdge()
        {
            TileMapEngine engine = new TileMapEngine();

            MockBitmap mockBitmap = new MockBitmap(
                new int[][] { 
                    new int[] { 1, 0, 0 }, 
                    new int[] { 1, 0, 0 }, 
                    new int[] { 1, 0, 0 }
                }
            );
            engine.Bitmap = mockBitmap;
            int actual = engine.GetIndexFromMapPoint(new Point(1, 1));

            Assert.AreEqual(41, actual);
        }

        [Test]
        public void TestSamplePointRightEdge()
        {
            TileMapEngine engine = new TileMapEngine();

            MockBitmap mockBitmap = new MockBitmap(
                new int[][] { 
                    new int[] { 0, 0, 1 }, 
                    new int[] { 0, 0, 1 },
                    new int[] { 0, 0, 1 } 
                }
            );
            engine.Bitmap = mockBitmap;
            int actual = engine.GetIndexFromMapPoint(new Point(1, 1));

            Assert.AreEqual(148, actual);
        }

        private MockBitmap Blank3x3Bitmap()
        {
            return new MockBitmap(
                new int[][] {
                    new int[] { 0, 0, 0 },
                    new int[] { 0, 0, 0 },
                    new int[] { 0, 0, 0 }
                }
            );
        }

        [Test]
        public void TestNoOffset()
        {
            TileMapEngine engine = new TileMapEngine();
            engine.Bitmap = Blank3x3Bitmap();

            Point actual = engine.GetPointWithOffset(0, 0);

            Assert.AreEqual(0, actual.X);
            Assert.AreEqual(0, actual.Y);
        }

        [Test]
        public void TestSimpleOffset()
        {
            TileMapEngine engine = new TileMapEngine();
            engine.Bitmap = Blank3x3Bitmap();

            engine.OffsetX = 2;
            engine.OffsetY = 2;
            Point actual = engine.GetPointWithOffset(0, 0);

            Assert.AreEqual(2, actual.X);
            Assert.AreEqual(2, actual.Y);
        }

        private MockBitmap Blank4x4Bitmap()
        {
            return new MockBitmap(
                new int[][] {
                    new int[] { 0, 0, 0, 0 },
                    new int[] { 0, 0, 0, 0 },
                    new int[] { 0, 0, 0, 0 },
                    new int[] { 0, 0, 0, 0 }
                }
            );
        }

        [Test]
        public void TestNegativeOffset()
        {
            TileMapEngine engine = new TileMapEngine();
            engine.Bitmap = Blank4x4Bitmap();

            engine.OffsetX = -1;
            Point actual = engine.GetPointWithOffset(0, 0);

            Assert.AreEqual(3, actual.X);
            Assert.AreEqual(0, actual.Y);
        }

        [Test]
        public void TestNegativeOffsetAtMapEdge()
        {
            TileMapEngine engine = new TileMapEngine();
            engine.Bitmap = Blank4x4Bitmap();

            engine.OffsetX = -1;
            Point actual = engine.GetPointWithOffset(3, 3);

            Assert.AreEqual(2, actual.X);
            Assert.AreEqual(3, actual.Y);
        }

        [Test]
        public void TestPositiveOffsetAtMapEdge()
        {
            TileMapEngine engine = new TileMapEngine();
            engine.Bitmap = Blank4x4Bitmap();

            engine.OffsetX = 2;
            Point actual = engine.GetPointWithOffset(3, 3);

            Assert.AreEqual(1, actual.X);
            Assert.AreEqual(3, actual.Y);
        }
    }
}
