using Laan.Utilities.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Laan.Library.Test
{


    /// <summary>
    ///This is a test class for StringHelperTest and is intended
    ///to contain all StringHelperTest Unit Tests
    ///</summary>
    [TestClass()]
    public class StringHelperTest
    {
        public TestContext TestContext { get; set; }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        [TestMethod()]
        public void XORTest()
        {
            string input = "This is a test";
            string strKey = "SOMEKEY";
            string actual = StringHelper.XOR(input, strKey);

            // ensure that XOR did something
            Assert.AreNotEqual(input, actual);

            // XOR is sunchronous, thus the test can check that the result of XORing input, 
            // then XORed will be input
            // 
            // ie. input XOR key ==> result
            // and result XOR key ==> input
            string synchValue = StringHelper.XOR(actual, strKey);

            Assert.AreEqual(input, synchValue);
        }

        [TestMethod()]
        public void ToTitleCaseTest()
        {
            var data = new[] 
            { 
                new { Input = "", Expected = "" },
                new { Input = "HELLO WORLD", Expected = "Hello World" },
                new { Input = "HELLO world", Expected = "Hello World" },
                new { Input = "hello WORLD", Expected = "Hello World" },
                new { Input = "hello world", Expected = "Hello World" },
            };

            foreach (var datum in data)
                Assert.AreEqual(datum.Expected, StringHelper.ToTitleCase(datum.Input));
        }

        [TestMethod()]
        public void ToCamelCaseTest()
        {
            var data = new[] 
            { 
                new { Input = "", Expected = "" },
                new { Input = "SomeProperty", Expected = "someProperty" },
                new { Input = "SomePROPERTY", Expected = "somePROPERTY" },
            };

            foreach (var datum in data)
                Assert.AreEqual(datum.Expected, StringHelper.ToCamelCase(datum.Input));
        }

        [TestMethod()]
        public void RightTest()
        {
            var data = new[] 
            { 
                new { Input = "", Length = 0, Expected = "" },
                new { Input = "Gimme Three", Length = 3, Expected = "ree" },
                new { Input = "Test", Length = 9, Expected = "Test" },
            };

            foreach (var datum in data)
                Assert.AreEqual(datum.Expected, StringHelper.Right(datum.Input, datum.Length));
        }

        [TestMethod()]
        public void ReverseTest()
        {
            var data = new[] 
            { 
                new { Input = "", Expected = "" },
                new { Input = "Reversed Text", Expected = "txeT desreveR" },
                new { Input = "Odd", Expected = "ddO" },
            };

            foreach (var datum in data)
                Assert.AreEqual(datum.Expected, StringHelper.Reverse(datum.Input));
        }

        [TestMethod()]
        public void LeftTest()
        {
            var data = new[] 
            { 
                new { Input = "", Length = 0, Expected = "" },
                new { Input = "Gimme Three", Length = 3, Expected = "Gim" },
                new { Input = "Test", Length = 9, Expected = "Test" },
            };

            foreach (var datum in data)
                Assert.AreEqual(datum.Expected, StringHelper.Left(datum.Input, datum.Length));
        }

        [TestMethod()]
        public void IsNullOrEmptyTest()
        {
            var data = new[] 
            { 
                new { Input = "", Expected = true },
                new { Input = (string)null, Expected = true },
                new { Input = "This is not NULL", Expected = false },
            };

            foreach (var datum in data)
                Assert.AreEqual(datum.Expected, StringHelper.IsNullOrEmpty(datum.Input));
        }

        [TestMethod()]
        public void FormattedTest()
        {
            string format = string.Empty; // TODO: Initialize to an appropriate value
            object[] data = null; // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = StringHelper.Formatted(format, data);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
