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
            Assert.AreEqual(2, loader.GetComponents<ICalculator>().Count);
        }
        [TestMethod]
        public void TestLoadMyCalculatorName()
        {
            Loader loader = new Loader();
            Assert.IsNotNull(loader.GetComponentByName<ICalculator>("My Calculator"));
            Assert.IsNull(loader.GetComponentByName<ICalculator>("My Calculator2"));
        }
        [TestMethod]
        public void TestLoadMyCalculatorNameVersion()
        {
            Loader loader = new Loader();
            Assert.IsNotNull(loader.GetComponentByNameVersion<ICalculator>("My Calculator", "2.0"));
            Assert.IsNull(loader.GetComponentByNameVersion<ICalculator>("My Calculator2", "2.0"));
            Assert.IsNull(loader.GetComponentByNameVersion<ICalculator>("My Calculator", "3.0"));
            Assert.IsNull(loader.GetComponentByNameVersion<ICalculator>("My Calculator2", "3.0"));
        }
        [TestMethod]
        public void TestLoadMyCalculatorNamePublisher()
        {
            Loader loader = new Loader();
            Assert.IsNotNull(loader.GetComponentByNamePublisher<ICalculator>("My Calculator", "MJayJ II"));
            Assert.IsNull(loader.GetComponentByNamePublisher<ICalculator>("My Calculator2", "MJayJ"));
            Assert.IsNull(loader.GetComponentByNamePublisher<ICalculator>("My Calculator", "MJayJ III"));
            Assert.IsNull(loader.GetComponentByNamePublisher<ICalculator>("My Calculator2", "MJayJ III"));
        }
        [TestMethod]
        public void TestLoadMyCalculatorNameVersionPublisher()
        {
            Loader loader = new Loader();
            Assert.IsNotNull(loader.GetComponentByNameVersionPublisher<ICalculator>("My Calculator", "2.0", "MJayJ II"));

            Assert.IsNull(loader.GetComponentByNameVersionPublisher<ICalculator>("My Calculator2", "2.0", "MJayJ II"));
            Assert.IsNull(loader.GetComponentByNameVersionPublisher<ICalculator>("My Calculator", "3.0", "MJayJ II"));
            Assert.IsNull(loader.GetComponentByNameVersionPublisher<ICalculator>("My Calculator", "2.0", "MJayJ III"));

            Assert.IsNull(loader.GetComponentByNameVersionPublisher<ICalculator>("My Calculator2", "3.0", "MJayJ II"));
            Assert.IsNull(loader.GetComponentByNameVersionPublisher<ICalculator>("My Calculator2", "2.0", "MJayJ III"));
            Assert.IsNull(loader.GetComponentByNameVersionPublisher<ICalculator>("My Calculator", "3.0", "MJayJ III"));

            Assert.IsNull(loader.GetComponentByNameVersionPublisher<ICalculator>("My Calculator2", "3.0", "MJayJ III"));
        }
        [TestMethod]
        public void TestLoadLowerManipulator()
        {
            Loader loader = new Loader();
            Assert.IsNotNull(loader.GetComponentByName<IStringManipulator>("Lowercase String Manipulator"));
        }
        [TestMethod]
        public void TestLoadUpperManipulator()
        {
            Loader loader = new Loader();
            Assert.IsNotNull(loader.GetComponentByName<IStringManipulator>("Uppercase String Manipulator"));
        }
        [TestMethod]
        public void TestLoadReversingManipulator()
        {
            Loader loader = new Loader();
            Assert.IsNotNull(loader.GetComponentByName<IStringManipulator>("Reversing String Manipulator"));
        }

        [TestMethod]
        public void TestNoExistingStringManipulator()
        {
            Loader loader = new Loader();
            Assert.IsNull(loader.GetComponentByName<IStringManipulator>("Not exists"));
        }

        [TestMethod]
        public void TestMyCalculator()
        {
            Loader loader = new Loader();
            ICalculator calc = loader.GetComponentByName<ICalculator>("My Calculator").Singleton;
            double a = -98.221;
            double b = 107.932;

            Assert.AreEqual(a + b, calc.Add(a, b));
            Assert.AreEqual(a - b, calc.Sub(a, b));
            Assert.AreEqual(a * b, calc.Mul(a, b));
            Assert.AreEqual(a / b, calc.Div(a, b));
        }
        [TestMethod]
        public void TestMyCalculator2()
        {
            Loader loader = new Loader();
            ICalculator calc = loader.GetComponentByNameVersion<ICalculator>("My Calculator", "2.0").Singleton;
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
            Loader loader = new Loader();
            IStringManipulator manip = loader.GetComponentByName<IStringManipulator>("Lowercase String Manipulator").Singleton;
            Assert.AreEqual("abcdefg123", manip.Manip("ABcDefG123"));
        }
        [TestMethod]
        public void TestUpperManipulator()
        {
            Loader loader = new Loader();
            IStringManipulator manip = loader.GetComponentByName<IStringManipulator>("Uppercase String Manipulator").Singleton;
            Assert.AreEqual("ABCDEFG123", manip.Manip("ABcDefG123"));
        }
        [TestMethod]
        public void TestReversingManipulator()
        {
            Loader loader = new Loader();
            IStringManipulator manip = loader.GetComponentByName<IStringManipulator>("Reversing String Manipulator").Singleton;
            Assert.AreEqual("321GfeDcBA", manip.Manip("ABcDefG123"));
        }
        [TestMethod]
        public void TestGetComponentsFromConfiguration()
        {
            Loader loader = new Loader();
            Assert.AreEqual(2, loader.GetComponentsFromConfiguration<ICalculator>().Count);
            Assert.AreEqual(3, loader.GetComponentsFromConfiguration<IStringManipulator>().Count);
        }
    }
}
