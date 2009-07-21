using System;
using System.Collections;
using System.ComponentModel;
using System.Data;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Laan.Utilities;

namespace HSL.Test
{

    [TestClass()]
    public class TestCommandLineParser
    {
        const string cTEST_FILE_XML = @"D:\test.xml";

        private CommandLineParser _parser;

        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void SetUp()
        {
            _parser = new CommandLineParser();
        }

        [TestCleanup]
        public void TearDown()
        {
        }

        [TestMethod]
        public void TestArgumentsDiffSwitchOneParam()
        {
            string[] args = new string[] { "+m" };
            _parser.Switches = new char[] { '+' };
            _parser.Parse(args);

            Assert.AreEqual(1, _parser.Arguments.Count);
            Assert.AreEqual("", _parser.Arguments["m"]);
        }

        [TestMethod]
        public void TestArgumentsDiffSwitchTwoParams()
        {
            string[] args = new string[] { "+m", "+k" };
            _parser.Switches = new char[] { '+' };
            _parser.Parse(args);

            Assert.AreEqual(2, _parser.Arguments.Count);
            Assert.AreEqual("", _parser.Arguments["m"]);
            Assert.AreEqual("", _parser.Arguments["k"]);
        }

        [TestMethod]
        public void TestArgumentsMissingParam()
        {
            string[] args = new string[] { "/c" };
            _parser.Parse(args);

            Assert.AreEqual(1, _parser.Arguments.Count);
            string value = _parser.Arguments["notThis"];
            Assert.AreEqual(null, value);
        }

        [TestMethod]
        public void TestArgumentsNoParams()
        {
            _parser.Parse(new string[] { });
            Assert.AreEqual(0, _parser.Arguments.Count);
        }

        [TestMethod]
        public void TestArgumentsOneParam()
        {
            string[] args = new string[] { "/c" };
            _parser.Parse(args);

            Assert.AreEqual(1, _parser.Arguments.Count);
            Assert.AreEqual("", _parser.Arguments["c"]);
        }

        [TestMethod]
        public void TestArgumentsOneParamWithValue()
        {
            string[] args = new string[] { "/output", cTEST_FILE_XML };
            _parser.Parse(args);

            Assert.AreEqual(1, _parser.Arguments.Count);
            Assert.AreEqual(cTEST_FILE_XML, _parser.Arguments["output"]);
        }

        [TestMethod]
        public void TestArgumentsParamWithSpaces()
        {
            _parser.Parse("/folder \"My Documents\"");

            Assert.AreEqual(1, _parser.Arguments.Count);
            Assert.AreEqual("My Documents", _parser.Arguments["folder"]);
        }

        [TestMethod]
        public void TestArgumentsTwoParamsBothHaveValue()
        {
            string[] args = new string[] { "/crud", "update", "/output", cTEST_FILE_XML };
            _parser.Parse(args);

            Assert.AreEqual(2, _parser.Arguments.Count);
            Assert.AreEqual(cTEST_FILE_XML, _parser.Arguments["output"]);
            Assert.AreEqual("update", _parser.Arguments["crud"]);
        }

        [TestMethod]
        public void TestArgumentsTwoParamsFirstHasValue()
        {
            string[] args = new string[] { "/input", cTEST_FILE_XML, "/b" };
            _parser.Parse(args);

            Assert.AreEqual(2, _parser.Arguments.Count);
            Assert.AreEqual(cTEST_FILE_XML, _parser.Arguments["input"]);
            Assert.AreEqual("", _parser.Arguments["b"]);
        }

        [TestMethod]
        public void TestArgumentsTwoParamsNoValues()
        {
            string[] args = new string[] { "/m", "/b" };
            _parser.Parse(args);

            Assert.AreEqual(2, _parser.Arguments.Count);
            Assert.AreEqual("", _parser.Arguments["m"]);
            Assert.AreEqual("", _parser.Arguments["b"]);
        }

        [TestMethod]
        public void TestArgumentsTwoParamsSecondHasValue()
        {
            string[] args = new string[] { "/m", "/output", cTEST_FILE_XML };
            _parser.Parse(args);

            Assert.AreEqual(2, _parser.Arguments.Count);
            Assert.AreEqual(cTEST_FILE_XML, _parser.Arguments["output"]);
            Assert.AreEqual("", _parser.Arguments["m"]);
        }
    }
}
