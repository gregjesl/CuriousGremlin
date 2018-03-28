using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using CuriousGremlin;
using CuriousGremlin.Objects;
using CuriousGremlin.Predicates;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CuriousGremlin.Client;

namespace CuriousGremlin.UnitTests
{
    [TestClass]
    public class Steps
    {

        [TestMethod]
        public void Steps_AddE()
        {
            using (var client = TestDatabase.GetClient("addE"))
            {
                // Setup
                var vertex1 = client.Execute(VertexQuery.Create("vertex1")).Result;
                var vertex2 = client.Execute(VertexQuery.Create("vertex2")).Result;
                // Test
                var query = VertexQuery.All().HasLabel("vertex1").AddEdge("edge1", DirectionStep.To(vertex2.First().id));
                client.Execute(query).Wait();
                // Verify
                Assert.AreEqual(client.Execute(VertexQuery.All().HasLabel("vertex1").Out()).Result.First().id, vertex2.First().id);
            }
        }

        [TestMethod]
        public void Steps_AddV()
        {
            using (var client = TestDatabase.GetClient("addV"))
            {
                var properties = new Dictionary<string, object>();
                properties.Add("key1", "value1");
                properties.Add("key2", "value2");
                Assert.AreEqual(client.Execute(VertexQuery.Create("test", properties)).Result.Count(), 1);
                Assert.AreEqual(client.Execute(VertexQuery.All()).Result.Count(), 1);
                client.Execute(VertexQuery.All().AddVertex("test2", properties)).Wait();
                Assert.AreEqual(client.Execute(VertexQuery.All()).Result.Count(), 2);
            }
        }

        [TestMethod]
        public void Steps_Aggregate()
        {
            using (var client = TestDatabase.GetClient("aggregate"))
            {
                client.Execute(VertexQuery.Create("test")).Wait();
               
                var query = VertexQuery.All().Aggregate("x").Where(new GPWithout(new List<object>() { "x" }));

                Assert.AreEqual(client.Execute(query).Result.Count(), 0);
            }
        }

        [TestMethod]
        public void Steps_And()
        {
            using (var client = TestDatabase.GetClient("and"))
            {
                client.Execute(VertexQuery.Create("test").AddProperty("age", 30).AddProperty("name", "steve")).Wait();
                client.Execute(VertexQuery.Create("test")).Wait();

                var baseQuery = VertexQuery.All();
                var query = VertexQuery.All().And(baseQuery.CreateSubQuery().Values("age").Is(30), baseQuery.CreateSubQuery().Values("name").Is("steve"));

                var result = client.Execute(query).Result;
                Assert.AreEqual(result.Count(), 1);
                Assert.AreEqual(client.Execute(VertexQuery.All()).Result.Count(), 2);
            }
        }

        [TestMethod]
        public void Steps_As()
        {
            using (var client = TestDatabase.GetClient("as"))
            {
                client.Execute(VertexQuery.Create("one")).Wait();
                client.Execute(VertexQuery.Create("two")).Wait();
                client.Execute(VertexQuery.All().HasLabel("one").AddEdge("to", DirectionStep.To(VertexQuery.Find("two")))).Wait();

                var query = VertexQuery.All().As("a").Out().As("b").Select("a", "b");

                var result = client.Execute(query).Result;
                Assert.AreEqual(result.Count(), 1);
                Assert.IsTrue(result.First().ContainsKey("a"));
                Assert.AreEqual(result.First()["a"].label, "one");
                Assert.IsTrue(result.First().ContainsKey("b"));
                Assert.AreEqual(result.First()["b"].label, "two");
            }
        }

        [TestMethod]
        public void Steps_Barrier()
        {
            using (var client = TestDatabase.GetClient("barrier"))
            {
                client.Execute(VertexQuery.Create("test")).Wait();

                var query = VertexQuery.All().Barrier().Count();

                var result = client.Execute(query).Result;
                Assert.AreEqual(result.First(), 1);
            }
        }

