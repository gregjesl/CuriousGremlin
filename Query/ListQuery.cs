using System;
using System.Collections.Generic;
using System.Text;
using CuriousGremlin.Query.Objects;
using CuriousGremlin.Query.CRTP;

namespace CuriousGremlin.Query
{
    public class ListQuery<T, From> : ListQueryTemplate<T, From, ListQuery<T, From>>
    {
        internal ListQuery(ITraversalQuery<From> query) : base(query) { }

        internal ListQuery() : base() { }

        public static implicit operator ListQuery<T, From>(CollectionQuery<List<T>, From> query)
        {
            return new ListQuery<T, From>(query);
        }
    }

    public class ListQuery<T> : ListQuery<T, GraphQuery>
    {
        internal ListQuery(ITraversalQuery<GraphQuery> query) : base(query) { }

        internal ListQuery() : base() { }

        public static implicit operator ListQuery<T>(CollectionQuery<List<T>, GraphQuery> query)
        {
            return new ListQuery<T>(query);
        }
    }
}
