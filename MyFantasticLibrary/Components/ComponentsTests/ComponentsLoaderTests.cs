using AbstractClassContract;
using Calculator;
using ComponentsLoader;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StringManipulator;
using System;

namespace ComponentsTests
{
    [TestClass]
    public class ComponentsLoaderTests
    {
        [TestMethod]
        public void TestLoadComponents()
        {
            Assert.AreEqual(3, Loader.GetComponents<IStringManipulator>().Count);
            Assert.AreEqual(2, Loader.GetComponents<ICalculator>().Count);
        }
        [TestMethod]
        public void TestLoadMyCalculatorName()
        {
            Assert.IsNotNull(Loader.GetComponentByName<ICalculator>("My Calculator"));
            Assert.IsNull(Loader.GetComponentByName<ICalculator>("My Calculator2"));
        }
        [TestMethod]
        public void TestLoadMyCalculatorNameVersion()
        {
            Assert.IsNotNull(Loader.GetComponentByNameVersion<ICalculator>("My Calculator", "2.0"));
            Assert.IsNull(Loader.GetComponentByNameVersion<ICalculator>("My Calculator2", "2.0"));
            Assert.IsNull(Loader.GetComponentByNameVersion<ICalculator>("My Calculator", "3.0"));
            Assert.IsNull(Loader.GetComponentByNameVersion<ICalculator>("My Calculator2", "3.0"));
        }
        [TestMethod]
        public void TestLoadMyCalculatorNamePublisher()
        {
            Assert.IsNotNull(Loader.GetComponentByNamePublisher<ICalculator>("My Calculator", "MJayJ II"));
            Assert.IsNull(Loader.GetComponentByNamePublisher<ICalculator>("My Calculator2", "MJayJ"));
            Assert.IsNull(Loader.GetComponentByNamePublisher<ICalculator>("My Calculator", "MJayJ III"));
            Assert.IsNull(Loader.GetComponentByNamePublisher<ICalculator>("My Calculator2", "MJayJ III"));
        }
        [TestMethod]
        public void TestLoadMyCalculatorNameVersionPublisher()
        {
            Assert.IsNotNull(Loader.GetComponentByNameVersionPublisher<ICalculator>("My Calculator", "2.0", "MJayJ II"));

            Assert.IsNull(Loader.GetComponentByNameVersionPublisher<ICalculator>("My Calculator2", "2.0", "MJayJ II"));
            Assert.IsNull(Loader.GetComponentByNameVersionPublisher<ICalculator>("My Calculator", "3.0", "MJayJ II"));
            Assert.IsNull(Loader.GetComponentByNameVersionPublisher<ICalculator>("My Calculator", "2.0", "MJayJ III"));

            Assert.IsNull(Loader.GetComponentByNameVersionPublisher<ICalculator>("My Calculator2", "3.0", "MJayJ II"));
            Assert.IsNull(Loader.GetComponentByNameVersionPublisher<ICalculator>("My Calculator2", "2.0", "MJayJ III"));
            Assert.IsNull(Loader.GetComponentByNameVersionPublisher<ICalculator>("My Calculator", "3.0", "MJayJ III"));

            Assert.IsNull(Loader.GetComponentByNameVersionPublisher<ICalculator>("My Calculator2", "3.0", "MJayJ III"));
        }
        [TestMethod]
        public void TestLoadLowerManipulator()
        {
            Assert.IsNotNull(Loader.GetComponentByName<IStringManipulator>("Lowercase String Manipulator"));
        }
        [TestMethod]
        public void TestLoadUpperManipulator()
        {
            Assert.IsNotNull(Loader.GetComponentByName<IStringManipulator>("Uppercase String Manipulator"));
        }
        [TestMethod]
        public void TestLoadReversingManipulator()
        {
            Assert.IsNotNull(Loader.GetComponentByName<IStringManipulator>("Reversing String Manipulator"));
        }

        [TestMethod]
        public void TestNoExistingStringManipulator()
        {
            Assert.IsNull(Loader.GetComponentByName<IStringManipulator>("Not exists"));
        }

