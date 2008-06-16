using System.Collections;
using System.Collections.Generic;
using Laan.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Laan.Library.Test
{
    
    
    /// <summary>
    ///This is a test class for ReflectionTest and is intended
    ///to contain all ReflectionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ReflectionTest
    {
        private const string cASSEMBLY_PATH = 
            @"D:\Development\GoogleCode\Laan.GameLibrary\Laan.Risk.Entities.CodeGen\bin\Debug\";

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

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

        private static void CompareListsContainSameTypes<T>(IList<T> expected, IList<T> actual)
        {
            Assert.AreEqual(expected.Count, actual.Count);
            for (int index = 0; index < actual.Count; index++)
            {
                var e = expected[index];
                var a = actual[index];
                Assert.IsTrue(a.GetType().FullName == e.GetType().FullName);
            }
        }

        private void FindByAssemblyByLingTest<T>() where T : class, new()
        {
            IList<T> expected = new List<T>();
            expected.Add(new T());
            IList<T> actual = Reflection.FindByAssemblyByLing<T>(cASSEMBLY_PATH);

            CompareListsContainSameTypes<T>(expected, actual);
        }

        /// <summary>
        ///A test for FindByAssemblyByLing
        ///</summary>
        [TestMethod()]
        public void FindByAssemblyByLingTestHelper()
        {
            FindByAssemblyByLingTest<GenericParameterHelper>();
        }

        public void FindByAssemblyTest<T>() where T : class, new()
        {
            IList<T> expected = new List<T>();
            expected.Add(new T());

            IList<T> actual = Reflection.FindByAssembly<T>(cASSEMBLY_PATH);
            CompareListsContainSameTypes<T>(expected, actual);
        }

        /// <summary>
        ///A test for FindByAssemblyByLing
        ///</summary>
        [TestMethod()]
        public void FindByAssemblyTestHelper()
        {
            FindByAssemblyTest<GenericParameterHelper>();
        }
    }
}
