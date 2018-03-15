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

        public Query Constant(T value)
        {
            Steps.Add("constant(" + GetObjectString(value) + ")");
            return this as Query;
        }

        public Query Inject(params T[] values)
        {
            string step = "inject(";
            List<string> valueList = new List<string>();
            foreach (var item in values)
            {
                valueList.Add(GetObjectString(item));
            }
            step += string.Join(",", valueList);
            step += ")";
            Steps.Add(step);
            return this as Query;
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

        public StringQuery<From> Key()
        {
            Steps.Add("key()");
            return new StringQuery<From>(this);
        }

        public Query Math(string mapping)
        {
            Steps.Add("math(" + Sanitize(mapping) + ")");
            return this as Query;
        }

        public Query Max()
        {
            Steps.Add("max()");
            return this as Query;
        }

        public Query Mean()
        {
            Steps.Add("mean()");
            return this as Query;
        }

        public Query Min()
        {
            Steps.Add("min()");
            return this as Query;
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

        public static implicit operator ValueQuery<T, From>(StringQuery<From> query)
        {
            return new ValueQuery<T, From>(query);
        }

        public static implicit operator ValueQuery<T, From>(IntegerQuery<From> query)
        {
            return new ValueQuery<T, From>(query);
        }
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