        [TestMethod]
        public void Steps_Choose()
        {
            using (var client = TestDatabase.GetClient("choose"))
            {
                var countQuery = VertexQuery.All().Count();
                Assert.AreEqual(client.Execute(countQuery).Result.First(), 0);

                var insertQuery = VertexQuery.Create("test");
                client.Execute(insertQuery).Wait();
                client.Execute(insertQuery).Wait();
                Assert.AreEqual(client.Execute(countQuery).Result.First(), 2);

                var baseQuery = VertexQuery.All();
                var findQuery = baseQuery.CreateSubQuery().HasLabel("new");
                var trueQuery = baseQuery.CreateSubQuery().HasLabel("new");
                var falseQuery = baseQuery.CreateSubQuery().AddVertex("new");

                var query = baseQuery.Choose<GraphVertex>(findQuery, trueQuery, falseQuery);

                client.Execute(query).Wait();

                Assert.AreEqual(client.Execute(countQuery).Result.First(), 4);

                client.Execute(query).Wait();

                Assert.AreEqual(client.Execute(countQuery).Result.First(), 6);
            }
        }

        [TestMethod]
        public void Steps_Coalesce()
        {
            using (var client = TestDatabase.GetClient("coalesce"))
            {
                var countQuery = VertexQuery.All().Count();
                Assert.AreEqual(client.Execute(countQuery).Result.First(), 0);

                var baseQuery = VertexQuery.All().HasLabel("test").Fold();
                var existsQuery = baseQuery.CreateSubQuery().Unfold();
                var createQuery = baseQuery.CreateSubQuery().AddVertex("test");
                VertexQuery query = baseQuery.Coalesce<GraphVertex>(existsQuery, createQuery);

                // client.Execute("g.V().has('person','name','bill').fold().coalesce(unfold(),addV('person').property('name', 'bill'))").Wait();

                client.Execute(query).Wait();

                Assert.AreEqual(client.Execute(countQuery).Result.First(), 1);

                client.Execute(query).Wait();

                Assert.AreEqual(client.Execute(countQuery).Result.First(), 1);

                client.Execute(VertexQuery.All().Drop()).Wait();
            }
        }

        /*
         * Currently not supported by Azure CosmosDB
         * 
        [TestMethod]
        public void Steps_Coin()
        {
            using (var client = TestDatabase.GetClient("coin"))
            {
                var countQuery = VertexQuery.All().Count();
                Assert.AreEqual(client.Execute(countQuery).Result.First(), 0);

                var insertQuery = VertexQuery.Create("test");
                client.Execute(insertQuery).Wait();
                Assert.AreEqual(client.Execute(countQuery).Result.First(), 1);

                client.Execute(VertexQuery.All().Coin(0.5)).Wait();

                client.Execute(VertexQuery.All().Drop()).Wait();
            }
        }
        */

        [TestMethod]
        public void Steps_Constant()
        {
            using (var client = TestDatabase.GetClient("constant"))
            {
                client.Execute(VertexQuery.Create("foo").AddProperty("name", "steve")).Wait();
                var baseQuery = VertexQuery.All();
                var conditionQuery = baseQuery.CreateSubQuery().HasLabel("bar");
                var trueQuery = baseQuery.CreateSubQuery().Values("name");
                var falseQuery = baseQuery.CreateSubQuery().Constant("unknown");

                var result = client.Execute(baseQuery.Choose(conditionQuery, trueQuery, falseQuery)).Result;
                Assert.AreEqual(result.First(), "unknown");
            }
        }

        [TestMethod]
        public void Steps_Count()
        {
            using (var client = TestDatabase.GetClient("count"))
            {
                client.Execute(VertexQuery.Create("foo")).Wait();
                var result = client.Execute(VertexQuery.All().Count()).Result;
                Assert.AreEqual(result.Count(), 1);
                Assert.AreEqual(result.First(), 1);
            }
        }

