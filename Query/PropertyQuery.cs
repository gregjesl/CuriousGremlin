using System;
using System.Collections.Generic;
using System.Text;
using CuriousGremlin.Query.CRTP;
using CuriousGremlin.Query.Objects;

namespace CuriousGremlin.Query
{
    public class PropertyQuery<From> : PropertyQueryTemplate<From, PropertyQuery<From>>
    {
        internal PropertyQuery(ITraversalQuery<From> query) : base(query) { }

        internal PropertyQuery() : base() { }

        public static implicit operator PropertyQuery<From>(CollectionQuery<GraphProperty, From> query)
        {
            return new PropertyQuery<From>(query);
        }
    }

    public class PropertyQuery : PropertyQueryTemplate<GraphQuery, PropertyQuery>
    {
        internal PropertyQuery() : base() { }

        public PropertyQuery(ITraversalQuery<GraphQuery, GraphProperty> query) : base(query) { }

        public static implicit operator PropertyQuery(CollectionQuery<GraphProperty, GraphQuery> query)
        {
            return new PropertyQuery(query);
        }

        public static implicit operator PropertyQuery(PropertyQuery<GraphQuery> query)
        {
            return new PropertyQuery(query);
        }
    }
}
