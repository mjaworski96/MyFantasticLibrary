using ComponentsLoader;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace ComponentsTests
{
    [TestClass]
    public class ComponentsLoaderTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            Loader loader = new Loader();
            List<Type> types = loader.GetTypes();
            Assert.AreEqual(4, types.Count);
        }
    }
}