        [TestMethod]
        public void Steps_CyclicPath()
        {
            using (var client = TestDatabase.GetClient("cyclic_path"))
            {
                client.Execute(VertexQuery.Create("one")).Wait();
                client.Execute(VertexQuery.Create("two")).Wait();
                client.Execute(VertexQuery.Create("three")).Wait();
                client.Execute(VertexQuery.All().HasLabel("one").AddEdge("to", DirectionStep.To(VertexQuery.Find("two")))).Wait();
                client.Execute(VertexQuery.All().HasLabel("two").AddEdge("to", DirectionStep.To(VertexQuery.Find("three")))).Wait();

                Assert.AreEqual(client.Execute(VertexQuery.All().HasLabel("one").Both().Both().Count()).Result.First(), 2L);
                Assert.AreEqual(client.Execute(VertexQuery.All().HasLabel("one").Both().Both().CyclicPath().Count()).Result.First(), 1L);
            }
        }

        [TestMethod]
        public void Steps_Dedup()
        {
            using (var client = TestDatabase.GetClient("dedup"))
            {
                var countQuery = VertexQuery.All().Count();
                Assert.AreEqual(client.Execute(countQuery).Result.First(), 0);

                var insertQuery = VertexQuery.Create("test").AddProperty("key", "value");
                client.Execute(insertQuery).Wait();
                client.Execute(insertQuery).Wait();
                Assert.AreEqual(client.Execute(countQuery).Result.First(), 2);

                var values = client.Execute(VertexQuery.All().Values<string>("key").Dedup()).Result;

                Assert.AreEqual(values.Count(), 1);

                client.Execute(VertexQuery.All().Drop()).Wait();
            }
        }

        [TestMethod]
        public void Steps_Drop()
        {
            using (var client = TestDatabase.GetClient("drop"))
            {
                client.Execute(VertexQuery.Create("foo")).Wait();
                client.Execute(VertexQuery.All().Drop()).Wait();
                Assert.AreEqual(client.Execute(VertexQuery.All().Count()).Result.First() , 0);
            }
        }

        [TestMethod]
        public void Steps_Fold()
        {
            using (var client = TestDatabase.GetClient("fold"))
            {
                var countQuery = VertexQuery.All().Count();
                Assert.AreEqual(client.Execute(countQuery).Result.First(), 0);

                var insertQuery = VertexQuery.Create("test");
                client.Execute(insertQuery).Wait();
                client.Execute(insertQuery).Wait();


                var query = VertexQuery.All().Fold();
                var result = client.Execute(VertexQuery.All().Fold()).Result;

                client.Execute(VertexQuery.All().Drop()).Wait();
            }
        }

        [TestMethod]
        public void Steps_Has()
        {
            using (var client = TestDatabase.GetClient("has"))
            {
                client.Execute(VertexQuery.Create("foo").AddProperty("key", "value")).Wait();
                Assert.AreEqual(client.Execute(VertexQuery.All().Has("key").Count()).Result.First(), 1L);
                Assert.AreEqual(client.Execute(VertexQuery.All().Has("not_key").Count()).Result.First(), 0L);
                Assert.AreEqual(client.Execute(VertexQuery.All().Has("key", "value").Count()).Result.First(), 1L);
                Assert.AreEqual(client.Execute(VertexQuery.All().Has("not_key", "value").Count()).Result.First(), 0L);
                Assert.AreEqual(client.Execute(VertexQuery.All().Has("key", "not_value").Count()).Result.First(), 0L);
                Assert.AreEqual(client.Execute(VertexQuery.All().Has("foo", "key", "value").Count()).Result.First(), 1L);
                Assert.AreEqual(client.Execute(VertexQuery.All().Has("not_foo", "key", "value").Count()).Result.First(), 0L);
                Assert.AreEqual(client.Execute(VertexQuery.All().Has("key", new GPWithin(new List<object>() { "value" })).Count()).Result.First(), 1L);
                Assert.AreEqual(client.Execute(VertexQuery.All().Has("key", new GPWithout(new List<object>() { "value" })).Count()).Result.First(), 0L);
            }
        }

        [TestMethod]
        public void Steps_HasId()
        {
            using (var client = TestDatabase.GetClient("hasId"))
            {
                var result = client.Execute(VertexQuery.Create("foo")).Result;
                Assert.AreEqual(client.Execute(VertexQuery.All().HasId(result.First().id).Count()).Result.First(), 1);
                Assert.AreEqual(client.Execute(VertexQuery.All().HasId(Guid.NewGuid().ToString()).Count()).Result.First(), 0);
            }
        }

