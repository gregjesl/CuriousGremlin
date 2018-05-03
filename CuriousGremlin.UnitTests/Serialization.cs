using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using CuriousGremlin;
using CuriousGremlin.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CuriousGremlin.Client;
using System.Runtime.Serialization;

namespace CuriousGremlin.UnitTests
{
    [TestClass]
    public class Serialization
    {
        [DataContract]
        public class SerializationDataContractTestObject : IVertexObject, IEdgeObject
        {
            public string ID { set; get; }
            public string VertexLabel { get { return "test_serialization"; } }
            public string EdgeLabel { get { return "test_edge_serialization"; } }
            [DataMember(Name = "tEsTsTrInG")]
            public string testString { set; get; }
            [DataMember]
            public bool testBool { set; get; }
            [DataMember]
            public float testFloat { set; get; }
            [DataMember]
            public double testDouble { set; get; }
            [DataMember]
            public decimal testDecimal { set; get; }
            [DataMember]
            public int testInt { set; get; }
            [DataMember]
            public long testLong { set; get; }
            [DataMember]
            public string testNull { set; get; }
            [DataMember]
            public int[] testList0 { set; get; }
            [DataMember]
            public int[] testList1 { set; get; }
            [DataMember]
            public int[] testList2 { set; get; }
            [DataMember]
            public DateTime testDateTime { set; get; }
            [DataMember]
            public TimeSpan testRandom { set; get; }
        }

        public static SerializationDataContractTestObject GetPopulatedObject()
        {
            return new SerializationDataContractTestObject()
            {
                testString = "test o'mally",
                testBool = true,
                testFloat = 1.2f,
                testDouble = 1.4,
                testDecimal = 1.6m,
                testInt = 11,
                testLong = 111,
                testNull = null,
                testList0 = new int[0],
                testList1 = new int[1] { 1 },
                testList2 = new int[2] { 1, 2 },
                testDateTime = new DateTime(2011, 05, 25, 10, 0, 0),
                testRandom = new TimeSpan(11, 11, 11)
            };
        }

        [TestMethod]
        public void Query_VertexSerialization()
        {
            using (var client = TestDatabase.GetClient("vertex_serialization"))
            {
                client.Execute(VertexQuery.Create(GetPopulatedObject())).Wait();
                var vertex = client.Execute(VertexQuery.All()).Result.FirstOrDefault();
                Assert.IsNotNull(vertex);
                var result = vertex.Deserialize<SerializationDataContractTestObject>();
                var truth = GetPopulatedObject();
                Assert.AreEqual(truth.testString, result.testString);
                Assert.AreEqual(truth.testBool, result.testBool);
                Assert.AreEqual(truth.testFloat, result.testFloat);
                Assert.AreEqual(truth.testDouble, result.testDouble);
                Assert.AreEqual(truth.testDecimal, result.testDecimal);
                Assert.AreEqual(truth.testInt, result.testInt);
                Assert.AreEqual(truth.testLong, result.testLong);
                Assert.AreEqual(null, result.testList0);
                Assert.AreEqual(1, result.testList1.Count());
                Assert.IsTrue(result.testList1.Contains(truth.testList1.First()));
                Assert.AreEqual(2, result.testList2.Count());
                Assert.IsTrue(result.testList2.Contains(truth.testList2[0]));
                Assert.IsTrue(result.testList2.Contains(truth.testList2[1]));
                Assert.AreEqual(truth.testDateTime, result.testDateTime);
                Assert.AreEqual(truth.testRandom, result.testRandom);
            }
        }
    }
}
