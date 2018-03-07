using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;
using CuriousGremlin.Query;
using CuriousGremlin.Query.Predicates;

namespace UnitTests.Query
{
    [TestClass]
    public class Serialization
    {
        [TestMethod]
        public void Query_SerializeVertexObject()
        {
            var item = new VertexSerializationTest();
            var result = GraphQuery.AddVertex(item);
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
    }

    class VertexSerializationTest : IVertexObject
    {
        public string VertexLabel { get { return "test_serialization"; } }
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
