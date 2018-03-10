using System;
using System.Collections.Generic;
using System.Text;
using CuriousGremlin.Query.Predicates;

namespace CuriousGremlin.Query
{
    public class CollectionQuery<From,To,Query> : GraphQuery<From,To,Query> where Query: CollectionQuery<From,To,Query>
    {
        internal CollectionQuery(IGraphQuery query) : base(query) { }

        public Query Aggregate(string label)
        {
            Steps.Add("aggregate('" + Sanitize(label) + "')");
            return this as Query;
        }

        public BooleanQuery<From> Any()
        {
            return Count().Is(new GPGreaterThanOrEqualTo(1));
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

        public ValueQuery<From> Count()
        {
            Steps.Add("count()");
            return new ValueQuery<From>(this);
        }

        /*
        public GraphQuery Choose(BooleanQuery condition, GraphQuery TrueQuery, GraphQuery FalseQuery)
        {
            throw new NotImplementedException();
        }
        */

        public ValueQuery<From> Constant(string value)
        {
            Steps.Add("constant('" + Sanitize(value) + "')");
            return new ValueQuery<From>(this);
        }

        public Query Coin(double probability)
        {
            if (probability < 0.0 || probability > 1.0)
                throw new ArgumentException("Probability must be between 0 and 1");
            Steps.Add(string.Format("coin({0})", probability));
            return this as Query;
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

        public ListQuery<From> Fold()
        {
            Steps.Add("fold()");
            return new ListQuery<From>(this);
        }

        public BooleanQuery<From> HasNext()
        {
            Steps.Add("hasNext()");
            return new BooleanQuery<From>(this);
        }

        public Query Limit(int limit)
        {
            if (limit < 0)
                throw new ArgumentException("Limit must be at least 0");
            Steps.Add(string.Format("limit({0})", limit));
            return this as Query;
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

        public Query OrderBy(string property, bool ascending = true)
        {
            string step = "order().by('" + Sanitize(property) + "', ";
            step += ascending ? "incr" : "decr";
            step += ")";
            Steps.Add(step);
            return this as Query;
        }

        public ListQuery<From> Path()
        {
            Steps.Add("path()");
            return new ListQuery<From>(this);
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

        public Query Sample(int samples)
        {
            if (samples < 1)
                throw new ArgumentException("Number of samples must be greater than zero");
            Steps.Add(string.Format("sample({0})", samples));
            return this as Query;
        }

        public DictionaryQuery<From> Select(params string[] items)
        {
            if (items.Length < 1)
                throw new ArgumentException("Must have at least one item to select");
            string step = "select(";
            var itemList = new List<string>();
            foreach(var item in items)
            {
                itemList.Add("'" + Sanitize(item) + "'");
            }
            step += string.Join(", ", itemList);
            step += ")";
            Steps.Add(step);
            return new DictionaryQuery<From>(this);
        }

        public DictionaryQuery<From> SelectBy(string label, params string[] items)
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
            return new DictionaryQuery<From>(this);
        }

        public Query SimplePath()
        {
            Steps.Add("simplePath()");
            return this as Query;
        }

        public Query Tail(int limit)
        {
            if (limit < 0)
                throw new ArgumentException("Limit cannot be less than zero");
            Steps.Add(string.Format("tail({0})", limit));
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
    }
}