        [TestMethod]
        public void Steps_HasKey()
        {
            using (var client = TestDatabase.GetClient("hasKey"))
            {
                client.Execute(VertexQuery.Create("foo").AddProperty("key", "value")).Wait();
                var query = VertexQuery.All().Properties().HasKey("key");
                var result = client.Execute(query).Result;
                Assert.AreEqual(result.Count(), 1);
                Assert.AreEqual(client.Execute(VertexQuery.All().Properties().HasKey("not_key").Count()).Result.First(), 0);
            }
        }

        [TestMethod]
        public void Steps_HasValue()
        {
            using (var client = TestDatabase.GetClient("hasValue"))
            {
                client.Execute(VertexQuery.Create("foo").AddProperty("key", "value")).Wait();
                Assert.AreEqual(client.Execute(VertexQuery.All().Properties().HasValue("value").Count()).Result.First(), 1);
                Assert.AreEqual(client.Execute(VertexQuery.All().Properties().HasValue("not_value").Count()).Result.First(), 0);
            }
        }

        [TestMethod]
        public void Steps_HasNot()
        {
            using (var client = TestDatabase.GetClient("hasNot"))
            {
                client.Execute(VertexQuery.Create("foo").AddProperty("key", "value")).Wait();
                Assert.AreEqual(client.Execute(VertexQuery.All().HasNot("key").Count()).Result.First(), 0);
                Assert.AreEqual(client.Execute(VertexQuery.All().HasNot("not_key").Count()).Result.First(), 1);
            }
        }

        [TestMethod]
        public void Steps_Id()
        {
            using (var client = TestDatabase.GetClient("id"))
            {
                client.Execute(VertexQuery.Create("foo")).Wait();
                Assert.AreEqual(client.Execute(VertexQuery.All().Id()).Result.Count(), 1);
            }
        }

        [TestMethod]
        public void Steps_Is()
        {
            using (var client = TestDatabase.GetClient("is"))
            {
                client.Execute(VertexQuery.Create("foo").AddProperty("age", 30)).Wait();
                Assert.AreEqual(client.Execute(VertexQuery.All().Values("age").Is(30).Count()).Result.First(), 1);
                Assert.AreEqual(client.Execute(VertexQuery.All().Values("age").Is(40).Count()).Result.First(), 0);
                Assert.AreEqual(client.Execute(VertexQuery.All().Values("age").Is(new GPBetween(25, 35)).Count()).Result.First(), 1);
            }
        }

        [TestMethod]
        public void Steps_Inject()
        {
            using (var client = TestDatabase.GetClient("inject"))
            {
                client.Execute(VertexQuery.Create("foo").AddProperty("key", "value")).Wait();
                var result = client.Execute(VertexQuery.All().Values("key").Inject("injected_value")).Result;
                Assert.IsTrue(result.Contains("value"));
                Assert.IsTrue(result.Contains("injected_value"));
            }
        }

        [TestMethod]
        public void Steps_Key()
        {
            using (var client = TestDatabase.GetClient("key"))
            {
                client.Execute(VertexQuery.Create("foo").AddProperty("key", "value")).Wait();
                Assert.IsTrue(client.Execute(VertexQuery.All().Properties().Key()).Result.First().Contains("key"));
            }
        }

        [TestMethod]
        public void Steps_Label()
        {
            using (var client = TestDatabase.GetClient("label"))
            {
                var countQuery = VertexQuery.All().Count();
                Assert.AreEqual(client.Execute(countQuery).Result.First(), 0);

                var insertQuery = VertexQuery.Create("test");
                client.Execute(insertQuery).Wait();
                Assert.AreEqual(client.Execute(countQuery).Result.First(), 1);

                var result = client.Execute(VertexQuery.All().Label()).Result;
                Assert.AreEqual(result.Count(), 1);
                Assert.AreEqual(result.First(), "test");

                client.Execute(VertexQuery.All().Drop()).Wait();
            }
        }

