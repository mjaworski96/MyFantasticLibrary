﻿using ConfigurationManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConfigurationTests
{
    [TestClass]
    public class ConfigurationTest
    {
        [TestMethod]
        public void TestGetMethod()
        {
            Configuration configuration = new Configuration();
            configuration.LoadConfiguration("configuration.myconfig");

            Assert.AreEqual("value1", configuration.GetString("key1"));
            Assert.AreEqual("value3", configuration.GetString("key2.key3"));
            Assert.AreEqual(2, configuration.GetListOfFields("list").Count);
            Assert.AreEqual(2, configuration.GetListOfFields("list")[0].Fields.Count);
            Assert.AreEqual(3, configuration.GetListOfFields("list")[1].Fields.Count);
        }
        [TestMethod]
        public void TestSetMethod()
        {
            Configuration configuration = new Configuration();
            configuration.LoadConfiguration("configuration.myconfig");

            configuration.SetString("key1", "value2");
            Assert.AreEqual("value2", configuration.GetString("key1"));
            configuration.SetString("key2.key4", "value4");
            Assert.AreEqual("value4", configuration.GetString("key2.key4"));

        }
    }
}
