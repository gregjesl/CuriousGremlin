using System;
using System.Collections.Generic;
using System.Text;
using CuriousGremlin.Query;
using CuriousGremlin.Query.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.AzureCosmosDB
{
    [TestClass]
    public class Steps
    {
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

        [TestMethod]
        public void Steps_Repeat()
        {
            using (var client = TestDatabase.GetClient("repeat"))
            {
                var countQuery = VertexQuery.All().Count();
                Assert.AreEqual(client.Execute(countQuery).Result[0], 0);

                var test = client.Execute(VertexQuery.All().Label().Inject("null")).Result;

                var baseQuery = VertexQuery.All();
                var conditionQuery = baseQuery.CreateSubQuery().HasLabel("test");
                var actionQuery = baseQuery.CreateSubQuery().AddVertex("test");
                var query = baseQuery.Repeat(actionQuery, conditionQuery, CollectionQuery<GraphElement, Graph, GraphVertex, VertexQuery>.RepeatTypeEnum.WhileDo);

                client.Execute("g.V().has('person','name','bill').coalesce(has('person','name','bill'),addV('person').property('name', 'bill'))").Wait();

                Assert.AreEqual(client.Execute(countQuery).Result[0], 1);

                client.Execute(query).Wait();

                Assert.AreEqual(client.Execute(countQuery).Result[0], 1);

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

                VertexQuery start = VertexQuery.All().HasLabel("test");
                ListQuery<GraphVertex, Graph> baseQuery = start.Fold();
                var existsQuery = baseQuery.CreateSubQuery().Unfold();
                var createQuery = baseQuery.CreateSubQuery().AddVertex("test");
                var query = baseQuery.Coalesce<GraphVertex>(existsQuery, createQuery);

                client.Execute("g.V().has('person','name','bill').coalesce(has('person','name','bill'),addV('person').property('name', 'bill'))").Wait();

                Assert.AreEqual(client.Execute(countQuery).Result[0], 1);

                client.Execute(query).Wait();

                Assert.AreEqual(client.Execute(countQuery).Result[0], 1);

                client.Execute(VertexQuery.All().Drop()).Wait();
            }
        }
    }
}