        [TestMethod]
        public void Steps_Limit()
        {
            using (var client = TestDatabase.GetClient("limit"))
            {
                var countQuery = VertexQuery.All().Count();
                Assert.AreEqual(client.Execute(countQuery).Result.First(), 0);

                var insertQuery = VertexQuery.Create("test");
                client.Execute(insertQuery).Wait();
                client.Execute(insertQuery).Wait();
                client.Execute(insertQuery).Wait();
                Assert.AreEqual(client.Execute(countQuery).Result.First(), 3);

                var result = client.Execute(VertexQuery.All().Limit(2)).Result;
                Assert.AreEqual(result.Count(), 2);

                client.Execute(VertexQuery.All().Drop()).Wait();
            }
        }

        [TestMethod]
        public void Steps_Max()
        {
            using (var client = TestDatabase.GetClient("max"))
            {
                var countQuery = VertexQuery.All().Count();
                Assert.AreEqual(client.Execute(countQuery).Result.First(), 0);

                client.Execute(VertexQuery.Create("test").AddProperty("age", 50)).Wait();
                client.Execute(VertexQuery.Create("test").AddProperty("age", 40)).Wait();
                Assert.AreEqual(client.Execute(countQuery).Result.First(), 2);

                var result = client.Execute(VertexQuery.All().Values<long>("age").Max()).Result;
                Assert.AreEqual(result.Count(), 1);
                Assert.AreEqual(result.First(), 50);

                client.Execute(VertexQuery.All().Drop()).Wait();
            }
        }

        [TestMethod]
        public void Steps_Mean()
        {
            using (var client = TestDatabase.GetClient("mean"))
            {
                var countQuery = VertexQuery.All().Count();
                Assert.AreEqual(client.Execute(countQuery).Result.First(), 0);

                client.Execute(VertexQuery.Create("test").AddProperty("age", 50)).Wait();
                client.Execute(VertexQuery.Create("test").AddProperty("age", 40)).Wait();
                Assert.AreEqual(client.Execute(countQuery).Result.First(), 2);

                var result = client.Execute(VertexQuery.All().Values<long>("age").Mean()).Result;
                Assert.AreEqual(result.Count(), 1);
                Assert.AreEqual(result.First(), 45);

                client.Execute(VertexQuery.All().Drop()).Wait();
            }
        }

        [TestMethod]
        public void Steps_Min()
        {
            using (var client = TestDatabase.GetClient("min"))
            {
                var countQuery = VertexQuery.All().Count();
                Assert.AreEqual(client.Execute(countQuery).Result.First(), 0);

                client.Execute(VertexQuery.Create("test").AddProperty("age", 50)).Wait();
                client.Execute(VertexQuery.Create("test").AddProperty("age", 40)).Wait();
                Assert.AreEqual(client.Execute(countQuery).Result.First(), 2);

                var result = client.Execute(VertexQuery.All().Values<long>("age").Min()).Result;
                Assert.AreEqual(result.Count(), 1);
                Assert.AreEqual(result.First(), 40);

                client.Execute(VertexQuery.All().Drop()).Wait();
            }
        }

        [TestMethod]
        public void Steps_Not()
        {
            using (var client = TestDatabase.GetClient("not"))
            {
                client.Execute(VertexQuery.Create("foo")).Wait();
                var baseQuery = VertexQuery.All();
                var subQuery = baseQuery.CreateSubQuery().HasLabel("foo");
                Assert.AreEqual(client.Execute(baseQuery.Not(subQuery)).Result.Count(), 0);
            }
        }

        [TestMethod]
        public void Steps_Optional()
        {
            using (var client = TestDatabase.GetClient("optional"))
            {
                client.Execute(VertexQuery.Create("one")).Wait();

                var subQuery = VertexQuery.All().CreateSubQuery().Out();
                Assert.AreEqual(client.Execute(VertexQuery.All().Optional(subQuery)).Result.First().label, "one");

                client.Execute(VertexQuery.Create("two")).Wait();
                client.Execute(VertexQuery.All().HasLabel("one").AddEdge("to", DirectionStep.To(VertexQuery.Find("two")))).Wait();
                Assert.AreEqual(client.Execute(VertexQuery.All().Optional(subQuery)).Result.First().label, "two");
            }
        }

