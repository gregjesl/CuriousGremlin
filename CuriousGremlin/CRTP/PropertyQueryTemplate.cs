using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using CuriousGremlin.Objects;

namespace CuriousGremlin.CRTP
{
    public class PropertyQueryTemplate<From, Query> : CollectionQueryTemplate<GraphProperty, From, Query>
        where Query : PropertyQueryTemplate<From, Query>
    {
        protected PropertyQueryTemplate(ITraversalQuery<From> query) : base(query) { }

        protected PropertyQueryTemplate() : base() { }

        /// <summary>
        /// Remove the traverser if the property does not have all of the provided keys.
        /// </summary>
        public Query HasKey(params string[] keys)
        {
            var list = new List<string>();
            foreach (var key in keys)
            {
                list.Add(GetObjectString(key));
            }
            Steps.Add("hasKey(" + string.Join(",", list) + ")");
            return this as Query;
        }

        /// <summary>
        /// Remove the traverser if the property does not have all of the provided keys.
        /// </summary>
        public Query HasKey(IEnumerable<string> keys)
        {
            return HasKey(keys.ToArray());
        }

        /// <summary>
        /// Remove the traverser if its property does not have all of the provided values.
        /// </summary>
        public Query HasValue(params string[] values)
        {
            var list = new List<string>();
            foreach (var value in values)
            {
                list.Add(GetObjectString(value));
            }
            Steps.Add("hasValue(" + string.Join(",", list) + ")");
            return this as Query;
        }

        /// <summary>
        /// Remove the traverser if its property does not have all of the provided values.
        /// </summary>
        public Query HasValue(IEnumerable<string> values)
        {
            return HasValue(values.ToArray());
        }

        /// <summary>
        /// Extract the key from the property
        /// </summary>
        public StringQuery<From> Key()
        {
            Steps.Add("key()");
            return new StringQuery<From>(this);
        }

        /// <summary>
        /// Extract the value from the property
        /// </summary>
        public ValueQuery<object, From> Value()
        {
            Steps.Add("value()");
            return new ValueQuery<object, From>(this);
        }
    }
}
