using Calculator;
using ComponentsLoader;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StringManipulator;
using System;
using System.Collections.Generic;

namespace ComponentsTests
{
    [TestClass]
    public class ComponentsLoaderTests
    {
        [TestMethod]
        public void TestLoadComponents()
        {
            Loader loader = new Loader();
            Assert.AreEqual(3, loader.GetComponents<IStringManipulator>().Count);
            Assert.AreEqual(1, loader.GetComponents<ICalculator>().Count);
        }
        [TestMethod]
        public void TestLoadMyCalculator()
        {
            Loader loader = new Loader();
            Assert.IsNotNull(loader.GetComponent<ICalculator>("My Calculator"));
        }
        [TestMethod]
        public void TestLoadLowerManipulator()
        {
            Loader loader = new Loader();
            Assert.IsNotNull(loader.GetComponent<IStringManipulator>("Lowercase String Manipulator"));
        }
        [TestMethod]
        public void TestLoadUpperManipulator()
        {
            Loader loader = new Loader();
            Assert.IsNotNull(loader.GetComponent<IStringManipulator>("Uppercase String Manipulator"));
        }
        [TestMethod]
        public void TestLoadReversingManipulator()
        {
            Loader loader = new Loader();
            Assert.IsNotNull(loader.GetComponent<IStringManipulator>("Reversing String Manipulator"));
        }
    }
}