        [TestMethod]
        public void Steps_Or()
        {
            using (var client = TestDatabase.GetClient("or"))
            {
                client.Execute(VertexQuery.Create("foo")).Wait();
                var subQuery1 = VertexQuery.All().CreateSubQuery().HasLabel("bar");
                var subQuery2 = VertexQuery.All().CreateSubQuery().HasLabel("foo");
                Assert.AreEqual(client.Execute(VertexQuery.All().Or(subQuery1, subQuery2)).Result.First().label, "foo");
            }
        }

        [TestMethod]
        public void Steps_Order()
        {
            using (var client = TestDatabase.GetClient("order"))
            {
                client.Execute(VertexQuery.Create("foo").AddProperty("age", 30)).Wait();
                client.Execute(VertexQuery.Create("bar").AddProperty("age", 20)).Wait();
                var result = client.Execute(VertexQuery.All().Label().Order(true)).Result;
                Assert.AreEqual(result.First(), "bar");
                Assert.AreEqual(result.ElementAt(1), "foo");

                var vertexResult = client.Execute(VertexQuery.All().OrderBy("age", true)).Result;
                Assert.AreEqual(vertexResult.First().label, "bar");
                Assert.AreEqual(vertexResult.ElementAt(1).label, "foo");
            }
        }

        [TestMethod]
        public void Steps_Properties()
        {
            using (var client = TestDatabase.GetClient("properties"))
            {
                client.Execute(VertexQuery.Create("foo").AddProperty("age", 30)).Wait();

                var result = client.Execute(VertexQuery.All().Properties()).Result;
                Assert.AreEqual(result.Count(), 1);
                Assert.AreEqual(result.First().label, "age");
                Assert.AreEqual(result.First().value, 30L);

                result = client.Execute(VertexQuery.All().Properties("age")).Result;
                Assert.AreEqual(result.Count(), 1);
                Assert.AreEqual(result.First().label, "age");
                Assert.AreEqual(result.First().value, 30L);

                result = client.Execute(VertexQuery.All().Properties("name")).Result;
                Assert.AreEqual(result.Count(), 0);
            }
        }

        [TestMethod]
        public void Steps_PropertyMap()
        {
            using (var client = TestDatabase.GetClient("propertyMap"))
            {
                client.Execute(VertexQuery.Create("foo").AddProperty("age", 30)).Wait();

                var query = VertexQuery.All().PropertyMap();
                var result = client.Execute(query).Result;
                Assert.AreEqual(result.Count(), 1);
                Assert.IsTrue(result.First().ContainsKey("age"));
                Assert.AreEqual(result.First()["age"].Count, 1);
                Assert.AreEqual(result.First()["age"].First().label, "age");
                Assert.AreEqual(result.First()["age"].First().value, 30L);

                client.Execute(VertexQuery.Create("bar")).Wait();
                client.Execute(VertexQuery.All().HasLabel("foo").AddEdge("to", DirectionStep.To(VertexQuery.Find("bar"))).AddProperty("key", "value")).Wait();

                var edgeQuery = VertexQuery.All().OutE().PropertyMap();
                var edgeResult = client.Execute(edgeQuery).Result;
                Assert.IsTrue(edgeResult.First().ContainsKey("key"));
                Assert.AreEqual(edgeResult.First()["key"].Key, "key");
                Assert.AreEqual(edgeResult.First()["key"].Value, "value");
            }
        }

        [TestMethod]
        public void Steps_Range()
        {
            using (var client = TestDatabase.GetClient("range"))
            {
                var countQuery = VertexQuery.All().Count();
                Assert.AreEqual(client.Execute(countQuery).Result.First(), 0);

                var insertQuery = VertexQuery.Create("test");
                client.Execute(insertQuery).Wait();
                client.Execute(insertQuery).Wait();
                client.Execute(insertQuery).Wait();
                client.Execute(insertQuery).Wait();
                Assert.AreEqual(client.Execute(countQuery).Result.First(), 4);

                var result = client.Execute(VertexQuery.All().Range(1,3)).Result;
                Assert.AreEqual(result.Count(), 2);

                client.Execute(VertexQuery.All().Drop()).Wait();
            }
        }

