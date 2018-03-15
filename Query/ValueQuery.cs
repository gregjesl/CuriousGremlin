using System;
using System.Collections.Generic;
using System.Text;
using CuriousGremlin.Query.Predicates;
using CuriousGremlin.Query.Objects;

namespace CuriousGremlin.Query
{
    public class ValueQuery<T, From, Query> : CollectionQuery<T, From, Query>
        where Query: ValueQuery<T, From, Query>
    {
        protected ValueQuery(ITraversalQuery<From> query) : base(query) { }

        internal ValueQuery() : base() { }

        public static implicit operator ValueQuery<T, From, Query>(ValueQuery<T, From> query)
        {
            return new ValueQuery<T, From, Query>(query);
        }

        public Query Is(float value)
        {
            Steps.Add("is(" + GetObjectString(value) + ")");
            return this as Query;
        }

        public Query Is(double value)
        {
            Steps.Add("is(" + GetObjectString(value) + ")");
            return this as Query;
        }

        public Query Is(decimal value)
        {
            Steps.Add("is(" + GetObjectString(value) + ")");
            return this as Query;
        }

        public Query Is(int value)
        {
            Steps.Add("is(" + GetObjectString(value) + ")");
            return this as Query;
        }

        public Query Is(long value)
        {
            Steps.Add("is(" + GetObjectString(value) + ")");
            return this as Query;
        }

        public Query Is(GraphPredicate predicate)
        {
            Steps.Add("is(" + predicate.ToString() + ")");
            return this as Query;
        }

        public ValueQuery<string> Key()
        {
            Steps.Add("key()");
            return new ValueQuery<string>(this);
        }

        public ValueQuery<T> Math(string mapping)
        {
            Steps.Add("math(" + Sanitize(mapping) + ")");
            return this;
        }

        public ValueQuery<T> Max()
        {
            Steps.Add("max()");
            return this;
        }

        public ValueQuery<T, From> Mean()
        {
            Steps.Add("mean()");
            return this;
        }

        public ValueQuery<T, From> Min()
        {
            Steps.Add("min()");
            return this;
        }

        public Query Sum()
        {
            Steps.Add("sum()");
            return this as Query;
        }
    }

    public class ValueQuery<T, From> : ValueQuery<T, From, ValueQuery<T, From>>
    {
        internal ValueQuery(ITraversalQuery<From> query) : base(query) { }

        internal ValueQuery() : base() { }
    }

    public class StringQuery<From> : ValueQuery<string, From>
    {
        internal StringQuery(ITraversalQuery<From> query) : base(query) { }

        internal StringQuery() : base() { }
    }

    public class IntegerQuery<From> : ValueQuery<long, From>
    {
        internal IntegerQuery(ITraversalQuery<From> query) : base(query) { }

        internal IntegerQuery() : base() { }
    }
}