        [TestMethod]
        public void TestMyCalculator()
        {
            ICalculator calc = Loader.GetComponentByName<ICalculator>("My Calculator").Singleton;
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
            ICalculator calc = Loader.GetComponentByNameVersion<ICalculator>("My Calculator", "2.0").Singleton;
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
            IStringManipulator manip = Loader.GetComponentByName<IStringManipulator>("Lowercase String Manipulator").Singleton;
            Assert.AreEqual("abcdefg123", manip.Manip("ABcDefG123"));
        }
        [TestMethod]
        public void TestUpperManipulator()
        {
            IStringManipulator manip = Loader.GetComponentByName<IStringManipulator>("Uppercase String Manipulator").Singleton;
            Assert.AreEqual("ABCDEFG123", manip.Manip("ABcDefG123"));
        }
        [TestMethod]
        public void TestReversingManipulator()
        {
            IStringManipulator manip = Loader.GetComponentByName<IStringManipulator>("Reversing String Manipulator").Singleton;
            Assert.AreEqual("321GfeDcBA", manip.Manip("ABcDefG123"));
        }
        [TestMethod]
        public void TestGetComponentsFromConfiguration()
        {
            Assert.AreEqual(2, Loader.GetComponentsFromConfiguration<ICalculator>().Count);
            Assert.AreEqual(3, Loader.GetComponentsFromConfiguration<IStringManipulator>().Count);
        }
        [TestMethod]
        public void TestGetAbstractClassComponent()
        {
            Assert.IsNotNull(Loader.GetComponentByName<DoSomething>("Abstract Class Contract Implementation"));
        }
        [TestMethod]
        public void TestIsComponentAvaiable()
        {
            Assert.IsTrue(Loader.IsComponentAvaiableByName<ICalculator>("My Calculator"));
            Assert.IsFalse(Loader.IsComponentAvaiableByName<ICalculator>("My Calulator 3"));

            Assert.IsTrue(Loader.IsComponentAvaiableByNameVersion<IStringManipulator>("Reversing String Manipulator", "1.0"));
            Assert.IsFalse(Loader.IsComponentAvaiableByNameVersion<IStringManipulator>("My Calulator", "2.0"));

            Assert.IsTrue(Loader.IsComponentAvaiableByNamePublisher<ICalculator>("My Calculator", "MJayJ"));
            Assert.IsFalse(Loader.IsComponentAvaiableByNamePublisher<ICalculator>("My Calulator", "My Company"));

            Assert.IsTrue(Loader.IsComponentAvaiableByNameVersionPublisher<IStringManipulator>("Reversing String Manipulator", "1.0", "MJayJ"));
            Assert.IsFalse(Loader.IsComponentAvaiableByNameVersionPublisher<ICalculator>("My Calculator Pro", "2.1", "My Company"));
        }
        [TestMethod]
        public void TestGetAssemblyAndReferencedAssembliesFromComponent()
        {
            LoadedComponent<IStringManipulator> component =
                Loader.GetComponentByName<IStringManipulator>("Reversing String Manipulator");
            Assert.AreEqual("ReversingManipulator", component.AssemblyName.Name);
            Assert.AreEqual(3, component.ReferencesAssembliesNames.Length);
            Assert.AreEqual("netstandard", component.ReferencesAssembliesNames[0].Name);
            Assert.AreEqual("ComponentContract", component.ReferencesAssembliesNames[1].Name);
            Assert.AreEqual("StringManipulator", component.ReferencesAssembliesNames[2].Name);
        }
        [TestMethod]
        [ExpectedException(typeof(NotComponentTypeException))]
        public void TestCreateLoadedComponentFromNotComponentType()
        {
            LoadedComponent<ICalculator> component = new LoadedComponent<ICalculator>(typeof(ComponentsLoaderTests));
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestGetComponentByNameNullName()
        {
            Loader.GetComponentByName<ICalculator>(null);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestGetComponentByNameEmptyParameter()
        {
            Loader.GetComponentByName<ICalculator>("");
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestGetComponentByNameNullDirectory()
        {
            Loader.GetComponentByName<ICalculator>("My Calculator", null);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestGetComponentByNameEmptyDirectory()
        {
            Loader.GetComponentByName<ICalculator>("My Calculator", "");
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestIsComponentAvaiableByNameNullName()
        {
            Loader.IsComponentAvaiableByName<ICalculator>(null);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestIsComponentAvaiableByNameEmptyParameter()
        {
            Loader.IsComponentAvaiableByName<ICalculator>("");
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestIsComponentAvaiableByNameNullDirectory()
        {
            Loader.IsComponentAvaiableByName<ICalculator>("My Calculator", null);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestIsComponentAvaiableByNameEmptyDirectory()
        {
            Loader.IsComponentAvaiableByName<ICalculator>("My Calculator", "");
        }
    }
}
