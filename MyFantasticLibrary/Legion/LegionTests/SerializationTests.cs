using System;
using System.IO;
using LegionCore.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LegionTests
{
    [TestClass]
    public class SerializationTests
    {
        public class TestClass
        {
            public int IntProp { get; set; }
            public string stringField;

            public TestClass()
            {

            }

            public TestClass(int intProp, string stringField)
            {
                IntProp = intProp;
                this.stringField = stringField;
            }
        }

        [TestMethod]
        public void TestJson()
        {
            TestClass testClass = new TestClass(1, "JSON");
            Stream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            JsonSerial json = new JsonSerial();

            json.Serialize(testClass, typeof(TestClass), writer);
            Assert.AreEqual(34, stream.Position);
            stream.Position = 0;
            StreamReader reader = new StreamReader(stream);
            TestClass deserialized = (TestClass)json.Deserialize(typeof(TestClass), reader);

            Assert.AreEqual(1, deserialized.IntProp);
            Assert.AreEqual("JSON", deserialized.stringField);
        }
        [TestMethod]
        public void TestXml()
        {
            TestClass testClass = new TestClass(2, "XML");
            Stream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            XmlSerial xml = new XmlSerial();

            xml.Serialize(testClass, typeof(TestClass), writer);
            Assert.AreEqual(222, stream.Position);
            stream.Position = 0;
            StreamReader reader = new StreamReader(stream);
            TestClass deserialized = (TestClass)xml.Deserialize(typeof(TestClass), reader);

            Assert.AreEqual(2, deserialized.IntProp);
            Assert.AreEqual("XML", deserialized.stringField);
        }
    }
}
