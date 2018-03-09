using System;
using System.Collections.Generic;
using System.Text;
using CuriousGremlin.Query.Predicates;

namespace CuriousGremlin.Query
{
    public class CollectionQuery<T> : GraphQuery where T: CollectionQuery<T>
    {
        internal CollectionQuery(string query) : base(query)
        {
            // Type shouldBeType = typeof(CollectionQuery<T>);
            // if (GetType() != shouldBeType)
            //     throw new ArgumentException("Generic type must be the same type as the instance");
        }

        public T Aggregate(string label)
        {
            Query += ".aggregate('" + Sanitize(label) + "')";
            return this as T;
        }

        public BooleanQuery Any()
        {
            return Count().Is(new GPGreaterThanOrEqualTo(1));
        }

        public T As(string label)
        {
            Query += "as('" + Sanitize(label) + "')";
            return this as T;
        }

        public T Barrier()
        {
            Query += ".barrier()";
            return this as T;
        }

        public ValueQuery Count()
        {
            Query += ".count()";
            return new ValueQuery(Query);
        }

        public GraphQuery Choose(BooleanQuery condition, GraphQuery TrueQuery, GraphQuery FalseQuery)
        {
            Query += ".choose(" + condition.ToString() + ", " + TrueQuery.ToString() + ", " + FalseQuery.ToString() + ")";
            return this;
        }

        public ValueQuery Constant(string value)
        {
            Query += ".constant('" + Sanitize(value) + "')";
            return new ValueQuery(Query);
        }

        public T Coin(double probability)
        {
            if (probability < 0.0 || probability > 1.0)
                throw new ArgumentException("Probability must be between 0 and 1");
            Query += string.Format(".coin({0})", probability);
            return this as T;
        }

        public T CyclicPath()
        {
            Query += ".cyclicPath()";
            return this as T;
        }

        public T Dedup()
        {
            Query += ".dedup()";
            return this as T;
        }

        public TerminalQuery Drop()
        {
            Query += ".drop()";
            return new TerminalQuery(Query);
        }

        public ListQuery Fold()
        {
            Query += ".fold()";
            return new ListQuery(Query);
        }

        public BooleanQuery HasNext()
        {
            Query += ".hasNext()";
            return new BooleanQuery(Query);
        }

        public T Limit(int limit)
        {
            if (limit < 0)
                throw new ArgumentException("Limit must be at least 0");
            Query += string.Format(".limit({0})", limit);
            return this as T;
        }

        public T Next()
        {
            Query += ".next()";
            return this as T;
        }

        public T Next(int count)
        {
            if (count < 1)
                throw new ArgumentException("Count must be greater than zero");
            Query += string.Format(".next('{0}')", count);
            return this as T;
        }

        public T OrderBy(string property, bool ascending = true)
        {
            Query += ".order().by('" + Sanitize(property) + "', ";
            Query += ascending ? "incr" : "decr";
            Query += ")";
            return this as T;
        }

        public ListQuery Path()
        {
            Query += ".path()";
            return new ListQuery(Query);
        }

        public T Range(int lowerBound, int upperBound)
        {
            if (lowerBound < 0)
                throw new ArgumentException("Lower bound cannot be less than zero");
            if (upperBound < lowerBound)
                throw new ArgumentException("Upper bound must be greater than or equal to the lower bound");
            Query += string.Format(".range({0},{1})", lowerBound, upperBound);
            return this as T;
        }

        public T Sample(int samples)
        {
            if (samples < 1)
                throw new ArgumentException("Number of samples must be greater than zero");
            Query += string.Format(".sample({0})", samples);
            return this as T;
        }

        public DictionaryQuery Select(params string[] items)
        {
            if (items.Length < 1)
                throw new ArgumentException("Must have at least one item to select");
            Query += ".select(";
            var itemList = new List<string>();
            foreach(var item in items)
            {
                itemList.Add("'" + Sanitize(item) + "'");
            }
            Query += string.Join(", ", itemList);
            Query += ")";
            return new DictionaryQuery(Query);
        }

        public DictionaryQuery SelectBy(string label, params string[] items)
        {
            if (label == null || label == "")
                throw new ArgumentException("Label cannot be empty or null");
            if (items.Length < 1)
                throw new ArgumentException("Must have at least one item to select");
            Query += ".select(";
            var itemList = new List<string>();
            foreach (var item in items)
            {
                itemList.Add("'" + Sanitize(item) + "'");
            }
            Query += string.Join(", ", itemList);
            Query += ")";
            Query += ".by('" + Sanitize(label) + "')";
            return new DictionaryQuery(Query);
        }

        public T SimplePath()
        {
            Query += ".simplePath()";
            return this as T;
        }

        public T Tail(int limit)
        {
            if (limit < 0)
                throw new ArgumentException("Limit cannot be less than zero");
            Query += string.Format(".tail({0})", limit);
            return this as T;
        }

        public T ToBulkSet()
        {
            Query += ".toBulkSet()";
            return this as T;
        }

        public T ToList()
        {
            Query += ".toList()";
            return this as T;
        }

        public T ToSet()
        {
            Query += ".toSet()";
            return this as T;
        }

        public T SubQuery()
        {
            var subquery = new CollectionQuery<T>("__");
            return subquery as T;
        }
    }
}
