using System;
using System.Collections.Generic;
using System.Text;
using CuriousGremlin.Query.Predicates;

namespace CuriousGremlin.Query.CRTP
{
    public class ValueQueryTemplate<T, From, Query> : CollectionQueryTemplate<T, From, Query>
        where Query : ValueQueryTemplate<T, From, Query>
    {
        protected ValueQueryTemplate(ITraversalQuery<From> query) : base(query) { }

        protected ValueQueryTemplate() : base() { }

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
}
