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
            ComponentsLoader.ComponentsLoader loader = new ComponentsLoader.ComponentsLoader();
            Assert.AreEqual(3, loader.GetComponents<IStringManipulator>().Count);
            Assert.AreEqual(1, loader.GetComponents<ICalculator>().Count);
        }
        [TestMethod]
        public void TestLoadMyCalculator()
        {
            ComponentsLoader.ComponentsLoader loader = new ComponentsLoader.ComponentsLoader();
            Assert.IsNotNull(loader.GetComponent<ICalculator>("My Calculator"));
        }
        [TestMethod]
        public void TestLoadLowerManipulator()
        {
            ComponentsLoader.ComponentsLoader loader = new ComponentsLoader.ComponentsLoader();
            Assert.IsNotNull(loader.GetComponent<IStringManipulator>("Lowercase String Manipulator"));
        }
        [TestMethod]
        public void TestLoadUpperManipulator()
        {
            ComponentsLoader.ComponentsLoader loader = new ComponentsLoader.ComponentsLoader();
            Assert.IsNotNull(loader.GetComponent<IStringManipulator>("Uppercase String Manipulator"));
        }
        [TestMethod]
        public void TestLoadReversingManipulator()
        {
            ComponentsLoader.ComponentsLoader loader = new ComponentsLoader.ComponentsLoader();
            Assert.IsNotNull(loader.GetComponent<IStringManipulator>("Reversing String Manipulator"));
        }


        [TestMethod]
        public void TestMyCalculator()
        {
            ComponentsLoader.ComponentsLoader loader = new ComponentsLoader.ComponentsLoader();
            ICalculator calc = loader.GetComponent<ICalculator>("My Calculator").Singleton;
            double a = -98.221;
            double b = 107.932;

            Assert.AreEqual(a + b, calc.Add(a, b));
            Assert.AreEqual(a - b, calc.Sub(a, b));
            Assert.AreEqual(a * b, calc.Mul(a, b));
            Assert.AreEqual(a / b, calc.Div(a, b));
        }
        [TestMethod]
        public void TestLowerManipulator()
        {
            ComponentsLoader.ComponentsLoader loader = new ComponentsLoader.ComponentsLoader();
            IStringManipulator manip = loader.GetComponent<IStringManipulator>("Lowercase String Manipulator").Singleton;
            Assert.AreEqual("abcdefg123", manip.Manip("ABcDefG123"));
        }
        [TestMethod]
        public void TestUpperManipulator()
        {
            ComponentsLoader.ComponentsLoader loader = new ComponentsLoader.ComponentsLoader();
            IStringManipulator manip = loader.GetComponent<IStringManipulator>("Uppercase String Manipulator").Singleton;
            Assert.AreEqual("ABCDEFG123", manip.Manip("ABcDefG123"));
        }
        [TestMethod]
        public void TestReversingManipulator()
        {
            ComponentsLoader.ComponentsLoader loader = new ComponentsLoader.ComponentsLoader();
            IStringManipulator manip = loader.GetComponent<IStringManipulator>("Reversing String Manipulator").Singleton;
            Assert.AreEqual("321GfeDcBA", manip.Manip("ABcDefG123"));
        }
    }
}