        [TestMethod]
        public void Steps_Repeat()
        {
            using (var client = TestDatabase.GetClient("repeat"))
            {
                var countQuery = VertexQuery.All().Count();
                Assert.AreEqual(client.Execute(countQuery).Result.First(), 0);

                var insertQuery = VertexQuery.Create("test");
                client.Execute(insertQuery).Wait();
                client.Execute(insertQuery).Wait();

                var baseQuery = VertexQuery.All().Fold();
                var actionQuery = baseQuery.CreateSubQuery().AddVertex("test");
                var query = baseQuery.Repeat(actionQuery, 2);

                client.Execute(query).Wait();

                Assert.AreEqual(client.Execute(countQuery).Result.First(), 4);
            }
        }

        [TestMethod]
        public void Steps_Sample()
        {
            using (var client = TestDatabase.GetClient("sample"))
            {
                client.Execute(VertexQuery.Create("foo")).Wait();
                client.Execute(VertexQuery.Create("foo")).Wait();
                client.Execute(VertexQuery.Create("foo")).Wait();

                Assert.AreEqual(client.Execute(VertexQuery.All().Sample(1)).Result.Count(), 1);
            }
        }

        [TestMethod]
        public void Steps_Select()
        {
            using (var client = TestDatabase.GetClient("select"))
            {
                client.Execute(VertexQuery.Create("one")).Wait();
                client.Execute(VertexQuery.Create("two")).Wait();
                client.Execute(VertexQuery.All().HasLabel("one").AddEdge("to", DirectionStep.To(VertexQuery.Find("two")))).Wait();

                var query = VertexQuery.All().As("a").Out().As("b").Select("a", "b");

                var result = client.Execute(query).Result;

                Assert.IsTrue(result.First().ContainsKey("a"));
                Assert.IsTrue(result.First().ContainsKey("b"));
            }
        }

        [TestMethod]
        public void Steps_SimplePath()
        {
            using (var client = TestDatabase.GetClient("simple_path"))
            {
                client.Execute(VertexQuery.Create("one")).Wait();
                client.Execute(VertexQuery.Create("two")).Wait();
                client.Execute(VertexQuery.Create("three")).Wait();
                client.Execute(VertexQuery.All().HasLabel("one").AddEdge("to", DirectionStep.To(VertexQuery.Find("two")))).Wait();
                client.Execute(VertexQuery.All().HasLabel("two").AddEdge("to", DirectionStep.To(VertexQuery.Find("three")))).Wait();

                Assert.AreEqual(client.Execute(VertexQuery.All().HasLabel("one").Both().Both().Count()).Result.First(), 2L);
                Assert.AreEqual(client.Execute(VertexQuery.All().HasLabel("one").Both().Both().SimplePath().Count()).Result.First(), 1L);
            }
        }

        /*
         * Currently not supported by Azure CosmosDB
         * 
        [TestMethod]
        public void Steps_Skip()
        {
            using (var client = TestDatabase.GetClient("skip"))
            {
                client.Execute(VertexQuery.Create("foo").AddProperty("age", 30)).Wait();
                client.Execute(VertexQuery.Create("bar").AddProperty("age", 20)).Wait();
                var result = client.Execute(VertexQuery.All().OrderBy("age", true).Skip(1).Label()).Result;
                Assert.AreEqual(result.Count(), 1);
                Assert.AreEqual(result.First(), "foo");
            }
        }
        */

        [TestMethod]
        public void Steps_Sum()
        {
            using (var client = TestDatabase.GetClient("sum"))
            {
                client.Execute(VertexQuery.Create("foo").AddProperty("age", 30)).Wait();
                client.Execute(VertexQuery.Create("bar").AddProperty("age", 20)).Wait();
                Assert.AreEqual(client.Execute(VertexQuery.All().Values("age").Sum()).Result.First(), 50L);
            }
        }

