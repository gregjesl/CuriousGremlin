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

        /// <summary>
        /// Injects a value into the collection
        /// </summary>
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

        /// <summary>
        /// Filters results based on the provided value
        /// </summary>
        public Query Is(object value)
        {
            Steps.Add("is(" + GetObjectString(value) + ")");
            return this as Query;
        }

        /// <summary>
        /// Filters results based on the provided condition
        /// </summary>
        public Query Is(GraphPredicate predicate)
        {
            Steps.Add("is(" + predicate.ToString() + ")");
            return this as Query;
        }

        /// <summary>
        /// Returns the keys of the collection
        /// </summary>
        /// <returns>The key.</returns>
        public StringQuery<From> Key()
        {
            Steps.Add("key()");
            return new StringQuery<From>(this);
        }

        /*
        public Query Math(string mapping)
        {
            Steps.Add("math(" + Sanitize(mapping) + ")");
            return this as Query;
        }
        */

        /// <summary>
        /// Returns the maximum value of the collection
        /// </summary>
        public Query Max()
        {
            Steps.Add("max()");
            return this as Query;
        }

        /// <summary>
        /// Returns the average value of the collection
        /// </summary>
        public Query Mean()
        {
            Steps.Add("mean()");
            return this as Query;
        }

        /// <summary>
        /// Returns the minimum value of the collection
        /// </summary>
        public Query Min()
        {
            Steps.Add("min()");
            return this as Query;
        }

        /// <summary>
        /// Sorts the collection
        /// </summary>
        public Query Order(bool ascending = true)
        {
            string step = "order().by(";
            step += ascending ? "incr" : "decr";
            step += ")";
            Steps.Add(step);
            return this as Query;
        }

        /// <summary>
        /// Returns the sum of the values
        /// </summary>
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
