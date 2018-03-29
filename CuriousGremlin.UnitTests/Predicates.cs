using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using CuriousGremlin;
using CuriousGremlin.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CuriousGremlin.UnitTests
{
    [TestClass]
    public class Predicates
    {
        private static int age = 30;

        [TestMethod]
        public void Predicates_EqualTo()
        {
            using (var client = TestDatabase.GetClient("predicates_eq"))
            {
                client.Execute(VertexQuery.Create("test").AddProperty("age", 30)).Wait();
                Assert.AreEqual(1, client.Execute(VertexQuery.All().Values("age").Is(GraphPredicate.EqualTo(age))).Result.Count());
                Assert.AreEqual(0, client.Execute(VertexQuery.All().Values("age").Is(GraphPredicate.EqualTo(age + 1))).Result.Count());
            }
        }

        [TestMethod]
        public void Predicates_NotEqualTo()
        {
            using (var client = TestDatabase.GetClient("predicates_neq"))
            {
                client.Execute(VertexQuery.Create("test").AddProperty("age", 30)).Wait();
                Assert.AreEqual(0, client.Execute(VertexQuery.All().Values("age").Is(GraphPredicate.NotEqualTo(age))).Result.Count());
                Assert.AreEqual(1, client.Execute(VertexQuery.All().Values("age").Is(GraphPredicate.NotEqualTo(age + 1))).Result.Count());
            }
        }

        [TestMethod]
        public void Predicates_LessThan()
        {
            using (var client = TestDatabase.GetClient("predicates_lt"))
            {
                client.Execute(VertexQuery.Create("test").AddProperty("age", 30)).Wait();
                Assert.AreEqual(1, client.Execute(VertexQuery.All().Values("age").Is(GraphPredicate.LessThan(age+1))).Result.Count());
                Assert.AreEqual(0, client.Execute(VertexQuery.All().Values("age").Is(GraphPredicate.LessThan(age - 1))).Result.Count());
            }
        }

        [TestMethod]
        public void Predicates_LessThanOrEqualTo()
        {
            using (var client = TestDatabase.GetClient("predicates_lte"))
            {
                client.Execute(VertexQuery.Create("test").AddProperty("age", 30)).Wait();
                Assert.AreEqual(1, client.Execute(VertexQuery.All().Values("age").Is(GraphPredicate.LessThanOrEqualTo(age + 1))).Result.Count());
                Assert.AreEqual(1, client.Execute(VertexQuery.All().Values("age").Is(GraphPredicate.LessThanOrEqualTo(age))).Result.Count());
                Assert.AreEqual(0, client.Execute(VertexQuery.All().Values("age").Is(GraphPredicate.LessThanOrEqualTo(age - 1))).Result.Count());
            }
        }

        [TestMethod]
        public void Predicates_GreaterThan()
        {
            using (var client = TestDatabase.GetClient("predicates_gt"))
            {
                client.Execute(VertexQuery.Create("test").AddProperty("age", 30)).Wait();
                Assert.AreEqual(0, client.Execute(VertexQuery.All().Values("age").Is(GraphPredicate.GreaterThan(age + 1))).Result.Count());
                Assert.AreEqual(1, client.Execute(VertexQuery.All().Values("age").Is(GraphPredicate.GreaterThan(age - 1))).Result.Count());
            }
        }

        [TestMethod]
        public void Predicates_GreaterThanOrEqualTo()
        {
            using (var client = TestDatabase.GetClient("predicates_gte"))
            {
                client.Execute(VertexQuery.Create("test").AddProperty("age", 30)).Wait();
                Assert.AreEqual(0, client.Execute(VertexQuery.All().Values("age").Is(GraphPredicate.GreaterThanOrEqualTo(age + 1))).Result.Count());
                Assert.AreEqual(1, client.Execute(VertexQuery.All().Values("age").Is(GraphPredicate.GreaterThanOrEqualTo(age))).Result.Count());
                Assert.AreEqual(1, client.Execute(VertexQuery.All().Values("age").Is(GraphPredicate.GreaterThanOrEqualTo(age - 1))).Result.Count());
            }
        }

        [TestMethod]
        public void Predicates_Inside()
        {
            using (var client = TestDatabase.GetClient("predicates_inside"))
            {
                client.Execute(VertexQuery.Create("test").AddProperty("age", 30)).Wait();
                Assert.AreEqual(1, client.Execute(VertexQuery.All().Values("age").Is(GraphPredicate.Inside(age - 1, age + 1))).Result.Count());
                Assert.AreEqual(0, client.Execute(VertexQuery.All().Values("age").Is(GraphPredicate.Inside(age + 1, age + 2))).Result.Count());
            }
        }

        [TestMethod]
        public void Predicates_Outside()
        {
            using (var client = TestDatabase.GetClient("predicates_outside"))
            {
                client.Execute(VertexQuery.Create("test").AddProperty("age", 30)).Wait();
                Assert.AreEqual(0, client.Execute(VertexQuery.All().Values("age").Is(GraphPredicate.Outside(age - 1, age + 1))).Result.Count());
                Assert.AreEqual(1, client.Execute(VertexQuery.All().Values("age").Is(GraphPredicate.Outside(age + 1, age + 2))).Result.Count());
            }
        }

        [TestMethod]
        public void Predicates_Within()
        {
            using (var client = TestDatabase.GetClient("predicates_within"))
            {
                int[] empty = { };
                int[] values = { age };
                int[] outValues = { age + 1 };
                client.Execute(VertexQuery.Create("test").AddProperty("age", 30)).Wait();
                Assert.ThrowsException<ArgumentException>(() => { GraphPredicate.Within(empty); });
                Assert.AreEqual(1, client.Execute(VertexQuery.All().Values("age").Is(GraphPredicate.Within(values))).Result.Count());
                Assert.AreEqual(0, client.Execute(VertexQuery.All().Values("age").Is(GraphPredicate.Within(outValues))).Result.Count());
            }
        }

        [TestMethod]
        public void Predicates_Without()
        {
            using (var client = TestDatabase.GetClient("predicates_without"))
            {
                int[] empty = { };
                int[] values = { age };
                int[] outValues = { age + 1 };
                client.Execute(VertexQuery.Create("test").AddProperty("age", 30)).Wait();
                Assert.ThrowsException<ArgumentException>(() => { GraphPredicate.Without(empty); });
                Assert.AreEqual(0, client.Execute(VertexQuery.All().Values("age").Is(GraphPredicate.Without(values))).Result.Count());
                Assert.AreEqual(1, client.Execute(VertexQuery.All().Values("age").Is(GraphPredicate.Without(outValues))).Result.Count());
            }
        }
    }
}
