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
        /*
        [TestMethod]
        public void Steps_Choose()
        {
            using (var client = TestDatabase.GetClient("choose"))
            {
                var countQuery = VertexQuery.All().Count();
                Assert.AreEqual(client.Execute(countQuery).Result[0], 0);

                var test = client.Execute(VertexQuery.All().Label().Inject("null")).Result;

                var baseQuery = VertexQuery.All();
                var findQuery = baseQuery.CreateSubQuery().Label().Inject("null");
                var insertQuery = baseQuery.CreateSubQuery().AddVertex("test");
                var existsQuery = baseQuery.CreateSubQuery().HasLabel("test");
                var query = baseQuery.Choose<GraphVertex>(findQuery, existsQuery, insertQuery);

                client.Execute(query).Wait();

                Assert.AreEqual(client.Execute(countQuery).Result[0], 1);

                client.Execute(query).Wait();

                Assert.AreEqual(client.Execute(countQuery).Result[0], 1);

                client.Execute(VertexQuery.All().Drop()).Wait();
            }
        }
        */

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
    }
}
