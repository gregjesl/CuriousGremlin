using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CuriousGremlin.GraphSON;

namespace UnitTests.GraphSON
{
    [TestClass]
    public class Deserialization
    {
        [TestMethod]
        public void GraphSON_Vertex_Deserialization()
        {
            var vertex = new Vertex()
            {
                id = Guid.NewGuid().ToString(),
                label = "test",
                properties = new Dictionary<string, List<KeyValuePair<string, object>>>()
            };
            vertex.properties.Add("testString", new List<KeyValuePair<string, object>> { new KeyValuePair<string, object>("value", @"test_deserialization o\'mally") });
            vertex.properties.Add("testBool", new List<KeyValuePair<string, object>> { new KeyValuePair<string, object>("value", true) });
            vertex.properties.Add("testFloat", new List<KeyValuePair<string, object>> { new KeyValuePair<string, object>("value", 1.2) });
            vertex.properties.Add("testDouble", new List<KeyValuePair<string, object>> { new KeyValuePair<string, object>("value", 1.4) });
            vertex.properties.Add("testDecimal", new List<KeyValuePair<string, object>> { new KeyValuePair<string, object>("value", 1.6) });
            vertex.properties.Add("testInt", new List<KeyValuePair<string, object>> { new KeyValuePair<string, object>("value", 11) });
            vertex.properties.Add("testLong", new List<KeyValuePair<string, object>> { new KeyValuePair<string, object>("value", 111) });
            vertex.properties.Add("testDateTime", new List<KeyValuePair<string, object>> { new KeyValuePair<string, object>("value", (new DateTime(2011, 05, 25, 10, 0, 0)).ToString("s")) });
            vertex.properties.Add("testRandom", new List<KeyValuePair<string, object>> { new KeyValuePair<string, object>("value", (new TimeSpan(11, 11, 11)).ToString()) });

            var result = vertex.Deserialize<TestClass>();
            Assert.AreEqual(result.testString, "test_deserialization o'mally");
            Assert.IsTrue(result.testBool);
            Assert.AreEqual(result.testFloat, 1.2f);
            Assert.AreEqual(result.testDouble, 1.4);
            Assert.AreEqual(result.testDecimal, 1.6m);
            Assert.AreEqual(result.testInt, 11);
            Assert.AreEqual(result.testLong, 111L);
            Assert.AreEqual(result.testDateTime, new DateTime(2011, 05, 25, 10, 0, 0));
            Assert.AreEqual(result.testRandom, new TimeSpan(11, 11, 11));
        }

        public class TestClass
        {
            public string testString { set; get; }
            public bool testBool { set; get; }
            public float testFloat { set; get; }
            public double testDouble { set; get; }
            public decimal testDecimal { set; get; }
            public int testInt { set; get; }
            public long testLong { set; get; }
            public DateTime testDateTime { set; get; }
            public TimeSpan testRandom { set; get; }
        }
    }
}
