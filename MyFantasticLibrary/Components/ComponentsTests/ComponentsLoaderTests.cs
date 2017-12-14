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
            Assert.AreEqual(2, loader.GetComponents<ICalculator>().Count);
        }
        [TestMethod]
        public void TestLoadMyCalculatorName()
        {
            ComponentsLoader.ComponentsLoader loader = new ComponentsLoader.ComponentsLoader();
            Assert.IsNotNull(loader.GetComponentByName<ICalculator>("My Calculator"));
            Assert.IsNull(loader.GetComponentByName<ICalculator>("My Calculator 2"));
        }
        [TestMethod]
        public void TestLoadMyCalculatorNameVersion()
        {
            ComponentsLoader.ComponentsLoader loader = new ComponentsLoader.ComponentsLoader();
            Assert.IsNotNull(loader.GetComponentByNameVersion<ICalculator>("My Calculator", "2.0"));
            Assert.IsNull(loader.GetComponentByNameVersion<ICalculator>("My Calculator 2", "2.0"));
            Assert.IsNull(loader.GetComponentByNameVersion<ICalculator>("My Calculator", "3.0"));
            Assert.IsNull(loader.GetComponentByNameVersion<ICalculator>("My Calculator 2", "3.0"));
        }
        [TestMethod]
        public void TestLoadMyCalculatorNamePublisher()
        {
            ComponentsLoader.ComponentsLoader loader = new ComponentsLoader.ComponentsLoader();
            Assert.IsNotNull(loader.GetComponentByNamePublisher<ICalculator>("My Calculator", "MJayJ II"));
            Assert.IsNull(loader.GetComponentByNamePublisher<ICalculator>("My Calculator 2", "MJayJ"));
            Assert.IsNull(loader.GetComponentByNamePublisher<ICalculator>("My Calculator", "MJayJ III"));
            Assert.IsNull(loader.GetComponentByNamePublisher<ICalculator>("My Calculator 2", "MJayJ III"));
        }
        [TestMethod]
        public void TestLoadMyCalculatorNameVersionPublisher()
        {
            ComponentsLoader.ComponentsLoader loader = new ComponentsLoader.ComponentsLoader();
            Assert.IsNotNull(loader.GetComponentByNameVersionPublisher<ICalculator>("My Calculator", "2.0", "MJayJ II"));

            Assert.IsNull(loader.GetComponentByNameVersionPublisher<ICalculator>("My Calculator 2", "2.0", "MJayJ II"));
            Assert.IsNull(loader.GetComponentByNameVersionPublisher<ICalculator>("My Calculator", "3.0", "MJayJ II"));
            Assert.IsNull(loader.GetComponentByNameVersionPublisher<ICalculator>("My Calculator", "2.0", "MJayJ III"));

            Assert.IsNull(loader.GetComponentByNameVersionPublisher<ICalculator>("My Calculator 2", "3.0", "MJayJ II"));
            Assert.IsNull(loader.GetComponentByNameVersionPublisher<ICalculator>("My Calculator 2", "2.0", "MJayJ III"));
            Assert.IsNull(loader.GetComponentByNameVersionPublisher<ICalculator>("My Calculator", "3.0", "MJayJ III"));

            Assert.IsNull(loader.GetComponentByNameVersionPublisher<ICalculator>("My Calculator 2", "3.0", "MJayJ III"));
        }
        [TestMethod]
        public void TestLoadLowerManipulator()
        {
            ComponentsLoader.ComponentsLoader loader = new ComponentsLoader.ComponentsLoader();
            Assert.IsNotNull(loader.GetComponentByName<IStringManipulator>("Lowercase String Manipulator"));
        }
        [TestMethod]
        public void TestLoadUpperManipulator()
        {
            ComponentsLoader.ComponentsLoader loader = new ComponentsLoader.ComponentsLoader();
            Assert.IsNotNull(loader.GetComponentByName<IStringManipulator>("Uppercase String Manipulator"));
        }
        [TestMethod]
        public void TestLoadReversingManipulator()
        {
            ComponentsLoader.ComponentsLoader loader = new ComponentsLoader.ComponentsLoader();
            Assert.IsNotNull(loader.GetComponentByName<IStringManipulator>("Reversing String Manipulator"));
        }

        [TestMethod]
        public void TestNoExistingStringManipulator()
        {
            ComponentsLoader.ComponentsLoader loader = new ComponentsLoader.ComponentsLoader();
            Assert.IsNull(loader.GetComponentByName<IStringManipulator>("Not exists"));
        }

        [TestMethod]
        public void TestMyCalculator()
        {
            ComponentsLoader.ComponentsLoader loader = new ComponentsLoader.ComponentsLoader();
            ICalculator calc = loader.GetComponentByName<ICalculator>("My Calculator").Singleton;
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
            IStringManipulator manip = loader.GetComponentByName<IStringManipulator>("Lowercase String Manipulator").Singleton;
            Assert.AreEqual("abcdefg123", manip.Manip("ABcDefG123"));
        }
        [TestMethod]
        public void TestUpperManipulator()
        {
            ComponentsLoader.ComponentsLoader loader = new ComponentsLoader.ComponentsLoader();
            IStringManipulator manip = loader.GetComponentByName<IStringManipulator>("Uppercase String Manipulator").Singleton;
            Assert.AreEqual("ABCDEFG123", manip.Manip("ABcDefG123"));
        }
        [TestMethod]
        public void TestReversingManipulator()
        {
            ComponentsLoader.ComponentsLoader loader = new ComponentsLoader.ComponentsLoader();
            IStringManipulator manip = loader.GetComponentByName<IStringManipulator>("Reversing String Manipulator").Singleton;
            Assert.AreEqual("321GfeDcBA", manip.Manip("ABcDefG123"));
        }
    }
}