        [TestMethod]
        public void Steps_Tail()
        {
            using (var client = TestDatabase.GetClient("tail"))
            {
                var countQuery = VertexQuery.All().Count();
                Assert.AreEqual(client.Execute(countQuery).Result.First(), 0);

                var insertQuery = VertexQuery.Create("test");
                client.Execute(insertQuery).Wait();
                client.Execute(insertQuery).Wait();
                client.Execute(insertQuery).Wait();
                client.Execute(insertQuery).Wait();
                Assert.AreEqual(client.Execute(countQuery).Result.First(), 4);

                var result = client.Execute(VertexQuery.All().Tail(2)).Result;
                Assert.AreEqual(result.Count(), 2);

                client.Execute(VertexQuery.All().Drop()).Wait();
            }
        }

        [TestMethod]
        public void Steps_Union()
        {
            using (var client = TestDatabase.GetClient("union"))
            {
                client.Execute(VertexQuery.Create("foo").AddProperty("firstName", "John").AddProperty("lastName", "Doe")).Wait();
                var query = VertexQuery.All().Union(
                    VertexQuery.All().CreateSubQuery().Values("firstName"), 
                    VertexQuery.All().CreateSubQuery().Values("lastName")
                );
                var result = client.Execute(query).Result;
                Assert.AreEqual(result.Count(), 2);
                Assert.IsTrue(result.Contains("John"));
                Assert.IsTrue(result.Contains("Doe"));
            }
        }

        [TestMethod]
        public void Steps_Value()
        {
            using (var client = TestDatabase.GetClient("value"))
            {
                client.Execute(VertexQuery.Create("foo").AddProperty("key", "value")).Wait();
                Assert.AreEqual(client.Execute(VertexQuery.All().Properties().Value()).Result.Count(), 1);
                Assert.AreEqual(client.Execute(VertexQuery.All().Properties().Value()).Result.First(), "value");
            }
        }

        [TestMethod]
        public void Steps_ValueMap()
        {
            using (var client = TestDatabase.GetClient("valueMap"))
            {
                client.Execute(VertexQuery.Create("foo").AddProperty("age", 30)).Wait();

                var vertexQuery = VertexQuery.All().ValueMap();
                var vertexResult = client.Execute(vertexQuery).Result;

                Assert.AreEqual(vertexResult.Count(), 1);
                Assert.IsTrue(vertexResult.First().ContainsKey("age"));
                Assert.AreEqual(vertexResult.First()["age"].First(), 30L);

                client.Execute(VertexQuery.Create("bar")).Wait();
                client.Execute(VertexQuery.All().HasLabel("foo").AddEdge("to", DirectionStep.To(VertexQuery.Find("bar"))).AddProperty("key", "value")).Wait();

                var edgeQuery = VertexQuery.All().OutE().ValueMap();
                var edgeResult = client.Execute(edgeQuery).Result;

                Assert.AreEqual(edgeResult.Count(), 1);
                Assert.IsTrue(edgeResult.First().ContainsKey("key"));
                Assert.AreEqual(edgeResult.First()["key"], "value");
            }
        }

        [TestMethod]
        public void Steps_Values()
        {
            using (var client = TestDatabase.GetClient("values"))
            {
                client.Execute(VertexQuery.Create("foo").AddProperty("key", "value")).Wait();
                Assert.AreEqual(client.Execute(VertexQuery.All().Values()).Result.First(), "value");
            }
        }

        [TestMethod]
        public void Steps_Where()
        {
            using (var client = TestDatabase.GetClient("where"))
            {
                client.Execute(VertexQuery.Create("one")).Wait();
                client.Execute(VertexQuery.Create("two")).Wait();
                client.Execute(VertexQuery.Create("three")).Wait();
                client.Execute(VertexQuery.All().HasLabel("one").AddEdge("to", DirectionStep.To(VertexQuery.Find("two")))).Wait();
                client.Execute(VertexQuery.All().HasLabel("two").AddEdge("to", DirectionStep.To(VertexQuery.Find("three")))).Wait();

                var query = VertexQuery.All().HasLabel("one").As("a").Both().Both().Where(new GPNotEqual("a"));
                var result = client.Execute(query).Result;
                Assert.AreEqual(result.Count(), 1);
                Assert.AreEqual(result.First().label, "three");
            }
        }
    }
}
