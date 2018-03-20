using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;
using CuriousGremlin.Query;
using CuriousGremlin.Query.Objects;
using CuriousGremlin.Query.Predicates;

namespace UnitTests.Query
{
    [TestClass]
    public class Serialization
    {
        [TestMethod]
        public void Query_SerializeVertexObject()
        {
            var item = new SerializationTestObject();
            var result = VertexQuery.Create(item);
            var test = Regex.Replace(result.ToString(), @"\s+", "");
            Assert.IsTrue(test.Contains(@"'testString','testo\'mally'"));
            Assert.IsTrue(test.Contains("'testBool',true"));
            Assert.IsTrue(test.Contains("'testFloat',1.2"));
            Assert.IsTrue(test.Contains("'testDouble',1.4"));
            Assert.IsTrue(test.Contains("'testDecimal',1.6"));
            Assert.IsTrue(test.Contains("'testInt',11"));
            Assert.IsTrue(test.Contains("'testLong',111"));
            Assert.IsTrue(test.Contains("'testDateTime','" + (new DateTime(2011, 05, 25, 10, 0, 0)).ToString("s") + "'"));
            Assert.IsTrue(test.Contains("'testRandom','" + (new TimeSpan(11, 11, 11)).ToString() + "'"));
        }

        [TestMethod]
        public void Query_DeserializeVertexObject()
        {
            var vertex = new GraphVertex()
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

        [TestMethod]
        public void Query_SerializeEdgeObject()
        {
            var item = new SerializationTestObject();
            var result = VertexQuery.Create(item);
            var test = Regex.Replace(result.ToString(), @"\s+", "");
            Assert.IsTrue(test.Contains(@"'testString','testo\'mally'"));
            Assert.IsTrue(test.Contains("'testBool',true"));
            Assert.IsTrue(test.Contains("'testFloat',1.2"));
            Assert.IsTrue(test.Contains("'testDouble',1.4"));
            Assert.IsTrue(test.Contains("'testDecimal',1.6"));
            Assert.IsTrue(test.Contains("'testInt',11"));
            Assert.IsTrue(test.Contains("'testLong',111"));
            Assert.IsTrue(test.Contains("'testDateTime','" + (new DateTime(2011, 05, 25, 10, 0, 0)).ToString("s") + "'"));
            Assert.IsTrue(test.Contains("'testRandom','" + (new TimeSpan(11, 11, 11)).ToString() + "'"));
        }

        [TestMethod]
        public void Query_DeserializeEdgeObject()
        {
            var edge = new GraphEdge()
            {
                id = Guid.NewGuid().ToString(),
                label = "test",
                properties = new Dictionary<string, object>()
            };
            edge.properties.Add("testString", @"test_deserialization o\'mally");
            edge.properties.Add("testBool", true);
            edge.properties.Add("testFloat", 1.2);
            edge.properties.Add("testDouble", 1.4);
            edge.properties.Add("testDecimal", 1.6);
            edge.properties.Add("testInt", 11);
            edge.properties.Add("testLong", 111);
            edge.properties.Add("testDateTime", (new DateTime(2011, 05, 25, 10, 0, 0)).ToString("s"));
            edge.properties.Add("testRandom", (new TimeSpan(11, 11, 11)).ToString());

            var result = edge.Deserialize<TestClass>();
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

    class SerializationTestObject : IVertexObject, IEdgeObject
    {
        public string VertexLabel { get { return "test_serialization"; } }
        public string EdgeLabel { get { return "test_edge_serialization"; } }
        public string testString = "test o'mally";
        public bool testBool = true;
        public float testFloat = 1.2f;
        public double testDouble = 1.4;
        public decimal testDecimal = 1.6m;
        public int testInt = 11;
        public long testLong = 111;
        public DateTime testDateTime = new DateTime(2011, 05, 25, 10, 0, 0);
        public TimeSpan testRandom = new TimeSpan(11, 11, 11);
    }
}
