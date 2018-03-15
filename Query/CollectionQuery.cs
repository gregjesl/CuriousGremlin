using System;
using System.Collections.Generic;
using System.Text;
using CuriousGremlin.Query.Objects;
using CuriousGremlin.Query.Predicates;
using Newtonsoft.Json.Linq;

namespace CuriousGremlin.Query
{
    public class CollectionQuery<T, From, Query> : TraversalQuery<From, T>
        where Query: CollectionQuery<T, From, Query>
    {
        public enum RepeatTypeEnum { DoWhile, WhileDo };

        protected CollectionQuery(ITraversalQuery<From> query)
        {
            if (query is null)
                throw new ArgumentNullException("Step list cannot be null");
            Steps = query.Steps;
        }

        public CollectionQuery() : base() { }

        public static implicit operator CollectionQuery<T, From, Query>(CollectionQuery<T, From> query)
        {
            return new CollectionQuery<T, From, Query>(query);
        }

        public VertexQuery<From> AddVertex(string label)
        {
            return AddVertex(label, new Dictionary<string, object>());
        }

        public VertexQuery<From> AddVertex(Dictionary<string, object> properties)
        {
            return AddVertex(null, properties);
        }

        public VertexQuery<From> AddVertex(string label, Dictionary<string, object> properties)
        {
            string step = "addV(";
            if (label != null && label != "")
                step += "'" + Sanitize(label) + "'";

            if (properties.Count > 0)
            {
                step += ", " + SeralizeProperties(properties);
            }
            step += ")";
            Steps.Add(step);
            return new VertexQuery<From>(this);
        }

        public VertexQuery<From> AddVertex(IVertexObject vertex)
        {
            var properties = JObject.FromObject(vertex).ToObject<Dictionary<string, object>>();
            foreach (var item in properties)
            {
                if (item.Value is null)
                    properties.Remove(item.Key);
            }
            properties.Remove("VertexLabel");
            return AddVertex(vertex.VertexLabel, properties);
        }

        public Query Aggregate(string label)
        {
            Steps.Add("aggregate('" + Sanitize(label) + "')");
            return this as Query;
        }

        public Query And(params ITraversalQuery<From, T>[] conditions)
        {
            return And(conditions);
        }

        public Query And<TOutput>(params ITraversalQuery<From, TOutput>[] conditions)
        {
            return And<TOutput>(conditions);
        }

        public Query And(IEnumerable<ITraversalQuery<From, T>> conditions)
        {
            string step = "and(";
            List<string> stepStrings = new List<string>();
            foreach (var condition in conditions)
            {
                stepStrings.Add(condition.ToString());
            }
            step += string.Join(",", stepStrings);
            step += ")";
            return this as Query;
        }

        public Query And<TOutput>(IEnumerable<ITraversalQuery<From, TOutput>> conditions)
        {
            string step = "and(";
            List<string> stepStrings = new List<string>();
            foreach (var condition in conditions)
            {
                stepStrings.Add(condition.ToString());
            }
            step += string.Join(",", stepStrings);
            step += ")";
            return this as Query;
        }

        public Query As(string label)
        {
            Steps.Add("as('" + Sanitize(label) + "')");
            return this as Query;
        }

        public Query Barrier()
        {
            Steps.Add("barrier()");
            return this as Query;
        }

        /*
        public StringQuery<From> By(string name)
        {
            Steps.Add("by('" + Sanitize(name) + "')");
            return new StringQuery<From>(this);
        }

        public DictionaryQuery<long, List<T>, From> ByCount()
        {
            Steps.Add("by(outE().count())");
            return new DictionaryQuery<long, List<T>, From>(this);
        }


        public DictionaryQuery<long, T> ByEdge()
        {
            Steps.Add("by(outE().count())");
            return new DictionaryQuery<long, T>(this);
        }
        */

        public CollectionQuery<T, From> Choose(ITraversalQuery<T> condition, ITraversalQuery<T, T> TrueQuery, ITraversalQuery<T, T> FalseQuery)
        {
            Steps.Add("choose(" + condition.ToString() + ", __." + TrueQuery.ToString() + ", __." + FalseQuery + ")");
            return new CollectionQuery<T, From>(this);
        }

        public CollectionQuery<TOutput, From> Choose<TOutput>(ITraversalQuery<T> condition, ITraversalQuery<T, TOutput> TrueQuery, ITraversalQuery<T, TOutput> FalseQuery)
        {
            Steps.Add("choose(" + condition.ToString() + ", __." + TrueQuery.ToString() + ", __." + FalseQuery + ")");
            return new CollectionQuery<TOutput, From>(this);
        }

        public IntegerQuery<From> Count()
        {
            Steps.Add("count()");
            return new IntegerQuery<From>(this);
        }

        public Query Coalesce(params ITraversalQuery<T, T>[] paths)
        {
            string step = "coalesce(";
            List<string> pathStrings = new List<string>();
            foreach (var path in paths)
            {
                pathStrings.Add(path.ToString());
            }
            step += string.Join(",", pathStrings);
            step += ")";
            Steps.Add(step);
            return this as Query;
        }

        public CollectionQuery<TOutput, From> Coalesce<TOutput>(params ITraversalQuery<T, TOutput>[] paths)
        {
            string step = "coalesce(";
            List<string> pathStrings = new List<string>();
            foreach (var path in paths)
            {
                pathStrings.Add(path.ToString());
            }
            step += string.Join(",", pathStrings);
            step += ")";
            Steps.Add(step);
            return new CollectionQuery<TOutput, From>(this);
        }

        public Query Coin(double probability)
        {
            if (probability < 0.0 || probability > 1.0)
                throw new ArgumentException("Probability must be between 0 and 1");
            Steps.Add(string.Format("coin({0})", probability));
            return this as Query;
        }

        public CollectionQuery<T, T> CreateSubQuery()
        {
            return new CollectionQuery<T, T>();
        }

        public Query CyclicPath()
        {
            Steps.Add("cyclicPath()");
            return this as Query;
        }

        public Query Dedup()
        {
            Steps.Add("dedup()");
            return this as Query;
        }

        public TerminalQuery<From> Drop()
        {
            Steps.Add("drop()");
            return new TerminalQuery<From>(this);
        }

        public ListQuery<T, From> Fold()
        {
            Steps.Add("fold()");
            return new ListQuery<T, From>(this);
        }

        public DictionaryQuery<string, T, From> Group(string property)
        {
            Steps.Add("group().by('" + Sanitize(property) + "')");
            return new DictionaryQuery<string, T, From>(this);
        }

        public Query HasNext()
        {
            Steps.Add("hasNext()");
            return this as Query;
        }

        public Query Limit(int limit)
        {
            if (limit < 0)
                throw new ArgumentException("Limit must be at least 0");
            Steps.Add(string.Format("limit({0})", limit));
            return this as Query;
        }

        public CollectionQuery<TOutput, From> Local<TOutput>(ITraversalQuery<T, TOutput> localQuery)
            where TOutput : IGraphOutput
        {
            Steps.Add("local(" + localQuery.ToString() + ")");
            return new CollectionQuery<TOutput, From>(this);
        }

        public Query Next()
        {
            Steps.Add("next()");
            return this as Query;
        }

        public Query Next(int count)
        {
            if (count < 1)
                throw new ArgumentException("Count must be greater than zero");
            Steps.Add(string.Format("next({0})", count));
            return this as Query;
        }

        public Query Not(ITraversalQuery<T, T> subquery)
        {
            Steps.Add("not(" + subquery.ToString() + ")");
            return this as Query;
        }

        public Query Optional(ITraversalQuery<T, T> subquery)
        {
            Steps.Add("optional(" + subquery.ToString() + ")");
            return this as Query;
        }

        public CollectionQuery<TOutput, From> Or<TOutput>(params ITraversalQuery<T, TOutput>[] paths)
            where TOutput : IGraphOutput
        {
            string step = "or(";
            var pathList = new List<string>();
            foreach (var path in paths)
            {
                pathList.Add("__." + path.ToString());
            }
            step += string.Join(", ", pathList);
            Steps.Add(step);
            return new CollectionQuery<TOutput, From>(this);
        }

        public Query OrderBy(string property, bool ascending = true)
        {
            string step = "order().by('" + Sanitize(property) + "', ";
            step += ascending ? "incr" : "decr";
            step += ")";
            Steps.Add(step);
            return this as Query;
        }

        public ListQuery<object, From> Path()
        {
            Steps.Add("path()");
            return new ListQuery<object, From>(this);
        }

        public DictionaryQuery<string, object, From> PropertyMap()
        {
            Steps.Add("propertyMap()");
            return new DictionaryQuery<string, object, From>(this);
        }

        public DictionaryQuery<string, object, From> PropertyMap(params string[] keys)
        {
            string step = "propertyMap(";
            var itemList = new List<string>();
            foreach (var item in keys)
            {
                itemList.Add("'" + Sanitize(item) + "'");
            }
            step += string.Join(", ", itemList);
            step += ")";
            Steps.Add(step);
            return new DictionaryQuery<string, object, From>(this);
        }

        public Query Range(int lowerBound, int upperBound)
        {
            if (lowerBound < 0)
                throw new ArgumentException("Lower bound cannot be less than zero");
            if (upperBound < lowerBound)
                throw new ArgumentException("Upper bound must be greater than or equal to the lower bound");
            Steps.Add(string.Format("range({0},{1})", lowerBound, upperBound));
            return this as Query;
        }

        public Query Repeat(ITraversalQuery<IGraphOutput, IGraphOutput> traversal, int count)
        {
            if (count < 1)
                throw new ArgumentException("Repeat count must be greater than 0");
            if (traversal.Steps.Count < 1)
                throw new ArgumentException("Provided traversal must contain at least one step");
            Steps.Add("repeat(" + traversal.ToString() + ").times(" + count.ToString() + ")");
            return this as Query;
        }

        public Query Repeat<TOutput>(ITraversalQuery<T, TOutput> traversal, ITraversalQuery<T> condition, RepeatTypeEnum type)
        {
            if (traversal.Steps.Count < 1)
                throw new ArgumentException("Provided traversal must contain at least one step");
            if (condition.Steps.Count < 1)
                throw new ArgumentException("Provided condition must have at least one step");
            string until = "until(" + condition.ToString() + ")";
            string repeat = "repeat(" + traversal.ToString() + ")";
            switch (type)
            {
                case RepeatTypeEnum.DoWhile:
                    Steps.Add(repeat + "." + until);
                    break;
                case RepeatTypeEnum.WhileDo:
                    Steps.Add(until + "." + repeat);
                    break;
            }
            return this as Query;
        }

        public Query Sample(int samples)
        {
            if (samples < 1)
                throw new ArgumentException("Number of samples must be greater than zero");
            Steps.Add(string.Format("sample({0})", samples));
            return this as Query;
        }

        public DictionaryQuery<string, T, From> Select(params string[] items)
        {
            if (items.Length < 1)
                throw new ArgumentException("Must have at least one item to select");
            string step = "select(";
            var itemList = new List<string>();
            foreach (var item in items)
            {
                itemList.Add("'" + Sanitize(item) + "'");
            }
            step += string.Join(", ", itemList);
            step += ")";
            Steps.Add(step);
            return new DictionaryQuery<string, T, From>(this);
        }

        public DictionaryQuery<string, T, From> SelectBy(string label, params string[] items)
        {
            if (label == null || label == "")
                throw new ArgumentException("Label cannot be empty or null");
            if (items.Length < 1)
                throw new ArgumentException("Must have at least one item to select");
            string step = "select(";
            var itemList = new List<string>();
            foreach (var item in items)
            {
                itemList.Add("'" + Sanitize(item) + "'");
            }
            step += string.Join(", ", itemList);
            step += ")";
            step += ".by('" + Sanitize(label) + "')";
            Steps.Add(step);
            return new DictionaryQuery<string, T, From>(this);
        }

        public Query SimplePath()
        {
            Steps.Add("simplePath()");
            return this as Query;
        }

        public Query Skip(int number)
        {
            Steps.Add("skip(" + number.ToString() + ")");
            return this as Query;
        }

        public Query Tail(int limit)
        {
            if (limit < 0)
                throw new ArgumentException("Limit cannot be less than zero");
            Steps.Add(string.Format("tail({0})", limit));
            return this as Query;
        }

        public Query TimeLimit(int milliseconds)
        {
            if (milliseconds <= 0)
                throw new ArgumentException("Time must be greater than zero");
            Steps.Add(string.Format("timeLimit({0})", milliseconds));
            return this as Query;
        }

        public Query ToBulkSet()
        {
            Steps.Add("toBulkSet()");
            return this as Query;
        }

        public Query ToList()
        {
            Steps.Add("toList()");
            return this as Query;
        }

        public Query ToSet()
        {
            Steps.Add("toSet()");
            return this as Query;
        }

        public CollectionQuery<TOutput, From> Union<TOutput>(params ITraversalQuery<T, TOutput>[] paths)
            where TOutput : IGraphOutput
        {
            if (paths.Length < 1)
                throw new ArgumentException("Must have at least one path");
            string step = "union(";
            var itemList = new List<string>();
            foreach (var item in paths)
            {
                itemList.Add("__." + item.ToString());
            }
            step += string.Join(", ", itemList);
            step += ")";
            Steps.Add(step);
            return new CollectionQuery<TOutput, From>(this);
        }

        public Query Where(ITraversalQuery<T> condition)
        {
            Steps.Add("where(" + condition.ToString() + ")");
            return this as Query;
        }
    }

    public class CollectionQuery<T, From> : CollectionQuery<T, From, CollectionQuery<T, From>>
    {
        internal CollectionQuery(ITraversalQuery<From> query) : base(query) { }

        internal CollectionQuery() : base() { }
    }
}
