using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using CuriousGremlin.AzureCosmosDB;
using CuriousGremlin.Query;
using CuriousGremlin.Query.Objects;
using CuriousGremlin.Query.Predicates;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.AzureCosmosDB
{
    [TestClass]
    public class VertexSteps
    {
        private void Setup_Test_Database(GraphClient client)
        {
            client.Execute(VertexQuery.Create("one")).Wait();
            client.Execute(VertexQuery.Create("two")).Wait();
            client.Execute(VertexQuery.Create("three")).Wait();
            client.Execute(VertexQuery.All().HasLabel("one").AddEdge("one_to_two", VertexQuery.Find("two"))).Wait();
            client.Execute(VertexQuery.All().HasLabel("two").AddEdge("two_to_three", VertexQuery.Find("three"))).Wait();
        }

        [TestMethod]
        public void Steps_Vertex_Out()
        {
            using (var client = TestDatabase.GetClient("steps_vertex_out"))
            {
                Setup_Test_Database(client);
                Assert.AreEqual(client.Execute(VertexQuery.All().HasLabel("one").Out()).Result[0].label, "two");
            }
        }

        [TestMethod]
        public void Steps_Vertex_In()
        {
            using (var client = TestDatabase.GetClient("steps_vertex_in"))
            {
                Setup_Test_Database(client);
                Assert.AreEqual(client.Execute(VertexQuery.All().HasLabel("two").In()).Result[0].label, "one");
            }
        }

        [TestMethod]
        public void Steps_Vertex_Both()
        {
            using (var client = TestDatabase.GetClient("steps_vertex_both"))
            {
                Setup_Test_Database(client);
                var result = client.Execute(VertexQuery.All().HasLabel("two").Both()).Result;
                Assert.AreEqual(result.Count, 2);
                Assert.IsTrue(result.Exists(v => v.label == "one"));
                Assert.IsTrue(result.Exists(v => v.label == "three"));
            }
        }

        [TestMethod]
        public void Steps_Vertex_OutE()
        {
            using (var client = TestDatabase.GetClient("steps_vertex_outE"))
            {
                Setup_Test_Database(client);
                Assert.AreEqual(client.Execute(VertexQuery.All().HasLabel("one").OutE()).Result[0].label, "one_to_two");
            }
        }

        [TestMethod]
        public void Steps_Vertex_InE()
        {
            using (var client = TestDatabase.GetClient("steps_vertex_inE"))
            {
                Setup_Test_Database(client);
                Assert.AreEqual(client.Execute(VertexQuery.All().HasLabel("two").InE()).Result[0].label, "one_to_two");
            }
        }

        [TestMethod]
        public void Steps_Vertex_BothE()
        {
            using (var client = TestDatabase.GetClient("steps_vertex_bothE"))
            {
                Setup_Test_Database(client);
                var result = client.Execute(VertexQuery.All().HasLabel("two").BothE()).Result;
                Assert.AreEqual(result.Count, 2);
                Assert.IsTrue(result.Exists(v => v.label == "one_to_two"));
                Assert.IsTrue(result.Exists(v => v.label == "two_to_three"));
            }
        }

        [TestMethod]
        public void Steps_Vertex_OutV()
        {
            using (var client = TestDatabase.GetClient("steps_vertex_outV"))
            {
                Setup_Test_Database(client);
                Assert.AreEqual(client.Execute(VertexQuery.All().HasLabel("one").OutE().OutV()).Result[0].label, "one");
            }
        }

        [TestMethod]
        public void Steps_Vertex_InV()
        {
            using (var client = TestDatabase.GetClient("steps_vertex_inV"))
            {
                Setup_Test_Database(client);
                Assert.AreEqual(client.Execute(VertexQuery.All().HasLabel("two").InE().InV()).Result[0].label, "one");
            }
        }

        [TestMethod]
        public void Steps_Vertex_BothV()
        {
            using (var client = TestDatabase.GetClient("steps_vertex_bothV"))
            {
                Setup_Test_Database(client);
                var result = client.Execute(VertexQuery.All().HasLabel("two").OutE().BothV()).Result;
                Assert.AreEqual(result.Count, 2);
                Assert.IsTrue(result.Exists(v => v.label == "two"));
                Assert.IsTrue(result.Exists(v => v.label == "three"));
            }
        }

        [TestMethod]
        public void Steps_Vertex_OtherV()
        {
            using (var client = TestDatabase.GetClient("steps_vertex_otherV"))
            {
                Setup_Test_Database(client);
                Assert.AreEqual(client.Execute(VertexQuery.All().HasLabel("two").OutE().OtherV()).Result[0].label, "three");
            }
        }
    }
}
