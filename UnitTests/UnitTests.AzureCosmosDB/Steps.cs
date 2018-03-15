using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using CuriousGremlin.Query;
using CuriousGremlin.Query.Objects;
using CuriousGremlin.Query.Predicates;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.AzureCosmosDB
{
    [TestClass]
    public class Steps
    {
        [TestMethod]
        public void Steps_Coalesce()
        {
            using (var client = TestDatabase.GetClient("coalesce"))
            {
                var countQuery = VertexQuery.All().Count();
                Assert.AreEqual(client.Execute(countQuery).Result[0], 0);

                var baseQuery = VertexQuery.All().HasLabel("test").Fold();
                var existsQuery = baseQuery.CreateSubQuery().Unfold();
                var createQuery = baseQuery.CreateSubQuery().AddVertex("test");
                var query = baseQuery.Coalesce<GraphVertex>(existsQuery, createQuery);

                // client.Execute("g.V().has('person','name','bill').fold().coalesce(unfold(),addV('person').property('name', 'bill'))").Wait();

                client.Execute(query).Wait();

                Assert.AreEqual(client.Execute(countQuery).Result[0], 1);

                client.Execute(query).Wait();

                Assert.AreEqual(client.Execute(countQuery).Result[0], 1);

                client.Execute(VertexQuery.All().Drop()).Wait();
            }
        }

        [TestMethod]
        public void Steps_Coin()
        {
            using (var client = TestDatabase.GetClient("coin"))
            {
                var countQuery = VertexQuery.All().Count();
                Assert.AreEqual(client.Execute(countQuery).Result[0], 0);

                var insertQuery = VertexQuery.Create("test");
                client.Execute(insertQuery).Wait();
                Assert.AreEqual(client.Execute(countQuery).Result[0], 1);

                client.Execute(VertexQuery.All().Coin(0.5)).Wait();

                client.Execute(VertexQuery.All().Drop()).Wait();
            }
        }

        [TestMethod]
        public void Steps_Choose()
        {
            using (var client = TestDatabase.GetClient("choose"))
            {
                var countQuery = VertexQuery.All().Count();
                Assert.AreEqual(client.Execute(countQuery).Result[0], 0);

                var insertQuery = VertexQuery.Create("test");
                client.Execute(insertQuery).Wait();
                client.Execute(insertQuery).Wait();
                Assert.AreEqual(client.Execute(countQuery).Result[0], 2);

                var baseQuery = VertexQuery.All();
                var findQuery = baseQuery.CreateSubQuery().HasLabel("new");
                var trueQuery = baseQuery.CreateSubQuery().HasLabel("new");
                var falseQuery = baseQuery.CreateSubQuery().AddVertex("new");

                var query = baseQuery.Choose<GraphVertex>(findQuery, trueQuery, falseQuery);

                client.Execute(query).Wait();

                Assert.AreEqual(client.Execute(countQuery).Result[0], 4);

                client.Execute(query).Wait();

                Assert.AreEqual(client.Execute(countQuery).Result[0], 6);

                client.Execute(VertexQuery.All().Drop()).Wait();
            }
        }

        [TestMethod]
        public void Steps_Dedup()
        {
            using (var client = TestDatabase.GetClient("coin"))
            {
                var countQuery = VertexQuery.All().Count();
                Assert.AreEqual(client.Execute(countQuery).Result[0], 0);

                var insertQuery = VertexQuery.Create("test").AddProperty("key", "value");
                client.Execute(insertQuery).Wait();
                client.Execute(insertQuery).Wait();
                Assert.AreEqual(client.Execute(countQuery).Result[0], 2);

                var values = client.Execute(VertexQuery.All().Values<string>("key").Dedup()).Result;

                Assert.AreEqual(values.Count, 1);

                client.Execute(VertexQuery.All().Drop()).Wait();
            }
        }

        [TestMethod]
        public void Steps_Fold()
        {
            using (var client = TestDatabase.GetClient("fold"))
            {
                var countQuery = VertexQuery.All().Count();
                Assert.AreEqual(client.Execute(countQuery).Result[0], 0);

                var insertQuery = VertexQuery.Create("test");
                client.Execute(insertQuery).Wait();
                client.Execute(insertQuery).Wait();


                var query = VertexQuery.All().Fold();
                var result = client.Execute(VertexQuery.All().Fold()).Result;

                client.Execute(VertexQuery.All().Drop()).Wait();
            }
        }

        [TestMethod]
        public void Steps_Label()
        {
            using (var client = TestDatabase.GetClient("label"))
            {
                var countQuery = VertexQuery.All().Count();
                Assert.AreEqual(client.Execute(countQuery).Result[0], 0);

                var insertQuery = VertexQuery.Create("test");
                client.Execute(insertQuery).Wait();
                Assert.AreEqual(client.Execute(countQuery).Result[0], 1);

                var result = client.Execute(VertexQuery.All().Label()).Result;
                Assert.AreEqual(result.Count, 1);
                Assert.AreEqual(result[0], "test");

                client.Execute(VertexQuery.All().Drop()).Wait();
            }
        }

        [TestMethod]
        public void Steps_Limit()
        {
            using (var client = TestDatabase.GetClient("limit"))
            {
                var countQuery = VertexQuery.All().Count();
                Assert.AreEqual(client.Execute(countQuery).Result[0], 0);

                var insertQuery = VertexQuery.Create("test");
                client.Execute(insertQuery).Wait();
                client.Execute(insertQuery).Wait();
                client.Execute(insertQuery).Wait();
                Assert.AreEqual(client.Execute(countQuery).Result[0], 3);

                var result = client.Execute(VertexQuery.All().Limit(2)).Result;
                Assert.AreEqual(result.Count, 2);

                client.Execute(VertexQuery.All().Drop()).Wait();
            }
        }

        [TestMethod]
        public void Steps_Max()
        {
            using (var client = TestDatabase.GetClient("max"))
            {
                var countQuery = VertexQuery.All().Count();
                Assert.AreEqual(client.Execute(countQuery).Result[0], 0);

                client.Execute(VertexQuery.Create("test").AddProperty("age", 50)).Wait();
                client.Execute(VertexQuery.Create("test").AddProperty("age", 40)).Wait();
                Assert.AreEqual(client.Execute(countQuery).Result[0], 2);

                var result = client.Execute(VertexQuery.All().Values<long>("age").Max()).Result;
                Assert.AreEqual(result.Count, 1);
                Assert.AreEqual(result[0], 50);

                client.Execute(VertexQuery.All().Drop()).Wait();
            }
        }

        [TestMethod]
        public void Steps_Mean()
        {
            using (var client = TestDatabase.GetClient("mean"))
            {
                var countQuery = VertexQuery.All().Count();
                Assert.AreEqual(client.Execute(countQuery).Result[0], 0);

                client.Execute(VertexQuery.Create("test").AddProperty("age", 50)).Wait();
                client.Execute(VertexQuery.Create("test").AddProperty("age", 40)).Wait();
                Assert.AreEqual(client.Execute(countQuery).Result[0], 2);

                var result = client.Execute(VertexQuery.All().Values<long>("age").Mean()).Result;
                Assert.AreEqual(result.Count, 1);
                Assert.AreEqual(result[0], 45);

                client.Execute(VertexQuery.All().Drop()).Wait();
            }
        }

        [TestMethod]
        public void Steps_Min()
        {
            using (var client = TestDatabase.GetClient("min"))
            {
                var countQuery = VertexQuery.All().Count();
                Assert.AreEqual(client.Execute(countQuery).Result[0], 0);

                client.Execute(VertexQuery.Create("test").AddProperty("age", 50)).Wait();
                client.Execute(VertexQuery.Create("test").AddProperty("age", 40)).Wait();
                Assert.AreEqual(client.Execute(countQuery).Result[0], 2);

                var result = client.Execute(VertexQuery.All().Values<long>("age").Min()).Result;
                Assert.AreEqual(result.Count, 1);
                Assert.AreEqual(result[0], 40);

                client.Execute(VertexQuery.All().Drop()).Wait();
            }
        }

        [TestMethod]
        public void Steps_Range()
        {
            using (var client = TestDatabase.GetClient("range"))
            {
                var countQuery = VertexQuery.All().Count();
                Assert.AreEqual(client.Execute(countQuery).Result[0], 0);

                var insertQuery = VertexQuery.Create("test");
                client.Execute(insertQuery).Wait();
                client.Execute(insertQuery).Wait();
                client.Execute(insertQuery).Wait();
                client.Execute(insertQuery).Wait();
                Assert.AreEqual(client.Execute(countQuery).Result[0], 4);

                var result = client.Execute(VertexQuery.All().Range(1,3)).Result;
                Assert.AreEqual(result.Count, 2);

                client.Execute(VertexQuery.All().Drop()).Wait();
            }
        }

        [TestMethod]
        public void Steps_Repeat()
        {
            using (var client = TestDatabase.GetClient("repeat"))
            {
                var countQuery = VertexQuery.All().Count();
                Assert.AreEqual(client.Execute(countQuery).Result[0], 0);

                var insertQuery = VertexQuery.Create("test");
                client.Execute(insertQuery).Wait();
                client.Execute(insertQuery).Wait();

                var baseQuery = VertexQuery.All().Fold();
                var actionQuery = baseQuery.CreateSubQuery().AddVertex("test");
                var query = baseQuery.Repeat(actionQuery, 2);

                client.Execute(query).Wait();

                Assert.AreEqual(client.Execute(countQuery).Result[0], 4);

                client.Execute(VertexQuery.All().Drop()).Wait();
            }
        }

        [TestMethod]
        public void Steps_Tail()
        {
            using (var client = TestDatabase.GetClient("tail"))
            {
                var countQuery = VertexQuery.All().Count();
                Assert.AreEqual(client.Execute(countQuery).Result[0], 0);

                var insertQuery = VertexQuery.Create("test");
                client.Execute(insertQuery).Wait();
                client.Execute(insertQuery).Wait();
                client.Execute(insertQuery).Wait();
                client.Execute(insertQuery).Wait();
                Assert.AreEqual(client.Execute(countQuery).Result[0], 4);

                var result = client.Execute(VertexQuery.All().Tail(2)).Result;
                Assert.AreEqual(result.Count, 2);

                client.Execute(VertexQuery.All().Drop()).Wait();
            }
        }
    }
}
